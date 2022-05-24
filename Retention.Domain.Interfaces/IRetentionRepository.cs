using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retention.Domain.Interfaces
{
    public interface IRetentionRepository
    {
        Task<IEnumerable<CourierUsersViewModel>> GetRetentionMerchantList(SearchBodyModel searchBodyModel);
        Task<IEnumerable<dynamic>> GetRetentionMerchantListV1(SearchBodyModel searchBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetOrderWiseRetentionMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> GetOrderedRetentionMerchantList(RequestBodyModel requestBodyModel);
        Task<MerchantVisited> AddVisitedMerchant(MerchantVisited merchantVisited);
        Task<MerchantCalled> AddCalledMerchant(MerchantCalled merchantCalled);
        Task<dynamic> NewRetentionMerchantFollowUpReport(RequestBodyModel requestBodyModel);
        Task<dynamic> NewRetentionMerchantFollowUpReportDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> SrWiseRetentionMerchantFollowUp(OrderBodyModel orderBodyModel);
        Task<IEnumerable<dynamic>> SrWiseRetentionMerchantFollowUpDetails(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> MonthWiseUniqueOrderdMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> FullMonthUniqueOrderedMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<dynamic>> SrWiseRegularOrderedMerchantList(RequestBodyModel requestBodyModel);
        Task<IEnumerable<CourierUsersViewModel>> GetTelesalesActiveMerchantList(RequestBodyModel requestBodyModel);
        Task<CourierUsersInfoViewModel> GetSrWiseCourierUsersInfo(SearchBodyModel searchBodyModel);
    }
}
