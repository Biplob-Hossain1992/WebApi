using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using Retention.Domain.Interfaces;
using Retention.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Retention.Services
{
    public class RetentionService : IRetentionService
    {
        private readonly IRetentionRepository _retentionRepository;

        public RetentionService(IRetentionRepository retentionRepository)
        {
            _retentionRepository = retentionRepository;
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetRetentionMerchantList(SearchBodyModel searchBodyModel)
        {
            return await _retentionRepository.GetRetentionMerchantList(searchBodyModel);
        }

        public async Task<IEnumerable<dynamic>> GetRetentionMerchantListV1(SearchBodyModel searchBodyModel)
        {
            return await _retentionRepository.GetRetentionMerchantListV1(searchBodyModel);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetOrderWiseRetentionMerchantList(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.GetOrderWiseRetentionMerchantList(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> GetOrderedRetentionMerchantList(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.GetOrderedRetentionMerchantList(requestBodyModel);
        }

        public async Task<MerchantVisited> AddVisitedMerchant(MerchantVisited merchantVisited)
        {
            return await _retentionRepository.AddVisitedMerchant(merchantVisited);
        }

        public async Task<MerchantCalled> AddCalledMerchant(MerchantCalled merchantCalled)
        {
            return await _retentionRepository.AddCalledMerchant(merchantCalled);
        }

        public async Task<dynamic> NewRetentionMerchantFollowUpReport(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.NewRetentionMerchantFollowUpReport(requestBodyModel);
        }

        public async Task<dynamic> NewRetentionMerchantFollowUpReportDetails(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.NewRetentionMerchantFollowUpReportDetails(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> MonthWiseUniqueOrderdMerchantList(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.MonthWiseUniqueOrderdMerchantList(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> FullMonthUniqueOrderedMerchantList(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.FullMonthUniqueOrderedMerchantList(requestBodyModel);
        }

        public async Task<IEnumerable<dynamic>> SrWiseRegularOrderedMerchantList(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.SrWiseRegularOrderedMerchantList(requestBodyModel);
        }

        public async Task<dynamic> SrWiseRetentionMerchantFollowUp(OrderBodyModel orderBodyModel)
        {
            return await _retentionRepository.SrWiseRetentionMerchantFollowUp(orderBodyModel);
        }

        public async Task<dynamic> SrWiseRetentionMerchantFollowUpDetails(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.SrWiseRetentionMerchantFollowUpDetails(requestBodyModel);
        }

        public async Task<IEnumerable<CourierUsersViewModel>> GetTelesalesActiveMerchantList(RequestBodyModel requestBodyModel)
        {
            return await _retentionRepository.GetTelesalesActiveMerchantList(requestBodyModel);
        }

        public async Task<CourierUsersInfoViewModel> GetSrWiseCourierUsersInfo(SearchBodyModel searchBodyModel)
        {
            return await _retentionRepository.GetSrWiseCourierUsersInfo(searchBodyModel);
        }
    }
}
