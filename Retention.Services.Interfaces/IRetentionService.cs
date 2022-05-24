using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;

namespace Retention.Services.Interfaces
{
    public interface IRetentionService
    {
        Task<IEnumerable<CourierUsersViewModel>> GetRetentionMerchantList(SearchBodyModel searchBodyModel);
        Task<IEnumerable<dynamic>> GetRetentionMerchantListV1(SearchBodyModel searchBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetOrderWiseRetentionMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> GetOrderedRetentionMerchantList(RequestBodyModel requestBodyModel);
        Task<MerchantVisited> AddVisitedMerchant(MerchantVisited merchantVisited);
        Task<MerchantCalled> AddCalledMerchant(MerchantCalled merchantCalled);
        Task<dynamic> NewRetentionMerchantFollowUpReport(RequestBodyModel requestBodyModel);
        Task<dynamic> NewRetentionMerchantFollowUpReportDetails(RequestBodyModel requestBodyModel);
        Task<dynamic> SrWiseRetentionMerchantFollowUp(OrderBodyModel orderBodyModel);
        Task<dynamic> SrWiseRetentionMerchantFollowUpDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> MonthWiseUniqueOrderdMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> FullMonthUniqueOrderedMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> SrWiseRegularOrderedMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetTelesalesActiveMerchantList(RequestBodyModel requestBodyModel);
        Task<CourierUsersInfoViewModel> GetSrWiseCourierUsersInfo(SearchBodyModel searchBodyModel);
    }
}
