using AdCourier.Context;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.DeliveryBondu;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class BondhuService: IBondhuService
    {
        private readonly IBondhuRepository _bondhuRepository;
        private readonly IRedisCacheClient _redis;
        private readonly IOrderService _orderService;
        private readonly ISmsEmailService _smsEmailService;
        private readonly IWeightRangeRepository _weightRangeRepository;
        private readonly SqlServerContext _sqlServerContext;
        private readonly OtherService _otherService;

        public BondhuService(IBondhuRepository bondhuRepository, 
            IRedisCacheClient redis,
            IOrderService orderService,
            ISmsEmailService smsEmailService,
            IWeightRangeRepository weightRangeRepository,
            SqlServerContext sqlServerContext,
            OtherService otherService
            )
        {
            _bondhuRepository = bondhuRepository;
            _redis = redis;
            _orderService = orderService;
            _smsEmailService = smsEmailService;
            _weightRangeRepository = weightRangeRepository;
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _otherService = otherService;
        }

        public async Task<SelfDeliveryAllDataResponseModel> LoadOrderForBondhuApp(SelfDeliveryOrderRequestNewModel model)
        {
            var responseTotalData = new SelfDeliveryAllDataResponseModel();
            var data = new List<CustomerOrderResponseModel>();

            var response = new List<SelfDeliveryOrderResponseModel>();

            if (model.Type.ToLower().Trim() == "return")
            {
                response = await _bondhuRepository.LoadOrderReturnForBondhuApp(model);
            }
            else
            {
                response = await _bondhuRepository.LoadOrderForBondhuApp(model);
            }

            if (model.Flag == 0)
            {
                data = response.GroupBy(x => x.CustomerId)
                    .Select(d => new CustomerOrderResponseModel
                    {
                        Id = d.FirstOrDefault().CustomerId,
                        Name = (d.FirstOrDefault().StatusId == 357 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 335 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 44 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 52 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 51 && model.Type == "collection") ? "Hub Delivery" : d.FirstOrDefault().CustomerName,
                        MerchantId = 0,
                        CollectAddressDistrictId = d.FirstOrDefault().CollectAddressDistrictId,
                        CollectAddressThanaId = d.FirstOrDefault().CollectAddressThanaId,
                        Latitude = d.FirstOrDefault().Latitude,
                        Longitude = d.FirstOrDefault().Longitude,
                        MobileNumber = d.FirstOrDefault().StatusId == 40 ? "" : d.FirstOrDefault().CustomerMobileNumber,
                        Address = d.FirstOrDefault().StatusId == 40 ? "" : model.Type == "collection" ? d.FirstOrDefault().OrderType.ToLower().StartsWith("same") ? "" : d.FirstOrDefault().HubAddress : d.FirstOrDefault().CustomerAddress,
                        TotalOrder = d.Count(),
                        District = model.Type == "collection" ? d.FirstOrDefault().OrderType.ToLower().StartsWith("same") ? "Main Hub" : d.FirstOrDefault().HubName : d.FirstOrDefault().District,
                        TotalPayment = response.Where(x => x.CustomerId == d.FirstOrDefault().CustomerId).Sum(x => x.CouponPrice)
                        + response.Where(x => x.CustomerId == d.FirstOrDefault().CustomerId).Sum(x => x.DeliveryCharge),
                        SourceInfo = new CollectionSource
                        {
                            SourcePersonName = "",
                            SourceAddress = "",
                            SourceMobile = "",
                            SourceDealPrice = 0,
                            SourceMessageData = new SelfDeliveryService().CustomerSourceMessage(d.FirstOrDefault().StatusId,
                            d.FirstOrDefault().PaymentType, d.FirstOrDefault().SourceAddress,
                            d.FirstOrDefault().CouponPrice, d.FirstOrDefault().SourceDealPrice,
                            d.FirstOrDefault().DeliveryManCommission,
                            d.Sum(x => x.DeliveryCharge) + d.Sum(x => x.CouponPrice),
                            d.FirstOrDefault().DeliveryCharge, d.FirstOrDefault().CouponId)
                        },
                        Actions = new SelfDeliveryService().CustomerActionData(d.FirstOrDefault().StatusId, d.FirstOrDefault().CouponId, model.Type),
                        CustomerOrderDataModel = d.Where(y => y.CustomerId == d.FirstOrDefault().CustomerId).Select(x => new CustomerOrderData
                        {
                            PriorityService = x.PriorityService,
                            WeightRangeId = x.WeightRangeId,
                            DeliveryRangeId = x.DeliveryRangeId,
                            CouponId = x.CouponId,
                            CustomerId = x.CustomerId,
                            ProductTitle = x.ProductTitle + " " + (x.Sizes) + " Qtn: " + x.ProductQtn + " Type: " + x.DeliveryRangeName,
                            ProductPrice = x.CouponPrice,
                            DeliveryCharge = x.DeliveryCharge,
                            BondhuCharge = x.BondhuCharge,
                            DeliveryType = x.DeliveryRangeName,
                            IsAdvancePayment = x.IsAdvancePayment,
                            Sizes = x.Sizes,
                            Colors = x.Colors,
                            ProductQtn = x.ProductQtn,
                            ImageUrl = BaseUrlModel.ImageBaseUrl + "images/deals/" + x.FolderName + "/smallimage1.jpg",
                            StatusId = x.StatusId,
                            Comments = x.Comments,
                            OrderDate = x.OrderDate,
                            MerchantId = x.MerchantId,
                            DealId = x.DealId,
                            DeliveryDate = x.DeliveryDate,
                            CommentedBy = x.CommentedBy,
                            PODNumber = x.PODNumber,
                            HubName = x.HubName,
                            CollectionPointId = x.CollectionPointId,
                            TotalPayment = (x.CouponPrice + x.DeliveryCharge),
                            CollectionTimeSlot = new CollectionTimeSlot
                            {
                                StartTime = x.StartTime,
                                EndTime = x.EndTime
                            },
                            SourceInfo = new CollectionSource
                            {
                                SourcePersonName = x.SourcePersonName,
                                SourceAddress = x.SourceAddress,
                                SourceMobile = x.SourceMobile,
                                SourceDealPrice = x.SourceDealPrice,
                                SourceMessageData = new SelfDeliveryService().SourceMessage(x.StatusId, x.PaymentType, x.SourceAddress, x.CouponPrice, x.SourceDealPrice, x.DeliveryManCommission, x.ProductQtn, x.CouponId, x.DeliveryCharge)
                            },
                            Actions = new SelfDeliveryService().SetActionDataModel(x.StatusId, x.CouponId, model.Flag, x.PaymentType, model.Type, x.CollectionAmount)
                        }).ToList()


                    }).OrderBy(i => i.Id).Skip(model.Index).Take(model.Count).ToList();
            }
            else if (model.Flag == 1)
            {
                data = response.GroupBy(x => new { x.CollectionPointId, x.CollectAddressThanaId })
                    .Select(d => new CustomerOrderResponseModel
                    {
                        Id = d.FirstOrDefault().CollectionPointId.ToString(),
                        Name = d.FirstOrDefault().SourcePersonName,
                        MerchantId = d.FirstOrDefault().CollectionPointId,
                        MobileNumber = d.FirstOrDefault().SourceMobile,
                        AlterMobile = d.FirstOrDefault().AlterMobile,
                        CollectAddressDistrictId = d.FirstOrDefault().CollectAddressDistrictId,
                        CollectAddressThanaId = d.FirstOrDefault().CollectAddressThanaId,
                        Latitude = d.FirstOrDefault().Latitude,
                        Longitude = d.FirstOrDefault().Longitude,
                        Address = d.FirstOrDefault().SourceAddress + " " + d.FirstOrDefault().AlterMobile,
                        TotalOrder = d.Count(),
                        District = d.FirstOrDefault().CollectDistrict,
                        SourceInfo = new CollectionSource
                        {
                            SourcePersonName = "",//d.SourcePersonName,
                            SourceAddress = "",//d.SourceAddress,
                            SourceMobile = "",//d.SourceMobile,
                            SourceDealPrice = 0, //d.SourceDealPrice,
                            SourceMessageData = new SelfDeliveryService().CustomerSourceMessage(d.FirstOrDefault().StatusId, d.FirstOrDefault().PaymentType,
                            d.FirstOrDefault().SourceAddress, d.FirstOrDefault().CouponPrice,
                            d.FirstOrDefault().SourceDealPrice, d.FirstOrDefault().DeliveryManCommission,
                            d.Sum(x => x.DeliveryCharge) + d.Sum(x => x.CouponPrice),
                            d.FirstOrDefault().DeliveryCharge, d.FirstOrDefault().CouponId)
                        },
                        Actions = new SelfDeliveryService().CustomerActionData(d.FirstOrDefault().StatusId, d.FirstOrDefault().CouponId, model.Type),
                        CustomerOrderDataModel = d.Where(y => y.CollectionPointId == d.FirstOrDefault().CollectionPointId).Select(x => new CustomerOrderData
                        {
                            PriorityService = x.PriorityService,
                            WeightRangeId = x.WeightRangeId,
                            DeliveryRangeId = x.DeliveryRangeId,
                            CouponId = x.CouponId,
                            CustomerId = x.CustomerId,
                            ProductTitle = x.ProductTitle + " " + (x.Sizes) + " Qtn: " + x.ProductQtn + " Type: " + x.DeliveryRangeName,
                            ProductPrice = x.CouponPrice,
                            DeliveryCharge = x.DeliveryCharge,
                            BondhuCharge = x.BondhuCharge,
                            DeliveryType = x.DeliveryRangeName,
                            IsAdvancePayment = x.IsAdvancePayment,
                            Sizes = x.Sizes,
                            Colors = x.Colors,
                            ProductQtn = x.ProductQtn,
                            ImageUrl = BaseUrlModel.ImageBaseUrl + "images/deals/" + x.FolderName + "/smallimage1.jpg",
                            StatusId = x.StatusId,
                            Comments = x.Comments,
                            OrderDate = x.OrderDate,
                            MerchantId = x.MerchantId,
                            DealId = x.DealId,
                            DeliveryDate = x.DeliveryDate,
                            CommentedBy = x.CommentedBy,
                            PODNumber = x.PODNumber,
                            HubName = x.HubName,
                            CollectionPointId = x.CollectionPointId,
                            TotalPayment = (x.CouponPrice + x.DeliveryCharge),
                            CollectionTimeSlot = new CollectionTimeSlot
                            {
                                StartTime = x.StartTime,
                                EndTime = x.EndTime
                            },
                            SourceInfo = new CollectionSource
                            {
                                SourcePersonName = x.SourcePersonName,
                                SourceAddress = x.SourceAddress,
                                SourceMobile = x.SourceMobile,
                                SourceDealPrice = x.SourceDealPrice,
                                SourceMessageData = new SelfDeliveryService().SourceMessage(x.StatusId, x.PaymentType, x.SourceAddress, x.CouponPrice, x.SourceDealPrice, x.DeliveryManCommission, x.ProductQtn, x.CouponId, x.DeliveryCharge)
                            },
                            Actions = new SelfDeliveryService().SetActionDataModel(x.StatusId, x.CouponId, model.Flag, x.PaymentType, model.Type, x.CollectionAmount),
                        }).ToList()
                    }).OrderBy(i => i.Id).Skip(model.Index).Take(model.Count).ToList();
            }

            responseTotalData = new SelfDeliveryAllDataResponseModel
            {
                TotalCount = model.Flag == 0 ? response.Select(x => x.CustomerId).Distinct().Count() : response.Select(x => x.CollectionPointId).Distinct().Count(),
                customerOrderResponseModel = data.ToList()
            };

            return responseTotalData;

        }

        public async Task<SelfDeliveryAllDataResponseModel> LoadOrderForBondhuAppByTimeSlot(SelfDeliveryOrderRequestNewModel model)
        {
            var responseTotalData = new SelfDeliveryAllDataResponseModel();
            var data = new List<CustomerOrderResponseModel>();

            var response = new List<SelfDeliveryOrderResponseModel>();

            if (model.Type.ToLower().Trim() == "return")
            {
                response = await _bondhuRepository.LoadOrderReturnForBondhuApp(model);
            }
            else
            {
                response = await _bondhuRepository.LoadOrderForBondhuAppByTimeSlot(model);
                //response = await _bondhuRepository.LoadOrderForBondhuApp(model);
            }

            if (model.Flag == 0)
            {
                data = response.GroupBy(x => x.CustomerId)
                    .Select(d => new CustomerOrderResponseModel
                    {
                        Id = d.FirstOrDefault().CustomerId,
                        Name = (d.FirstOrDefault().StatusId == 357 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 335 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 44 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 52 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 51 && model.Type == "collection") ? "Hub Delivery" : d.FirstOrDefault().CustomerName,
                        MerchantId = 0,
                        CollectAddressDistrictId = d.FirstOrDefault().CollectAddressDistrictId,
                        CollectAddressThanaId = d.FirstOrDefault().CollectAddressThanaId,
                        Latitude = d.FirstOrDefault().Latitude,
                        Longitude = d.FirstOrDefault().Longitude,
                        MobileNumber = d.FirstOrDefault().StatusId == 40 ? "" : d.FirstOrDefault().CustomerMobileNumber,
                        Address = d.FirstOrDefault().StatusId == 40 ? "" : model.Type == "collection" ? d.FirstOrDefault().OrderType.ToLower().StartsWith("same") ? "" : d.FirstOrDefault().HubAddress : d.FirstOrDefault().CustomerAddress,
                        TotalOrder = d.Count(),
                        District = model.Type == "collection" ? d.FirstOrDefault().OrderType.ToLower().StartsWith("same") ? "Main Hub" : d.FirstOrDefault().HubName : d.FirstOrDefault().District,
                        TotalPayment = response.Where(x => x.CustomerId == d.FirstOrDefault().CustomerId).Sum(x => x.CouponPrice)
                        + response.Where(x => x.CustomerId == d.FirstOrDefault().CustomerId).Sum(x => x.DeliveryCharge),
                        SourceInfo = new CollectionSource
                        {
                            SourcePersonName = "",
                            SourceAddress = "",
                            SourceMobile = "",
                            SourceDealPrice = 0,
                            SourceMessageData = new SelfDeliveryService().CustomerSourceMessage(d.FirstOrDefault().StatusId,
                            d.FirstOrDefault().PaymentType, d.FirstOrDefault().SourceAddress,
                            d.FirstOrDefault().CouponPrice, d.FirstOrDefault().SourceDealPrice,
                            d.FirstOrDefault().DeliveryManCommission,
                            d.Sum(x => x.DeliveryCharge) + d.Sum(x => x.CouponPrice),
                            d.FirstOrDefault().DeliveryCharge, d.FirstOrDefault().CouponId)
                        },
                        Actions = new SelfDeliveryService().CustomerActionData(d.FirstOrDefault().StatusId, d.FirstOrDefault().CouponId, model.Type),
                        CustomerOrderDataModel = d.Where(y => y.CustomerId == d.FirstOrDefault().CustomerId).Select(x => new CustomerOrderData
                        {
                            PriorityService = x.PriorityService,
                            WeightRangeId = x.WeightRangeId,
                            DeliveryRangeId = x.DeliveryRangeId,
                            CouponId = x.CouponId,
                            CustomerId = x.CustomerId,
                            ProductTitle = x.ProductTitle + " " + (x.Sizes) + " Qtn: " + x.ProductQtn + " Type: " + x.DeliveryRangeName,
                            ProductPrice = x.CouponPrice,
                            DeliveryCharge = x.DeliveryCharge,
                            BondhuCharge = x.BondhuCharge,
                            DeliveryType = x.DeliveryRangeName,
                            IsAdvancePayment = x.IsAdvancePayment,
                            Sizes = x.Sizes,
                            Colors = x.Colors,
                            ProductQtn = x.ProductQtn,
                            ImageUrl = BaseUrlModel.ImageBaseUrl + "images/deals/" + x.FolderName + "/smallimage1.jpg",
                            StatusId = x.StatusId,
                            Comments = x.Comments,
                            OrderDate = x.OrderDate,
                            MerchantId = x.MerchantId,
                            DealId = x.DealId,
                            DeliveryDate = x.DeliveryDate,
                            CommentedBy = x.CommentedBy,
                            PODNumber = x.PODNumber,
                            HubName = x.HubName,
                            CollectionPointId = x.CollectionPointId,
                            TotalPayment = (x.CouponPrice + x.DeliveryCharge),
                            CollectionTimeSlot = new CollectionTimeSlot
                            {
                                StartTime = x.StartTime,
                                EndTime = x.EndTime
                            },
                            SourceInfo = new CollectionSource
                            {
                                SourcePersonName = x.SourcePersonName,
                                SourceAddress = x.SourceAddress,
                                SourceMobile = x.SourceMobile,
                                SourceDealPrice = x.SourceDealPrice,
                                SourceMessageData = new SelfDeliveryService().SourceMessage(x.StatusId, x.PaymentType, x.SourceAddress, x.CouponPrice, x.SourceDealPrice, x.DeliveryManCommission, x.ProductQtn, x.CouponId, x.DeliveryCharge)
                            },
                            Actions = new SelfDeliveryService().SetActionDataModel(x.StatusId, x.CouponId, model.Flag, x.PaymentType, model.Type, x.CollectionAmount)
                        }).ToList()


                    }).OrderBy(i => i.Id).Skip(model.Index).Take(model.Count).ToList();
            }
            else if (model.Flag == 1)
            {
                data = response.GroupBy(x => new { x.CollectionPointId, x.CollectAddressThanaId })
                    .Select(d => new CustomerOrderResponseModel
                    {
                        Id = d.FirstOrDefault().CollectionPointId.ToString(),
                        Name = d.FirstOrDefault().SourcePersonName,
                        MerchantId = d.FirstOrDefault().CollectionPointId,
                        MobileNumber = d.FirstOrDefault().SourceMobile,
                        AlterMobile = d.FirstOrDefault().AlterMobile,
                        CollectAddressDistrictId = d.FirstOrDefault().CollectAddressDistrictId,
                        CollectAddressThanaId = d.FirstOrDefault().CollectAddressThanaId,
                        Latitude = d.FirstOrDefault().Latitude,
                        Longitude = d.FirstOrDefault().Longitude,
                        Address = d.FirstOrDefault().SourceAddress + " " + d.FirstOrDefault().AlterMobile,
                        TotalOrder = d.Count(),
                        District = d.FirstOrDefault().CollectDistrict,
                        SourceInfo = new CollectionSource
                        {
                            SourcePersonName = "",//d.SourcePersonName,
                            SourceAddress = "",//d.SourceAddress,
                            SourceMobile = "",//d.SourceMobile,
                            SourceDealPrice = 0, //d.SourceDealPrice,
                            SourceMessageData = new SelfDeliveryService().CustomerSourceMessage(d.FirstOrDefault().StatusId, d.FirstOrDefault().PaymentType,
                            d.FirstOrDefault().SourceAddress, d.FirstOrDefault().CouponPrice,
                            d.FirstOrDefault().SourceDealPrice, d.FirstOrDefault().DeliveryManCommission,
                            d.Sum(x => x.DeliveryCharge) + d.Sum(x => x.CouponPrice),
                            d.FirstOrDefault().DeliveryCharge, d.FirstOrDefault().CouponId)
                        },
                        Actions = new SelfDeliveryService().CustomerActionData(d.FirstOrDefault().StatusId, d.FirstOrDefault().CouponId, model.Type),
                        CustomerOrderDataModel = d.Where(y => y.CollectionPointId == d.FirstOrDefault().CollectionPointId).Select(x => new CustomerOrderData
                        {
                            PriorityService = x.PriorityService,
                            WeightRangeId = x.WeightRangeId,
                            DeliveryRangeId = x.DeliveryRangeId,
                            CouponId = x.CouponId,
                            CustomerId = x.CustomerId,
                            ProductTitle = x.ProductTitle + " " + (x.Sizes) + " Qtn: " + x.ProductQtn + " Type: " + x.DeliveryRangeName,
                            ProductPrice = x.CouponPrice,
                            DeliveryCharge = x.DeliveryCharge,
                            BondhuCharge = x.BondhuCharge,
                            DeliveryType = x.DeliveryRangeName,
                            IsAdvancePayment = x.IsAdvancePayment,
                            Sizes = x.Sizes,
                            Colors = x.Colors,
                            ProductQtn = x.ProductQtn,
                            ImageUrl = BaseUrlModel.ImageBaseUrl + "images/deals/" + x.FolderName + "/smallimage1.jpg",
                            StatusId = x.StatusId,
                            Comments = x.Comments,
                            OrderDate = x.OrderDate,
                            MerchantId = x.MerchantId,
                            DealId = x.DealId,
                            DeliveryDate = x.DeliveryDate,
                            CommentedBy = x.CommentedBy,
                            PODNumber = x.PODNumber,
                            HubName = x.HubName,
                            CollectionPointId = x.CollectionPointId,
                            TotalPayment = (x.CouponPrice + x.DeliveryCharge),
                            CollectionTimeSlot = new CollectionTimeSlot
                            {
                                StartTime = x.StartTime,
                                EndTime = x.EndTime
                            },
                            SourceInfo = new CollectionSource
                            {
                                SourcePersonName = x.SourcePersonName,
                                SourceAddress = x.SourceAddress,
                                SourceMobile = x.SourceMobile,
                                SourceDealPrice = x.SourceDealPrice,
                                SourceMessageData = new SelfDeliveryService().SourceMessage(x.StatusId, x.PaymentType, x.SourceAddress, x.CouponPrice, x.SourceDealPrice, x.DeliveryManCommission, x.ProductQtn, x.CouponId, x.DeliveryCharge)
                            },
                            Actions = new SelfDeliveryService().SetActionDataModel(x.StatusId, x.CouponId, model.Flag, x.PaymentType, model.Type, x.CollectionAmount),
                        }).ToList()
                    }).OrderBy(i => i.Id).Skip(model.Index).Take(model.Count).ToList();
            }

            responseTotalData = new SelfDeliveryAllDataResponseModel
            {
                TotalCount = model.Flag == 0 ? response.Select(x => x.CustomerId).Distinct().Count() : response.Select(x => x.CollectionPointId).Distinct().Count(),
                customerOrderResponseModel = data.ToList()
            };

            return responseTotalData;

        }

        public async Task<SelfDeliveryAllDataResponseModel> LoadOrderForBondhuAppByTimeSlotNew(SelfDeliveryOrderRequestNewModel model)
        {
            var responseTotalData = new SelfDeliveryAllDataResponseModel();
            var data = new List<CustomerOrderResponseModel>();

            var response = new List<SelfDeliveryOrderResponseModel>();

            if (model.Type.ToLower().Trim() == "return")
            {
                response = await _bondhuRepository.LoadOrderReturnForBondhuApp(model);
            }
            else
            {
                if(model.CollectionSlotId == 0 || model.CollectionSlotId == -1 || model.CollectionSlotId == 4)
                {
                    response = await _bondhuRepository.LoadOrderForBondhuApp(model);
                }
                else
                {
                    response = await _bondhuRepository.LoadOrderForBondhuAppByTimeSlotNew(model);
                }
                
            }

            if (model.Flag == 0)
            {
                data = response.GroupBy(x => x.CustomerId)
                    .Select(d => new CustomerOrderResponseModel
                    {
                        Id = d.FirstOrDefault().CustomerId,
                        Name = (d.FirstOrDefault().StatusId == 357 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 335 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 44 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 52 && model.Type == "collection") || (d.FirstOrDefault().StatusId == 51 && model.Type == "collection") ? "Hub Delivery" : d.FirstOrDefault().CustomerName,
                        MerchantId = d.FirstOrDefault().CollectionPointId,
                        CollectAddressDistrictId = d.FirstOrDefault().CollectAddressDistrictId,
                        CollectAddressThanaId = d.FirstOrDefault().CollectAddressThanaId,
                        Latitude = d.FirstOrDefault().Latitude,
                        Longitude = d.FirstOrDefault().Longitude,
                        ServiceType = d.FirstOrDefault().ServiceType,
                        MobileNumber = d.FirstOrDefault().StatusId == 40 ? "" : d.FirstOrDefault().CustomerMobileNumber,
                        Address = d.FirstOrDefault().StatusId == 40 ? "" : model.Type == "collection" ? d.FirstOrDefault().OrderType.ToLower().StartsWith("same") ? "" : d.FirstOrDefault().HubAddress : d.FirstOrDefault().CustomerAddress,
                        TotalOrder = d.Count(),
                        District = model.Type == "collection" ? d.FirstOrDefault().OrderType.ToLower().StartsWith("same") ? "Main Hub" : d.FirstOrDefault().HubName : d.FirstOrDefault().District,
                        TotalPayment = response.Where(x => x.CustomerId == d.FirstOrDefault().CustomerId).Sum(x => x.CouponPrice)
                        + response.Where(x => x.CustomerId == d.FirstOrDefault().CustomerId).Sum(x => x.DeliveryCharge),
                        SourceInfo = new CollectionSource
                        {
                            SourcePersonName = "",
                            SourceAddress = "",
                            SourceMobile = "",
                            SourceDealPrice = 0,
                            SourceMessageData = new SelfDeliveryService().CustomerSourceMessage(d.FirstOrDefault().StatusId,
                            d.FirstOrDefault().PaymentType, d.FirstOrDefault().SourceAddress,
                            d.FirstOrDefault().CouponPrice, d.FirstOrDefault().SourceDealPrice,
                            d.FirstOrDefault().DeliveryManCommission,
                            d.Sum(x => x.DeliveryCharge) + d.Sum(x => x.CouponPrice),
                            d.FirstOrDefault().DeliveryCharge, d.FirstOrDefault().CouponId)
                        },
                        Actions = new SelfDeliveryService().CustomerActionData(d.FirstOrDefault().StatusId, d.FirstOrDefault().CouponId, model.Type),
                        CustomerOrderDataModel = d.Where(y => y.CustomerId == d.FirstOrDefault().CustomerId).Select(x => new CustomerOrderData
                        {
                            PriorityService = x.PriorityService,
                            WeightRangeId = x.WeightRangeId,
                            DeliveryRangeId = x.DeliveryRangeId,
                            CouponId = x.CouponId,
                            CustomerId = x.CustomerId,
                            //ProductTitle = x.ProductTitle + " " + (x.Sizes) + " Qtn: " + x.ProductQtn + " Type: " + x.DeliveryRangeName,
                            ProductTitle = x.ProductTitle + " " + (x.Sizes) + " ওজন: " + x.Weight,
                            ProductPrice = x.CouponPrice,
                            DeliveryCharge = x.DeliveryCharge,
                            BondhuCharge = x.BondhuCharge,
                            DeliveryType = x.DeliveryRangeName,
                            IsAdvancePayment = x.IsAdvancePayment,
                            Sizes = x.Sizes,
                            Colors = x.Colors,
                            ProductQtn = x.ProductQtn,
                            ImageUrl = BaseUrlModel.ImageBaseUrl + "images/deals/" + x.FolderName + "/smallimage1.jpg",
                            StatusId = x.StatusId,
                            Comments = x.Comments,
                            OrderDate = x.OrderDate,
                            MerchantId = x.MerchantId,
                            DealId = x.DealId,
                            DeliveryDate = x.DeliveryDate,
                            CommentedBy = x.CommentedBy,
                            PODNumber = x.PODNumber,
                            HubName = x.HubName,
                            CollectionPointId = x.CollectionPointId,
                            TotalPayment = (x.CouponPrice + x.DeliveryCharge),
                            IsHeavyWeight = x.IsHeavyWeight,
                            DocumentUrl = x.DocumentUrl,
                            IsPohOrder = x.PaymentServiceType,
                            CollectionTimeSlot = new CollectionTimeSlot
                            {
                                StartTime = x.StartTime,
                                EndTime = x.EndTime,
                                CollectionTimeSlotId = x.CollectionTimeSlotId
                            },
                            SourceInfo = new CollectionSource
                            {
                                SourcePersonName = x.SourcePersonName,
                                SourceAddress = x.SourceAddress,
                                SourceMobile = x.SourceMobile,
                                SourceDealPrice = x.SourceDealPrice,
                                SourceMessageData = new SelfDeliveryService().SourceMessage(x.StatusId, x.PaymentType, x.SourceAddress, x.CouponPrice, x.SourceDealPrice, x.DeliveryManCommission, x.ProductQtn, x.CouponId, x.DeliveryCharge)
                            },
                            Actions = new SelfDeliveryService().SetActionDataModel(x.StatusId, x.CouponId, model.Flag, x.PaymentType, model.Type, x.CollectionAmount)
                        }).ToList()


                    }).OrderBy(i => i.Id).Skip(model.Index).Take(model.Count).ToList();
            }
            else if (model.Flag == 1)
            {
                data = response.GroupBy(x => new { x.CollectionPointId, x.CollectAddressThanaId })
                    .Select(d => new CustomerOrderResponseModel
                    {
                        Id = d.FirstOrDefault().CollectionPointId.ToString(),
                        Name = d.FirstOrDefault().SourcePersonName,
                        MerchantId = d.FirstOrDefault().CollectionPointId,
                        MobileNumber = d.FirstOrDefault().SourceMobile,
                        AlterMobile = d.FirstOrDefault().AlterMobile,
                        CollectAddressDistrictId = d.FirstOrDefault().CollectAddressDistrictId,
                        CollectAddressThanaId = d.FirstOrDefault().CollectAddressThanaId,
                        Latitude = d.FirstOrDefault().Latitude,
                        Longitude = d.FirstOrDefault().Longitude,
                        ServiceType = d.FirstOrDefault().ServiceType,
                        Address = d.FirstOrDefault().SourceAddress + " " + d.FirstOrDefault().AlterMobile,
                        TotalOrder = d.Count(),
                        District = d.FirstOrDefault().CollectDistrict,
                        SourceInfo = new CollectionSource
                        {
                            SourcePersonName = "",//d.SourcePersonName,
                            SourceAddress = "",//d.SourceAddress,
                            SourceMobile = "",//d.SourceMobile,
                            SourceDealPrice = 0, //d.SourceDealPrice,
                            SourceMessageData = new SelfDeliveryService().CustomerSourceMessage(d.FirstOrDefault().StatusId, d.FirstOrDefault().PaymentType,
                            d.FirstOrDefault().SourceAddress, d.FirstOrDefault().CouponPrice,
                            d.FirstOrDefault().SourceDealPrice, d.FirstOrDefault().DeliveryManCommission,
                            d.Sum(x => x.DeliveryCharge) + d.Sum(x => x.CouponPrice),
                            d.FirstOrDefault().DeliveryCharge, d.FirstOrDefault().CouponId)
                        },
                        Actions = new SelfDeliveryService().CustomerActionData(d.FirstOrDefault().StatusId, d.FirstOrDefault().CouponId, model.Type),
                        CustomerOrderDataModel = d.Where(y => y.CollectionPointId == d.FirstOrDefault().CollectionPointId).Select(x => new CustomerOrderData
                        {
                            PriorityService = x.PriorityService,
                            WeightRangeId = x.WeightRangeId,
                            DeliveryRangeId = x.DeliveryRangeId,
                            CouponId = x.CouponId,
                            CustomerId = x.CustomerId,
                            //ProductTitle = x.ProductTitle + " " + (x.Sizes) + " Weight: " + x.ProductQtn + " Type: " + x.DeliveryRangeName,
                            ProductTitle = x.ProductTitle + " " + (x.Sizes) + " ওজন: " + x.Weight,
                            ProductPrice = x.CouponPrice,
                            DeliveryCharge = x.DeliveryCharge,
                            BondhuCharge = x.BondhuCharge,
                            DeliveryType = x.DeliveryRangeName,
                            IsAdvancePayment = x.IsAdvancePayment,
                            Sizes = x.Sizes,
                            Colors = x.Colors,
                            ProductQtn = x.ProductQtn,
                            ImageUrl = BaseUrlModel.ImageBaseUrl + "images/deals/" + x.FolderName + "/smallimage1.jpg",
                            StatusId = x.StatusId,
                            Comments = x.Comments,
                            OrderDate = x.OrderDate,
                            MerchantId = x.MerchantId,
                            DealId = x.DealId,
                            DeliveryDate = x.DeliveryDate,
                            CommentedBy = x.CommentedBy,
                            PODNumber = x.PODNumber,
                            HubName = x.HubName,
                            CollectionPointId = x.CollectionPointId,
                            TotalPayment = (x.CouponPrice + x.DeliveryCharge),
                            IsHeavyWeight = x.IsHeavyWeight,
                            DocumentUrl = x.DocumentUrl,
                            IsPohOrder = x.PaymentServiceType,
                            CollectionTimeSlot = new CollectionTimeSlot
                            {
                                StartTime = x.StartTime,
                                EndTime = x.EndTime,
                                CollectionTimeSlotId = x.CollectionTimeSlotId
                            },
                            SourceInfo = new CollectionSource
                            {
                                SourcePersonName = x.SourcePersonName,
                                SourceAddress = x.SourceAddress,
                                SourceMobile = x.SourceMobile,
                                SourceDealPrice = x.SourceDealPrice,
                                SourceMessageData = new SelfDeliveryService().SourceMessage(x.StatusId, x.PaymentType, x.SourceAddress, x.CouponPrice, x.SourceDealPrice, x.DeliveryManCommission, x.ProductQtn, x.CouponId, x.DeliveryCharge)
                            },
                            Actions = new SelfDeliveryService().SetActionDataModel(x.StatusId, x.CouponId, model.Flag, x.PaymentType, model.Type, x.CollectionAmount),
                        }).ToList()
                    }).OrderBy(i => i.Id).Skip(model.Index).Take(model.Count).ToList();
            }

            responseTotalData = new SelfDeliveryAllDataResponseModel
            {
                TotalCount = model.Flag == 0 ? response.Select(x => x.CustomerId).Distinct().Count() : response.Select(x => x.CollectionPointId).Distinct().Count(),
                customerOrderResponseModel = data.ToList()
            };

            return responseTotalData;

        }

        public async Task<bool> UpdateBondhuOrder(List<CourierOrders> courierOrders)
        {
            var statusModel = await _sqlServerContext.CourierOrderStatus.Where(x => x.StatusId.Equals(courierOrders.FirstOrDefault().Status)).FirstOrDefaultAsync();
            var courierOrderDetailsViewModel = new CourierOrderDetailsViewModel();

            var orderArray = new List<string>();
            var merchantMobile = "";

            foreach (var item in courierOrders)
            {
                var singleOrder = new LoadCourierOrderBodyModel
                {
                    FromDate = new DateTime(2001, 1, 1),
                    ToDate = new DateTime(2001, 1, 1),
                    PodNumber = "",
                    Mobile = "",
                    Status = -1,
                    CourierUserId = -1,
                    StatusGroup = new string[] { "-1" },
                    StatusList = new int[] { -1 },
                    OrderIds = item.CourierOrdersId,
                    CollectionName = "",
                    DistrictIds = new int[] { -1 },
                    DistrictGroupName = "Select City / Sadar",
                    CourierId = 0,
                    DistrictId = 0,
                    ThanaId = 0,
                    AreaId = 0,
                    PaymentType = "-1",
                };
                courierOrderDetailsViewModel = await _orderService.LoadOrders(singleOrder);
                var model = courierOrderDetailsViewModel.CourierOrderViewModel.FirstOrDefault();
                
                var courierOrderStatusHistory = new CourierOrderStatusHistoryViewModel
                {
                    Id = 0,
                    CourierOrderId = item.CourierOrdersId,
                    OrderDate = Convert.ToDateTime(model.CourierOrderDateDetails.OrderDate),
                    IsConfirmedBy = "deliveryman",
                    CourierId = 35, // Delivey Bondhu courierId
                    Status = item.Status,
                    PostedBy = item.UpdatedBy,
                    Comment = item.Comment,
                    PodNumber = model.CourierOrdersId + "-" + "deliveybondhu",
                    BkashNumber = model.UserInfo.BkashNumber,
                    CompanyName = model.UserInfo.CompanyName,
                    CourierName = model.CourierName,
                    CustomerName = model.CustomerName,
                    DistrictName = model.CourierAddressContactInfo.DistrictName,
                    ThanaName = model.CourierAddressContactInfo.ThanaName,
                    AreaName = model.CourierAddressContactInfo.AreaName,
                    MerchantId = model.UserInfo.CourierUserId,
                    MerchantName = model.UserInfo.CompanyName,
                    MerchantEmail = model.UserInfo.EmailAddress,
                    MerchantMobile = model.UserInfo.Mobile,
                    MerchantPaymentAmount = (model.CourierPrice.CollectionAmount - model.CourierPrice.TotalServiceCharge).ToString(),
                    MessageFormat = statusModel.Message,
                    EmailFormat = statusModel.Email,
                    CustomerMessageFormat = statusModel.CustomerMessage,
                    CustomerEmailFormat = statusModel.CustomerEmail,
                    IsSms = model.AdCourierCommunicationInfo.IsSms,
                    IsEmail = model.AdCourierCommunicationInfo.IsEmail,
                    IsCustomerSms = model.AdCourierCommunicationInfo.IsCustomerSms,
                    IsCustomerEmail = model.AdCourierCommunicationInfo.IsCustomerEmail,
                    CustomerMobile = model.CourierAddressContactInfo.Mobile,
                    CourierCharge = model.CourierPrice.CourierCharge,
                    HubName = "",
                    RetentionMessageFormat = statusModel.RetentionMessage,
                    RetentionUserMobile = model.UserInfo.RetentionUserMobile
                };

                var res = await _orderService.UpdateOrderHistory(item.CourierOrdersId, courierOrderStatusHistory);
                if (res != null)
                {
                    //<<---Accounting Part (Poh Download)--Start--->>
                    //if(item.Status.Equals(44) && model.PaymentServiceType.Equals(1))
                    //{
                    //    bool responsePoh = await _otherService.DownloadPoh(item.CourierOrdersId);
                    //    if (responsePoh.Equals(true))
                    //    {
                    //        await _otherService.BlockCodCodesForPoh(model.UserInfo.CourierUserId);
                    //    }
                    //}
                    //<<---poh download---Ends---->>

                    if (item.Status == 3 || item.Status == 7) //item.Status == 44 remove as per Sourav's bhai requirements
                    {
                        var complainUpdate = await _otherService.UpdateComplainStatus(item.CourierOrdersId);
                    }

                    if (item.Status.Equals(38))
                    {
                        orderArray.Add(item.CourierOrdersId);
                        merchantMobile = courierOrderStatusHistory.MerchantMobile;
                    }
                }
            }

            if (courierOrders.FirstOrDefault().Status.Equals(38))
            {
                var riderName = _sqlServerContext.DeliveryUsers.Where(r => r.Id.Equals(courierOrders.FirstOrDefault().UpdatedBy)).FirstOrDefault().Name;

                SendMobileBodyModel merchantSmsBody = new SendMobileBodyModel
                {
                    numbers = new string[] { merchantMobile },
                    text = "ডেলিভারি ম্যান " + riderName + " নিম্ন লিখিত রিটার্ন পন্য গুলো " + orderArray.Join(",") + " আজ ডেলিভারি দেবার জন্য নিশ্চিত করেছে",
                    datacoding = 0,
                    type = 0
                };

                var listOfData = new List<SendMobileBodyModel>();
                listOfData.Add(merchantSmsBody);
                bool isActive = await _smsEmailService.SmsSend(listOfData);
            }


            return true;
        }

        public async Task<SelfDeliveryModel> DeliveryManRegistration(DeliveryBondhuRegistration bondhuRegistration)
        {
            return await _bondhuRepository.DeliveryManRegistration(bondhuRegistration);
        }

        public async Task<bool> UpdateDeliveryManInfo(DeliveryManGeneralInfoUpdate infoUpdate)
        {
            return await _bondhuRepository.UpdateDeliveryManInfo(infoUpdate);
        }

        public async Task<SelfDeliveryLoginResponseModel> SelfDeliveryLogin(SelfDeliveryLoginModel model)
        {
            SelfDeliveryLoginResponseModel datamodel = new SelfDeliveryLoginResponseModel();
            var response = await _bondhuRepository.SelfDeliveryLogin(model);

            if (response.FirstOrDefault().DeliveryUserId > 0 && response.FirstOrDefault().IsActive == 1)
            {
                datamodel.DeliveryUserId = response.FirstOrDefault().DeliveryUserId;
                datamodel.DeliveryUserName = response.FirstOrDefault().DeliveryUserName;
                datamodel.MobileNumber = response.FirstOrDefault().MobileNumber;
                datamodel.IsActive = response.FirstOrDefault().IsActive;
                datamodel.FirebaseToken = response.FirstOrDefault().FirebaseToken;
                datamodel.BkashMobileNumber = response.FirstOrDefault().BkashMobileNumber;
                datamodel.ProfileImage = response.FirstOrDefault().IsProfileImage == true ? BaseUrlModel.ImageBaseUrl + "images/bondhuprofileimage/" + response.FirstOrDefault().DeliveryUserId + "/profileimage.jpg" : "";
                datamodel.Message = "সফল ভাবে লগইন হয়েছে।";
                return datamodel;
            }
            else
            {
                if (response.FirstOrDefault().DeliveryUserId > 0 && datamodel.IsActive == 0)
                {
                    datamodel.Message = "আপনার একাউন্টটি এখনো সচল না।";
                }
                else
                {
                    datamodel.Message = "মোবাইল নম্বার আথবা পাসওয়ার্ড ভুল আছে।";
                }
                return datamodel;
            }
        }
        public async Task<IEnumerable<OrderStatusCountDeliveryManWise>> GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise(DeliveryBondhuOrderSearchModel searchModel)
        {
            List<OrderStatusCountView> dt = new List<OrderStatusCountView>();
            var data = await _bondhuRepository.GetDeliveryBondhuOrderStatusHistoryCountDeliveryManWise(searchModel);
            var dtDataDeliveryManWise = data.GroupBy(d => d.DeliveryManId).Select(s => new OrderStatusCountDeliveryManWise
            {
                DeliveryManId = s.FirstOrDefault().DeliveryManId,
                DeliveryManName = s.FirstOrDefault().DeliveryManName,
                OrderCounts = s.ToList()
            }).ToList();
            
            return dtDataDeliveryManWise;
        }

        public async Task<IEnumerable<DtOrderDetailsDataModel>> GetDtOrderHistoryDetailsReportForDeliveryMan(DeliveryBondhuOrderSearchModel searchModel)
        {
            return await _bondhuRepository.GetDtOrderHistoryDetailsReportForDeliveryMan(searchModel);
        }

        public async Task<int> UpdateDocumentUrl(List<CourierOrders> orders)
        {
            return await _bondhuRepository.UpdateDocumentUrl(orders);
        }

        public async Task<int> AddLatLag(LatLagModel model)
        {
            return await _bondhuRepository.AddLatLag(model);
        }
        public async Task<int> GetDeliveryBondhuShowOrderAutomatic()
        {
            return await _bondhuRepository.GetDeliveryBondhuShowOrderAutomatic();
        }
        public async Task<IEnumerable<CourierOrders>> GetUpdateTimeSlotAutomatic()
        {
            var listOfOrderId = await _bondhuRepository.GetUpdateTimeSlotAutomatic();

            var orders = new List<CourierOrders>();

            foreach (var item in listOfOrderId)
            {
                orders.Add(new CourierOrders
                {
                   Id = item.Id
                });
            }
            var sendSMS = await _weightRangeRepository.SlotChangeSMSandNotification(orders);

            return listOfOrderId;
        }

        public async Task<DeliveryUsers> UserAccess(int bondhuId, bool isNowOffline)
        {
            var deliveryUsers = new DeliveryUsers()
            {
                IsNowOffline = isNowOffline
            };

            deliveryUsers = await _weightRangeRepository.UpdateNowOfflineRiders(bondhuId, deliveryUsers);

            if (deliveryUsers.IsNowOffline)
            {
                await _bondhuRepository.UserAccess(bondhuId, isNowOffline);
            }

            return deliveryUsers;
        }

        public async Task<UserAccessResponseModel> GetBondhuInfo(int bondhuId)
        {
            return await _bondhuRepository.GetBondhuInfo(bondhuId);
        }

        public async Task<bool> GetBondhuAcceptStatus(CourierOrders courierOrders)
        {
            int startIndex = 3;
            int endIndex = courierOrders.CourierOrdersId.Length - 3;
            courierOrders.Id = Convert.ToInt32(courierOrders.CourierOrdersId.Substring(startIndex, endIndex));

            var data = await _bondhuRepository.GetBondhuAcceptStatus(courierOrders);

            if (data == null)
            {
                return false;
            }
            else if(data.Status == 41 && data.DeliveryUserId != courierOrders.UpdatedBy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<dynamic>> GetBondhuAcceptStatus_Test(List<CourierOrders> courierOrders)
        {
            //Need to remove after completed the test
            var data = new List<dynamic>();

            foreach (var item in courierOrders)
            {
                int startIndex = 3;
                int endIndex = item.CourierOrdersId.Length - 3;
                item.Id = Convert.ToInt32(item.CourierOrdersId.Substring(startIndex, endIndex));

                var response = await _bondhuRepository.GetBondhuAcceptStatus(item);

                if (response == null)
                {
                    data.Add(new
                    {
                        Id = item.Id,
                        IsUpdate = false
                    });
                }
                else if (response.Status == 41 && response.DeliveryUserId != courierOrders.FirstOrDefault().UpdatedBy)
                {
                    data.Add(new
                    {
                        Id = item.Id,
                        IsUpdate = true
                    });
                }
                else
                {
                    data.Add(new
                    {
                        Id = item.Id,
                        IsUpdate = false
                    });
                }

            }

            return data;
        }

        public async Task<int> UpdateSelfDeliveryUserPassword(SelfDeliveryUserPasswordUpdateModel updateModel)
        {
            return await _bondhuRepository.UpdateSelfDeliveryUserPassword(updateModel);
        }

        public async Task<dynamic> GetZoneWiseOrdersCount(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.GetZoneWiseOrdersCount(requestBody);
        }
        public async Task<dynamic> GetZoneWiseOrderDetails(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.GetZoneWiseOrderDetails(requestBody);
        }
        public async Task<IEnumerable<dynamic>> CollectedNotCollectedMerchantInfo(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.CollectedNotCollectedMerchantInfo(requestBody);
        }
        public async Task<IEnumerable<dynamic>> DeliveredAndPendingCustomerInfo(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.DeliveredAndPendingCustomerInfo(requestBody);
        }

        public async Task<dynamic> MerchantWiseOrder(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.MerchantWiseOrder(requestBody);
        }
        public async Task<IEnumerable<dynamic>> GetAllLocationAssignHistory(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.GetAllLocationAssignHistory(requestBody);
        }

        public async Task<dynamic> GetCustomCommentsWithDateRange(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.GetCustomCommentsWithDateRange(requestBody);
        }

        public async Task<dynamic> GetMerchantWiseRiderCountWithDetails(RequestBodyModel requestBody)
        {
            return await _bondhuRepository.GetMerchantWiseRiderCountWithDetails(requestBody);
        }
    }
}
