using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Interfaces;
using AdCourier.Infrastructure.Data;
using AdCourier.Services;
using AdCourier.Services.Interfaces;
using Ajkerdeal.Helpers;
using Cm.Infrastructure.Data;
using Crm.Domain.Interfaces;
using Crm.Services;
using Crm.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MsgPack.Serialization;
using Retention.Domain.Interfaces;
using Retention.Infrastructure.Data;
using Retention.Services;
using Retention.Services.Interfaces;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Ajkerdeal
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IServiceCollection _services { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            RegisterCorsPolicies();

            services.Configure<ConnectionStringList>(Configuration.GetSection("ConnectionString"));
            services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(Configuration["ConnectionString:MsSqlConnection"]));

            // Redis
            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddSingleton(redisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<IRedisDefaultCacheClient, RedisDefaultCacheClient>();
            services.AddSingleton<StackExchange.Redis.Extensions.Core.ISerializer, StackExchange.Redis.Extensions.MsgPack.MsgPackObjectSerializer>();


            //API Versioning
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            // Sending email using SmtpClient 

            //services.AddTransient<SmtpClient>((serviceProvider) =>
            //{
            //    var config = serviceProvider.GetRequiredService<IConfiguration>();
            //    return new SmtpClient()
            //    {
            //        Host = config.GetValue<String>("Email:Smtp:Host"),
            //        Port = config.GetValue<int>("Email:Smtp:Port"),
            //        Credentials = new NetworkCredential(
            //                config.GetValue<String>("Email:Smtp:Username"),
            //                config.GetValue<String>("Email:Smtp:Password")
            //            )
            //    };
            //});

            services.AddScoped<SmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClient()
                {
                    Host = config.GetValue<String>("Email:Smtp:Host"),
                    Port = config.GetValue<int>("Email:Smtp:Port"),
                    Credentials = new NetworkCredential(
                            config.GetValue<String>("Email:Smtp:Username"),
                            config.GetValue<String>("Email:Smtp:Password")
                        )
                };
            });


            services.AddMvc();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info
                {
                    Version = "v1.0",
                    Title = "API",
                    Description = "Versioned Api v1.0 ASP.NET Core 2.0",
                    TermsOfService = "None",
                    Contact = new Contact()
                    {
                        Name = "Dotnet Detail",
                        Email = "porag214@gmail.com",
                        Url = "www.test.net"
                    },
                    License = new License
                    {
                        Name = "ABC",
                        Url = "www.test.net"
                    },
                });

                c.SwaggerDoc("v1.1", new Info { Title = "Versioned Api v1.1", Version = "v1.1" });


                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                });


                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);

                // Get xml comments path
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                // Set xml path
                c.IncludeXmlComments(xmlPath);

            });

            // Use GzipCompression.
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            //services.AddResponseCompression();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };

                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            // Use GzipCompression.

            // off now
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
            //        // Apply CORS policy for any type of origin  
            //        .AllowAnyMethod()
            //        // Apply CORS policy for any type of http methods  
            //        .AllowAnyHeader()
            //        // Apply CORS policy for any headers  
            //        .AllowCredentials());
            //    // Apply CORS policy for all users  
            //});


            // configure strongly typed settings objects


            var redisModel = Configuration.GetSection("RedisModel");
            services.Configure<RedisModel>(redisModel);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);


            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Or you can also register as follows

            services.AddHttpContextAccessor();

            // Register application services.
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<ILoginRepository, LoginRepository>();

            services.AddTransient<IWeightRangeService, WeightRangeService>();
            services.AddTransient<IWeightRangeRepository, WeightRangeRepository>();

            services.AddTransient<IDeliveryRangeService, DeliveryRangeService>();
            services.AddTransient<IDeliveryRangeRepository, DeliveryRangeRepository>();

            services.AddTransient<IDeliveryChargeDetailsService, DeliveryChargeDetailsService>();
            services.AddTransient<IDeliveryChargeDetailsRepository, DeliveryChargeDetailsRepository>();

            services.AddTransient<IBreakableService, BreakableService>();
            services.AddTransient<IBreakableRepository, BreakableRepository>();

            services.AddTransient<IOrderTrackingService, OrderTrackingService>();
            services.AddTransient<IOrderTrackingRepository, OrderTrackingRepository>();

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISettingsRepository, SettingsRepository>();

            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();

            services.AddTransient<IReturnService, ReturnService>();
            services.AddTransient<IReturnRepository, ReturnRepository>();

            services.AddTransient<IOrderReportService, OrderReportService>();
            services.AddTransient<IOrderReportRepository, OrderReportRepository>();

            services.AddTransient<IPackagingChargeRangeService, PackagingChargeRangeService>();
            services.AddTransient<IPackagingChargeRangeRepository, PackagingChargeRangeRepository>();

            services.AddTransient<IBondhuService, BondhuService>();
            services.AddTransient<IBondhuRepository, BondhuRepository>();

            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();

            services.AddTransient<IGenerateLinkService, GenerateLinkService>();
            services.AddTransient<IGenerateLinkRepository, GenerateLinkRepository>();


            services.AddTransient<IImageProcessingService, ImageProcessingService>();
            services.AddTransient<IImageProcessingRepository, ImageProcessingRepository>();

            services.AddTransient<IOrderHistoryRepository, OrderRepository>();

            services.AddTransient<ISmsEmailService, SmsEmailService>();
            services.AddTransient<IJwtSecurityService, JwtSecurityService>();
            services.AddTransient<IExcelService, ExcelService>();

            services.AddTransient<IOrderGenericRepository, OrderGenericRepository>();
            services.AddTransient<IFirebaseCloudService, FirebaseCloudService>();


            services.AddTransient<IOrderRequestService,OrderRequestService>();
            services.AddTransient<IOrderRequestRepository,OrderRequestRepository>();

            services.AddTransient<IDashBoardPrivateAppService, DashBoardPrivateAppService>();
            services.AddTransient<IDashBoardPrivateAppRepository, DashBoardPrivateAppRepository>();

            services.AddTransient<IQuickOrderService, QuickOrderService>();
            services.AddTransient<IQuickOrderRepository, QuickOrderRepository>();

            //District Section
            services.AddTransient<IDistrictInfoService, DistrictInfoService>();
            services.AddTransient<IDistrictInfoRepository, DistrictInfoRepository>();

            //Loan Section
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<ILoanRepository, LoanRepository>();

            // Dana
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ITestRepository, TestRepository>();

            // Test
            services.AddTransient<IDanaService, DanaService>();
            services.AddTransient<IDanaRepository, DanaRepository>();

            services.AddTransient<OtherService>();
            //services.AddTransient<IGenericRepository<OrderModel>, GenericRepository<OrderModel>>();

            //Crm Section
            services.AddTransient<ICrmOrderService, CrmOrderService>();
            services.AddTransient<ICrmOrderRepository, CrmOrderRepository>();
            services.AddTransient<IMerchantService, MerchantService>();
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            //Retention Section
            services.AddTransient<IRetentionService, RetentionService>();
            services.AddTransient<IRetentionRepository, RetentionRepository>();

            //Voucher Section
            services.AddTransient<IVoucherService, VoucherService>();
            services.AddTransient<IVoucherRepository, VoucherRepository>();

            services.AddTransient<IInstantCodService, InstantCodService>();
            services.AddTransient<IInstantCodRepository, InstantCodRepository>();

            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient<IIntegrationRepository, IntegrationRepository>();
        }


        private void RegisterCorsPolicies()
        {
            string[] localHostOrigins = new string[] {
                "http://localhost:4200",
                "https://localhost:4200",
                "http://localhost:65497",
                "https://localhost:65497",
                "http://localhost:51301",
                "http://localhost:9151",
                "http://localhost:49283",
                "http://127.0.0.1:5500/",
                "http://localhost:58676/",
                "http://localhost:8885",
                "http://localhost:52406",
                "http://localhost:4200",
                "http://localhost:30809",
                "http://localhost:44379",
                "https://localhost:44379"
            };

            string[] productionHostOrigins = new string[] {
                "http://deliverytiger.ajkerdeal.com", "https://deliverytiger.ajkerdeal.com",
                "http://deliverytiger.com.bd", "https://deliverytiger.com.bd",
                "http://ajkerdeal.com", "https://ajkerdeal.com",
                "http://localhost:4200","https://localhost:4200",
                "http://localhost:65497","https://localhost:65497",
                "http://localhost:49283", "https://localhost:49283",
                "http://localhost:8885", "https://localhost:8885",
                "http://localhost:9151", "https://localhost:9151",
                "http://localhost:4200","https://localhost:4200",
                "http://localhost:30809", "https://localhost:30809",
                "http://content.ajkerdeal.com","https://content.ajkerdeal.com",
                "http://thirdparty.ajkerdeal.com", "https://thirdparty.ajkerdeal.com",
                "http://crm.ajkerdeal.com","https://crm.ajkerdeal.com",
                "http://fulfillment.ajkerdeal.com","https://fulfillment.ajkerdeal.com",
                "http://test2.ajkerdeal.com", "https://test2.ajkerdeal.com",
                "http://testdeliverytiger.deliverytiger.com.bd", "https://testdeliverytiger.deliverytiger.com.bd",
                "http://crmcore.ajkerdeal.com", "https://crmcore.ajkerdeal.com",
                "http://complain.ajkerdeal.com","https://complain.ajkerdeal.com",

                "http://admin.deliverytiger.com.bd","https://admin.deliverytiger.com.bd",
                "http://home.deliverytiger.com.bd","https://home.deliverytiger.com.bd",
                "http://admin.ajkerdeal.com","https://admin.ajkerdeal.com",
                "http://login.deliverytiger.com.bd","https://login.deliverytiger.com.bd",
                "http://m.ajkerdeal.com","https://m.ajkerdeal.com",
                "http://localhost:44379","https://localhost:44379",
                "http://lp.deliverytiger.com.bd", "https://lp.deliverytiger.com.bd",
                "http://altthirdparty.ajkerdeal.com","https://altthirdparty.ajkerdeal.com"
            };

            _services.AddCors(options =>    // CORS middleware must precede any defined endpoints
            {
                options.AddPolicy("DevelopmentCorsPolicy", builder =>
                {
                    builder.WithOrigins(localHostOrigins)
                            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
                options.AddPolicy("ProductionCorsPolicy", builder =>
                {
                    builder.WithOrigins(productionHostOrigins)
                            .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("DevelopmentCorsPolicy");
                app.UseDeveloperExceptionPage();
            }

            
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1");
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned Api v1.0");
                c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Versioned Api v1.1");
            });

            app.UseCors("ProductionCorsPolicy");
            //app.UseCors("CorsPolicy");

            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
