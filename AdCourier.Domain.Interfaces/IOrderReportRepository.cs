﻿using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DapperDataModel;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.RegisteredUsers;
using AdCourier.Domain.Entities.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IOrderReportRepository
    {
        Task<dynamic> GetReferrerRefereeList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierOrdersViewModel>> GetOrderFromWiseOrderCount(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetMerchantOrderFromDetails(LoadCourierOrderBodyModel loadCourierOrderBodyModel);
        Task<IEnumerable<AdCourier.Domain.Entities.ViewModel.CourierUsers.CourierUsersViewModel>> GetPreferredPaymentCycle(OrderBodyModel orderBodyModel);
        Task<IEnumerable<LastMileInformationDapperModel>> GetLastMileInformation(OrderBodyModel orderBodyModel);
        Task<CourierConsignmentViewModel> CourierConsignment(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierReportViewModel>> CourierConsignmentDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<CourierOrdersViewModel>> DeliveredOrderDetails(OrderBodyModel orderBodyModel);
        Task<IEnumerable<RetentionComplainDetailsDapperModel>> RetentionUserWiseComplainDetails(int courierUserId);
        Task<IEnumerable<RetentionUserPerformanceDapperModel>> RetentionUsersPerformance(OrderBodyModel orderBodyModel);
        Task<ServiceTypeNew> GetDistrictSpeedByService(OrderBodyModel orderBodyModel);
        Task<IEnumerable<RetentionUserPerformanceDapperModel>> SRAssignedInactiveMerchantList(int retentionUserId, int inactiveDuration);
        Task<IEnumerable<CourierOrdersViewModel>> GetReceivedOrders(OrderBodyModel orderBodyModel);
        Task<IEnumerable<MerchantOrder>> GetMerchantOrders(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierOrders>> GetOrders(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CollectorReceived>> CollectorReceivedReport(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> CollectorReceivedReportDetails(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> OrderAssign(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> RiderOrderHistory(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> OrderAgeingReport(string statusList);
        Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>> PackageDateDeliveryDateReport(OrderBodyModel orderBodyModel);
        Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.CourierOrdersDapperModel>> PickDateDeliveryDateReport(OrderBodyModel orderBodyModel);
        Task<IEnumerable<Entities.DapperDataModel.CourierOrdersDapperModel>> GetDeliveryRangeTypeWiseOrders(OrderBodyModel orderBodyModel);
        Task<IEnumerable<RegisteredUsersViewModel>> GetAllRegisteredUsers(string joinDate);
        Task<dynamic> GetRetentionAcquisitionUsers(int userId);
        Task<IEnumerable<MerchantDetailsResponseModel>> MerchantDetailsWithOrders(string joinDate, int flag);
        Task<dynamic> AcquisitionManagerWiseOrderDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<MerchantDetailsResponseModel>> MerchantDetails(string joinDate, int flag);
        Task<IEnumerable<AdCourier.Domain.Entities.DapperDataModel.OrderResponseDapperModel>> GetTotalOrdersWithDateFlag(RequestBodyModel requestBody);
        Task<IEnumerable<dynamic>> GetCalledMerchantList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> GetVisitedMerchantList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> DateWiseOrderPlacedCalledMerchantList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> DateWiseOrderPlacedVisitedMerchantList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<OrderStatusCountView>> StatusWiseTotalOrder(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> StatusWiseTotalOrderDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<CourierOrdersViewModel>> CourierWiseReturnReport(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierOrdersViewModel>> PendingShipmentReconciliation(string orderId, int flag);
        Task<IEnumerable<HoursCalculationViewModel>> HoursCalculationReport(RequestBodyModel bodyModel);
        Task<IEnumerable<HoursCalculationViewModel>> HoursCalculationReportForDelivery(RequestBodyModel bodyModel);
        Task<dynamic> EatAnalysisOverCollectionReport(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> GetTelesalesActiveMerchantList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierBillReportViewModel>> GetCourierBillList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<LoanSurveyViewModel>> GetLoanSurvey(OrderBodyModel orderBodyModel);
        Task<IEnumerable<LoanSurveyViewModel>> GetLoanSurveyByLender(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> MonthWiseTotalCollectionAmount(int CourierUserId);
        Task<IEnumerable<dynamic>> AcquisitionLeadManagement(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierOrdersViewModel>> ReAttemptOrdersList(OrderBodyModel orderBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetVouchers(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> TelesalesDetails(int teleSalesStatus, DateTime date);
        Task<IEnumerable<dynamic>> StatusWiseTelesalesDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> StatusWiseHistoryCount(RequestBodyModel bodyModel);
        Task<IEnumerable<dynamic>> StatusWiseHistoryCountDetails(RequestBodyModel bodyModel);
        Task<IEnumerable<dynamic>> GetReRoutedOrders(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> SlotBasedOrder(RequestBodyModel requestBody);
        Task<IEnumerable<dynamic>> TigerReport(RequestBodyModel requestBodyModel);
        Task<IEnumerable<RiderPaymentReportViewModel>> RiderPaymentReport(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> ReturnCourierPendingStatusCount(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> ReturnCourierPendingStatusCountDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> ReturnCourierPendingStatusDetails(RequestBodyModel requestBodyModel);
    }
}
