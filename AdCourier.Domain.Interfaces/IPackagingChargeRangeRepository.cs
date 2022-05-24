using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IPackagingChargeRangeRepository
    {
        Task<List<SurveyQuestionAnswerLog>> AddSurveyQuestionAnswerLog(List<SurveyQuestionAnswerLog> surveyQuestionAnswerLog);
        Task<PackagingChargeRange> AddPackagingChargeRange(PackagingChargeRange packagingChargeRange);
        Task<IEnumerable<PackagingChargeRange>> GetPackagingChargeRange(bool onlyActive);
        Task<PackagingChargeRange> UpdatePackagingChargeRange(int id, PackagingChargeRange packagingChargeRange);
        Task<CourierOrders> UpdateCourierOrders(string id, CourierOrders courierOrders);
        Task<CourierOrders> UpdateDeliveryChargeFromOperation(string courierOrdersId, CourierOrders courierOrders);
        Task<CourierOrders> UpdateCourierOrdersApp(string id, CourierOrders courierOrders);
        Task<CourierOrders> UpdateCourierOrdersAppV2(string id, CourierOrders courierOrders);
        Task<CourierOrders> UpdateOrdersBondhuApp(int id, CourierOrders courierOrders);
        Task<List<Category>> AddDtCategories(List<Category> categories);
        Task<List<SubCategory>> AddSubCategories(List<SubCategory> subCategories);
        Task<Category> UpdateDtCategory(int categoryId, Category category);
        Task<SubCategory> UpdateSubCategory(int subCategoryId, SubCategory subCategory);
        Task<int> UpdateServicePriceBulk(List<DeliveryChargeDetails> deliveryChargeDetails);
        Task<int> UpdateWeightRangePriceBulk(List<DeliveryChargeDetails> deliveryChargeDetails);
        Task<int> UpdateBkashPayment(List<CourierOrders> orders);
        Task<int> UpdateCourierPodnumbers(List<CourierOrders> orders);
        Task<List<DeliveryChargeDetails>> AddAssignCouirerAndService(List<DeliveryChargeDetails> deliveryChargeDetails);
        Task<List<DeliveryChargeMerchantDetails>> AddMerchantWiseAssignCouirerAndService(List<DeliveryChargeMerchantDetails> deliveryChargeMerchantDetails);
        Task<List<BkashPayment>> BkashPaymentLog(List<BkashPayment> request);
        Task<List<CustomerSMS>> AddCustomerSMSLog(List<CustomerSMS> customerSMs);
        Task<List<CouriersWithLoanSurvey>> AddCouriersWithLoanSurvey(List<CouriersWithLoanSurvey> couriersWithLoanSurvey);
    }
}
