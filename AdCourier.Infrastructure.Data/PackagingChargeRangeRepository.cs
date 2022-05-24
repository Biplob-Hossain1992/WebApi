using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class PackagingChargeRangeRepository : IPackagingChargeRangeRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        public PackagingChargeRangeRepository(SqlServerContext sqlServerContext, IOrderHistoryRepository orderHistoryRepository)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _orderHistoryRepository = orderHistoryRepository;
        }
        public async Task<PackagingChargeRange> AddPackagingChargeRange(PackagingChargeRange packagingChargeRange)
        {
            await _sqlServerContext.PackagingChargeRange.AddAsync(packagingChargeRange);
            await _sqlServerContext.SaveChangesAsync();
            return packagingChargeRange;
        }
        public async Task<IEnumerable<PackagingChargeRange>> GetPackagingChargeRange(bool onlyActive)
        {
            //if (onlyActive == true)
            //{
            //    return await _sqlServerContext.PackagingChargeRange.Where(h => h.IsActive == true).ToListAsync();
            //}
            //else {
            //    return await _sqlServerContext.PackagingChargeRange.ToListAsync();
            //}

            if (onlyActive == true)
            {
                IQueryable<PackagingChargeRange> data = from w in _sqlServerContext.PackagingChargeRange.AsNoTracking()
                                                        where w.IsActive == true
                                                        select w;
                return await data.ToListAsync();
            }
            else
            {
                IQueryable<PackagingChargeRange> data = from w in _sqlServerContext.PackagingChargeRange.AsNoTracking()
                                                        select w;
                return await data.ToListAsync();
            }
        }

        public async Task<CourierOrders> UpdateCourierOrdersApp(string id, CourierOrders courierOrders)
        {
            int startIndex = 3;
            int endIndex = id.Length - 3;
            int OrderId = Convert.ToInt32(id.Substring(startIndex, endIndex));


            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == OrderId);
            if (entity != null)
            {
                entity.Mobile = courierOrders.Mobile;
                entity.OtherMobile = courierOrders.OtherMobile;
                entity.Address = courierOrders.Address;
                await _sqlServerContext.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<CourierOrders> UpdateCourierOrdersAppV2(string id, CourierOrders courierOrders)
        {
            int startIndex = 3;
            int endIndex = id.Length - 3;
            int orderId = Convert.ToInt32(id.Substring(startIndex, endIndex));

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == orderId);

            if(entity != null)
            {
                entity.CustomerName = courierOrders.CustomerName;
                entity.Mobile = courierOrders.Mobile;
                entity.OtherMobile = courierOrders.OtherMobile;
                entity.Address = courierOrders.Address;
                entity.CollectionName = courierOrders.CollectionName;
                entity.CollectionAmount = courierOrders.CollectionAmount;
                entity.CodCharge = courierOrders.CodCharge;
                entity.OfficeDrop = courierOrders.OfficeDrop;
                entity.CollectionCharge = courierOrders.CollectionCharge;
                await _sqlServerContext.SaveChangesAsync();
                return entity;
            }
            return null;
        }

        public async Task<CourierOrders> UpdateCourierOrders(string CourierOrderId, CourierOrders courierOrders)
        {
            int startIndex = 3;
            int endIndex = CourierOrderId.Length - 3;
            int id = Convert.ToInt32(CourierOrderId.Substring(startIndex, endIndex));

            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);

            //var courierUsers = new CourierUsers();

            //if (!courierOrders.OfficeDrop)
            //{
            var courierUsers = _sqlServerContext.CourierUsers.FirstOrDefault(item => item.CourierUserId == entity.MerchantId);
            //}



            if (entity != null)
            {
                //PropertyInfo[] props = typeof(CourierOrders).GetProperties();
                //Type stringType = typeof(String);
                //Type intType = typeof(Int32);
                //Type decimalType = typeof(decimal); 
                //Type boolType = typeof(bool);

                //foreach (var prop in props)
                //{
                //    var name = prop.Name;
                //    var value = prop.GetValue(courierOrders);
                //    var typeName = prop.PropertyType.Name;
                //    var type = prop.PropertyType;

                //    if (type == stringType)
                //    {
                //        if (value.ToString() != "")
                //        {
                //            entity.CustomerName = value.ToString();
                //        }
                //    }
                //}
                entity.PackagingCharge = courierOrders.PackagingCharge;
                entity.PackagingName = courierOrders.PackagingName;
                entity.CustomerName = courierOrders.CustomerName;
                entity.Mobile = courierOrders.Mobile;
                entity.OtherMobile = courierOrders.OtherMobile;
                entity.Address = courierOrders.Address;
                entity.CollectAddress = courierOrders.CollectAddress;
                entity.CollectionName = courierOrders.CollectionName;
                entity.ProductType = courierOrders.ProductType;
                entity.Weight = courierOrders.Weight;
                entity.WeightRangeId = courierOrders.WeightRangeId;
                entity.PaymentType = courierOrders.PaymentType;
                entity.DeliveryRangeId = courierOrders.DeliveryRangeId;
                entity.DeliveryCharge = courierOrders.DeliveryCharge;
                entity.DistrictId = courierOrders.DistrictId;
                entity.ThanaId = courierOrders.ThanaId;
                entity.AreaId = courierOrders.AreaId;
                entity.CollectAddressDistrictId = courierOrders.CollectAddressDistrictId;
                entity.CollectAddressThanaId = courierOrders.CollectAddressThanaId;
                entity.OfficeDrop = courierOrders.OfficeDrop;
                entity.CollectionCharge = courierOrders.OfficeDrop ? 0 : courierUsers.CollectionCharge;
                entity.MerchantDeliveryDate = courierOrders.MerchantDeliveryDate;
                entity.MerchantCollectionDate = courierOrders.MerchantCollectionDate;
                entity.OfferCode = courierOrders.OfferCode;
                entity.OfferCodDiscount = courierOrders.OfferCodDiscount;
                entity.OfferBkashDiscount = courierOrders.OfferBkashDiscount;
                entity.IsOfferCodActive = courierOrders.IsOfferCodActive;
                entity.IsOfferBkashActive = courierOrders.IsOfferBkashActive;
                entity.ClassifiedId = courierOrders.ClassifiedId;
                entity.ServiceType = courierOrders.ServiceType;

                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;

        }

        public async Task<CourierOrders> UpdateDeliveryChargeFromOperation(string courierOrdersId, CourierOrders courierOrders)
        {
            int startIndex = 3;
            int endIndex = courierOrdersId.Length - 3;
            int orderId = Convert.ToInt32(courierOrdersId.Substring(startIndex, endIndex));
            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(orders => orders.Id == orderId);

            if(entity != null)
            {
                entity.DeliveryCharge = courierOrders.DeliveryCharge;
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<PackagingChargeRange> UpdatePackagingChargeRange(int id, PackagingChargeRange packagingChargeRange)
        {
            var entity = await _sqlServerContext.PackagingChargeRange.FirstOrDefaultAsync(item => item.PackagingChargeId == id);

            if (entity != null)
            {
                entity.PackagingName = packagingChargeRange.PackagingName;
                entity.PackagingCharge = packagingChargeRange.PackagingCharge;
                entity.IsActive = packagingChargeRange.IsActive;
                // Update entity in DbSet
                _sqlServerContext.PackagingChargeRange.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<CourierOrders> UpdateOrdersBondhuApp(int id, CourierOrders courierOrders)
        {
            var entity = await _sqlServerContext.CourierOrders.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.DeliveryCharge = courierOrders.DeliveryCharge;
                entity.WeightRangeId = courierOrders.WeightRangeId;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;
        }

        public async Task<List<SurveyQuestionAnswerLog>> AddSurveyQuestionAnswerLog(List<SurveyQuestionAnswerLog> surveyQuestionAnswerLog)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.SurveyQuestionAnswerLog.AddRangeAsync(surveyQuestionAnswerLog);
            await _sqlServerContext.SaveChangesAsync();
            return surveyQuestionAnswerLog;
        }

        public async Task<List<DeliveryChargeDetails>> AddAssignCouirerAndService(List<DeliveryChargeDetails> deliveryChargeDetails)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.DeliveryChargeDetails.AddRangeAsync(deliveryChargeDetails);
            await _sqlServerContext.SaveChangesAsync();
            return deliveryChargeDetails;
            //return null;
        }
        public async Task<List<DeliveryChargeMerchantDetails>> AddMerchantWiseAssignCouirerAndService(List<DeliveryChargeMerchantDetails> deliveryChargeMerchantDetails)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.DeliveryChargeMerchantDetails.AddRangeAsync(deliveryChargeMerchantDetails);
            await _sqlServerContext.SaveChangesAsync();
            return deliveryChargeMerchantDetails;
        }

        public async Task<List<Category>> AddDtCategories(List<Category> categories)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.Category.AddRangeAsync(categories);
            await _sqlServerContext.SaveChangesAsync();
            return categories;

        }
        public async Task<List<SubCategory>> AddSubCategories(List<SubCategory> subCategories)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.SubCategory.AddRangeAsync(subCategories);
            await _sqlServerContext.SaveChangesAsync();
            return subCategories;

        }

        public async Task<int> UpdateCourierPodnumbers(List<CourierOrders> orders)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;

            var orderIds = orders.Select(a => a.Id).ToArray();

            foreach (var item in orders)
            {
                var entity = _sqlServerContext.CourierOrders.AsNoTracking()
                .Where(x => x.Id.Equals(item.Id))
                .BatchUpdate(x => new CourierOrders
                {
                    PodNumber = item.PodNumber,
                    UpdatedOn = item.UpdatedOn,
                    UpdatedBy = 49,
                    IsConfirmedBy = "courier",
                    CourierId = 49,
                    Comment = "Order received by 2LP",
                    Status = 11
                });
            }

            var resEntity = _sqlServerContext.CourierOrders.AsNoTracking()
            .Where(x => orderIds.Contains(x.Id));

            var courierOrderStatusHistory = resEntity.Select(x => new CourierOrderStatusHistory()
            {
                CourierOrderId = x.CourierOrdersId,
                IsConfirmedBy = x.IsConfirmedBy,
                OrderDate = x.OrderDate,
                Status = x.Status,
                PostedBy = x.UpdatedBy,
                MerchantId = x.MerchantId,
                Comment = x.Comment,
                PodNumber = x.PodNumber,
                CourierId = x.CourierId,
                HubName = x.HubName
            }).ToList();
            await _sqlServerContext.BulkInsertAsync(courierOrderStatusHistory);


            return 1;

        }


        public async Task<List<BkashPayment>> BkashPaymentLog(List<BkashPayment> request)
        {

            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;

            var orderId = request.Select(x => x.OrderId).ToArray();
            var transactionId = request.FirstOrDefault().TransactionId;
            var courierId = request.FirstOrDefault().CourierId;
            var invoiceNumber = request.FirstOrDefault().InvoiceNumber;
            var paymentType = request.FirstOrDefault().PaymentType;
            var postedBy = request.FirstOrDefault().PostedBy;
            var adminType = request.FirstOrDefault().AdminType;
            var postedOn = request.FirstOrDefault().PostedOn;

            var bkashLog = _sqlServerContext.CourierOrders.Where(x => orderId.Contains(x.Id)).Select(y => new BkashPayment
            {
                OrderId = y.Id,
                PODNumber = y.PodNumber,
                TransactionId = transactionId,
                CourierId = courierId,
                PaymentType = paymentType,
                PostedBy = postedBy,
                Status = y.Status,
                InvoiceNumber = invoiceNumber,
                AdminType = adminType,
                CollectionAmount = y.CollectionAmount,
                PostedOn = postedOn
            }).ToList();

            await _sqlServerContext.BulkInsertAsync(bkashLog);

            var entity = _sqlServerContext.BkashPayment.AsNoTracking().Where(x=> orderId.Contains(x.OrderId) && x.TransactionId == transactionId);

            return entity.ToList();
        }

        public async Task<int> UpdateBkashPayment(List<CourierOrders> orders)
        {
            //var transaction = _sqlServerContext.Database.BeginTransaction();
            //try
            //{

                _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;

                var orderIds = orders.Select(a => a.Id).ToArray();
                var transactionId = orders.Select(a => a.TransactionId).FirstOrDefault();
                var validationId = orders.Select(a => a.ValidationId).FirstOrDefault();
                var courierId = orders.Select(a => a.CourierId).FirstOrDefault();
                var status = orders.Select(a => a.Status).FirstOrDefault();
                var comment = orders.Select(a => a.Comment).FirstOrDefault();
                var isConfirmedBy = orders.Select(a => a.IsConfirmedBy).FirstOrDefault();
                var updatedBy = orders.Select(a => a.UpdatedBy).FirstOrDefault();

                var entity = await _sqlServerContext.CourierOrders.AsNoTracking()
                .Where(x => orderIds.Contains(x.Id))
                .BatchUpdateAsync(x => new CourierOrders
                {
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = updatedBy,
                    IsConfirmedBy = isConfirmedBy,
                    CourierId = courierId,
                    Comment = comment,
                    Status = status,
                    TransactionId = transactionId,
                    ValidationId = validationId
                });
                //var responseData = new List<CourierOrderStatusHistory>();

                var resEntity = _sqlServerContext.CourierOrders.AsNoTracking()
                .Where(x => orderIds.Contains(x.Id));

                var courierOrderStatusHistory = resEntity.Select(x => new CourierOrderStatusHistory()
                {
                    CourierOrderId = x.CourierOrdersId,
                    IsConfirmedBy = x.IsConfirmedBy,
                    OrderDate = x.OrderDate,
                    Status = x.Status,
                    PostedBy = x.UpdatedBy,
                    MerchantId = x.MerchantId,
                    Comment = x.Comment,
                    PodNumber = x.PodNumber,
                    CourierId = x.CourierId,
                    HubName =x.HubName
                }).ToList();

                //responseData = await _orderHistoryRepository.AddListCourierOrderHistory(responseData);
                await _sqlServerContext.BulkInsertAsync(courierOrderStatusHistory);


                //_sqlServerContext.SaveChanges();
                //transaction.Commit();

                return entity;
            //}
            //catch
            //{
            //    transaction.Rollback();
            //}

            //return 0;

        }
        public async Task<Category> UpdateDtCategory(int categoryId, Category category)
        {
            var entity = await _sqlServerContext.Category.FirstOrDefaultAsync(item => item.CategoryId == categoryId);
            if(entity != null)
            {
                entity.CategoryNameEng = category.CategoryNameEng;
                entity.CategoryNameBng = category.CategoryNameBng;
                entity.IsActive = category.IsActive;

                if(_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;

        }
        public async Task<SubCategory> UpdateSubCategory(int subCategoryId, SubCategory subCategory)
        {
            var entity = await _sqlServerContext.SubCategory.FirstOrDefaultAsync(item => item.SubCategoryId == subCategoryId);
            if (entity != null)
            {
                entity.SubCategoryNameEng = subCategory.SubCategoryNameEng;
                entity.SubCategoryNameBng = subCategory.SubCategoryNameBng;
                entity.IsActive = subCategory.IsActive;
                entity.CategoryId = subCategory.CategoryId;

                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    _sqlServerContext.Update(entity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                return entity;
            }
            return null;

        }

        public async Task<int> UpdateServicePriceBulk(List<DeliveryChargeDetails> deliveryChargeDetails)
        {
            await _sqlServerContext.BulkUpdateAsync(deliveryChargeDetails);

            return 1;
        }
        public async Task<int> UpdateWeightRangePriceBulk(List<DeliveryChargeDetails> deliveryChargeDetails)
        {
            await _sqlServerContext.BulkUpdateAsync(deliveryChargeDetails);

            return 1;
        }

        public async Task<List<CustomerSMS>> AddCustomerSMSLog(List<CustomerSMS> customerSMs)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.CustomerSMS.AddRangeAsync(customerSMs);
            await _sqlServerContext.SaveChangesAsync();
            return customerSMs;

        }

        public async Task<List<CouriersWithLoanSurvey>> AddCouriersWithLoanSurvey(List<CouriersWithLoanSurvey> couriersWithLoanSurvey)
        {
            _sqlServerContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await _sqlServerContext.CouriersWithLoanSurvey.AddRangeAsync(couriersWithLoanSurvey);
            await _sqlServerContext.SaveChangesAsync();
            return couriersWithLoanSurvey;

        }

    }
}
