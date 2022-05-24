using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AdCourier.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCourier.Services
{
    public class PackagingChargeRangeService : IPackagingChargeRangeService
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly IWeightRangeRepository _weightRangeRepository;
        private readonly IPackagingChargeRangeRepository _packagingChargeRangeRepository;
        private readonly IDeliveryRangeRepository _deliveryRangeRepository;
        public PackagingChargeRangeService(SqlServerContext sqlServerContext, IDeliveryRangeRepository deliveryRangeRepository, IWeightRangeRepository weightRangeRepository, IPackagingChargeRangeRepository packagingChargeRangeRepository)
        {
            _packagingChargeRangeRepository = packagingChargeRangeRepository;
            _weightRangeRepository = weightRangeRepository;
            _deliveryRangeRepository = deliveryRangeRepository;
            _sqlServerContext = sqlServerContext;
        }

        public async Task<PackagingChargeRange> AddPackagingChargeRange(PackagingChargeRange packagingChargeRange)
        {
            return await _packagingChargeRangeRepository.AddPackagingChargeRange(packagingChargeRange);
        }
        public async Task<IEnumerable<PackagingChargeRange>> GetPackagingChargeRange(bool onlyActive)
        {
            return await _packagingChargeRangeRepository.GetPackagingChargeRange(onlyActive);
        }

        public async Task<CourierOrders> UpdateCourierOrders(string id, CourierOrders courierOrders)
        {
            return await _packagingChargeRangeRepository.UpdateCourierOrders(id, courierOrders);
        }

        public async Task<CourierOrders> UpdateDeliveryChargeFromOperation(string id, CourierOrders courierOrders)
        {
            return await _packagingChargeRangeRepository.UpdateDeliveryChargeFromOperation(id, courierOrders);
        }

        public async Task<CourierOrders> UpdateCourierOrdersApp(string id, CourierOrders courierOrders)
        {
            return await _packagingChargeRangeRepository.UpdateCourierOrdersApp(id, courierOrders);
        }

        public async Task<CourierOrders> UpdateCourierOrdersAppV2(string id, CourierOrders courierOrders)
        {
            return await _packagingChargeRangeRepository.UpdateCourierOrdersAppV2(id, courierOrders);
        }

        public async Task<PackagingChargeRange> UpdatePackagingChargeRange(int id, PackagingChargeRange packagingChargeRange)
        {
            return await _packagingChargeRangeRepository.UpdatePackagingChargeRange(id, packagingChargeRange);
        }

        public async Task<CourierOrders> UpdateOrdersBondhuApp(int id, CourierOrders courierOrders)
        {
            return await _packagingChargeRangeRepository.UpdateOrdersBondhuApp(id, courierOrders);
        }

        public async Task<List<SurveyQuestionAnswerLog>> AddSurveyQuestionAnswerLog(List<SurveyQuestionAnswerLog> surveyQuestionAnswerLog)
        {
            return await _packagingChargeRangeRepository.AddSurveyQuestionAnswerLog(surveyQuestionAnswerLog);
        }
        public async Task<List<DeliveryChargeDetails>> AddAssignCouirerAndService(List<DeliveryChargeDetails> deliveryChargeDetails)
        {

            var deliveryChargeDetailsList = new List<DeliveryChargeDetails>();
            var weightRanges = await _weightRangeRepository.GetWeightRange();

            var deliveryRanges = await _deliveryRangeRepository.GetDeliveryRange();

            foreach (var item in deliveryChargeDetails)
            {
                if (weightRanges.Count > 0)
                {
                    weightRanges = weightRanges.Where(w => w.WeightNumber > 0).OrderBy(w => w.WeightNumber).ToList();

                    var deliveryRange = deliveryRanges.Where(d => d.Id.Equals(item.DeliveryRangeId)).FirstOrDefault();

                    decimal deliveryCharge = 0;

                    foreach (var range in weightRanges)
                    {
                        if (deliveryRange.Type.Trim().ToLower().Equals("express"))
                        {
                            deliveryCharge = range.ExpressTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                        }
                        else if (deliveryRange.Type.Trim().ToLower().Equals("regular"))
                        {
                            deliveryCharge = range.RegularTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                        }

                        deliveryChargeDetailsList.Add(new DeliveryChargeDetails
                        {
                            DistrictId = item.DistrictId,
                            ThanaId = item.ThanaId,
                            AreaId = item.AreaId,
                            WeightRangeId = range.Id,
                            DeliveryRangeId = item.DeliveryRangeId,
                            CourierId = item.CourierId,
                            CourierDeliveryCharge = deliveryCharge,
                            ServiceType = item.ServiceType,
                            IsActive = item.IsActive
                        });
                    }
                }
            }

            return await _packagingChargeRangeRepository.AddAssignCouirerAndService(deliveryChargeDetailsList);
        }

        public async Task<List<DeliveryChargeMerchantDetails>> AddMerchantWiseAssignCouirerAndService(List<DeliveryChargeMerchantDetails> deliveryChargeMerchantDetails)
        {

            var deliveryChargeMerchantDetailsList = new List<DeliveryChargeMerchantDetails>();
            var weightRanges = await _weightRangeRepository.GetWeightRange();

            var deliveryRanges = await _deliveryRangeRepository.GetDeliveryRange();

            foreach (var item in deliveryChargeMerchantDetails)
            {
                if (weightRanges.Count > 0)
                {
                    weightRanges = weightRanges.Where(w => w.WeightNumber > 0).OrderBy(w => w.WeightNumber).ToList();

                    var deliveryRange = deliveryRanges.Where(d => d.Id.Equals(item.DeliveryRangeId)).FirstOrDefault();

                    decimal deliveryCharge = 0;

                    foreach (var range in weightRanges)
                    {
                        if (deliveryRange.Type.Trim().ToLower().Equals("express"))
                        {
                            deliveryCharge = range.ExpressTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                        }
                        else if (deliveryRange.Type.Trim().ToLower().Equals("regular"))
                        {
                            deliveryCharge = range.RegularTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                        }

                        deliveryChargeMerchantDetailsList.Add(new DeliveryChargeMerchantDetails
                        {
                            DistrictId = item.DistrictId,
                            ThanaId = item.ThanaId,
                            AreaId = item.AreaId,
                            WeightRangeId = range.Id,
                            DeliveryRangeId = item.DeliveryRangeId,
                            CourierId = item.CourierId,
                            CourierDeliveryCharge = deliveryCharge,
                            ServiceType = item.ServiceType,
                            IsActive = item.IsActive,
                            CourierUserId = item.CourierUserId,
                            IsSpecial = item.IsSpecial
                        });
                    }
                }
            }

            return await _packagingChargeRangeRepository.AddMerchantWiseAssignCouirerAndService(deliveryChargeMerchantDetailsList);
        }

        public async Task<List<Category>> AddDtCategories(List<Category> categories)
        {
            return await _packagingChargeRangeRepository.AddDtCategories(categories);
        }
        public async Task<List<SubCategory>> AddSubCategories(List<SubCategory> subCategories)
        {
            return await _packagingChargeRangeRepository.AddSubCategories(subCategories);
        }

        public async Task<Category> UpdateDtCategory(int categoryId, Category category)
        {
            return await _packagingChargeRangeRepository.UpdateDtCategory(categoryId, category);
        }
        public async Task<SubCategory> UpdateSubCategory(int subCategoryId, SubCategory subCategory)
        {
            return await _packagingChargeRangeRepository.UpdateSubCategory(subCategoryId, subCategory);
        }

        public async Task<int> UpdateWeightRangePriceBulk(WeightRange weightRange)
        {
            var deliveryRanges = await _deliveryRangeRepository.GetDeliveryRange();

            var deliveryChargeDetails = _sqlServerContext.DeliveryChargeDetails.AsNoTracking().Where(x => x.WeightRangeId.Equals(weightRange.Id)).OrderBy(x => x.WeightRangeId).ToList();

            foreach (var item in deliveryChargeDetails)
            {
                var deliveryRange = deliveryRanges.Where(d => d.Id.Equals(item.DeliveryRangeId)).FirstOrDefault();

                if (deliveryRange.Type.Trim().ToLower().Equals("express"))
                {
                    item.CourierDeliveryCharge = weightRange.ExpressTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                }
                else if (deliveryRange.Type.Trim().ToLower().Equals("regular"))
                {
                    item.CourierDeliveryCharge = weightRange.RegularTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                }
            }
            return await _packagingChargeRangeRepository.UpdateWeightRangePriceBulk(deliveryChargeDetails);
        }


        public async Task<int> UpdateBkashPayment(List<CourierOrders> orders)
        {
            if (orders.FirstOrDefault().Status == 24)
            {
                var ids = orders.Select(b =>b.Id).ToArray();

                var _order = await _sqlServerContext.CourierOrders.Where(x => ids.Contains(x.Id) && x.Status.Equals(25)).ToListAsync();

                var firstUpdateStatus = await _packagingChargeRangeRepository.UpdateBkashPayment(orders);

                if (_order.Count() > 0)
                {
                    _order.ForEach(y =>
                    {
                        y.TransactionId = orders.Where(z => z.Id.Equals(y.Id)).FirstOrDefault().TransactionId;
                        y.ValidationId = orders.Where(z => z.Id.Equals(y.Id)).FirstOrDefault().ValidationId;
                    });

                    var secondUpdateStatus = await _packagingChargeRangeRepository.UpdateBkashPayment(_order);
                }
                return firstUpdateStatus;
            }
            else
            {
                var updateStatus = await _packagingChargeRangeRepository.UpdateBkashPayment(orders);
                return updateStatus;
            }

        }

        public async Task<int> UpdateCourierPodnumbers(List<CourierOrders> orders)
        {
            return await _packagingChargeRangeRepository.UpdateCourierPodnumbers(orders);
        }

        public async Task<int> UpdateServicePriceBulk(DeliveryRange deliveryRange)
        {

            var weightRanges = await _weightRangeRepository.GetWeightRange();

            var deliveryChargeDetails = _sqlServerContext.DeliveryChargeDetails.AsNoTracking().Where(x => x.DeliveryRangeId.Equals(deliveryRange.Id)).OrderBy(x => x.WeightRangeId).ToList();
            foreach (var item in deliveryChargeDetails)
            {
                var singleWeightRange = weightRanges.Where(w => w.Id.Equals(item.WeightRangeId)).FirstOrDefault();

                if (deliveryRange.Type.Trim().ToLower().Equals("express"))
                {
                    item.CourierDeliveryCharge = singleWeightRange.ExpressTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                }
                else if (deliveryRange.Type.Trim().ToLower().Equals("regular"))
                {
                    item.CourierDeliveryCharge = singleWeightRange.RegularTypeCourierDeliveryCharge + deliveryRange.CourierDeliveryCharge;
                }
            }
            return await _packagingChargeRangeRepository.UpdateServicePriceBulk(deliveryChargeDetails);
        }

        public async Task<List<BkashPayment>> BkashPaymentLog(List<BkashPayment> request)
        {
            return await _packagingChargeRangeRepository.BkashPaymentLog(request);
        }

        public async Task<List<CustomerSMS>> AddCustomerSMSLog(List<CustomerSMS> customerSMs)
        {
            return await _packagingChargeRangeRepository.AddCustomerSMSLog(customerSMs);
        }

        public async Task<List<CouriersWithLoanSurvey>> AddCouriersWithLoanSurvey(List<CouriersWithLoanSurvey> couriersWithLoanSurvey)
        {
            return await _packagingChargeRangeRepository.AddCouriersWithLoanSurvey(couriersWithLoanSurvey);
        }
    }
}
