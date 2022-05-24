using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.IntegrationBody;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IIntegrationRepository _integrationRepository;
        public IntegrationService(IIntegrationRepository integrationRepository)
        {
            _integrationRepository = integrationRepository;
        }
        public async Task<string> order(IntegrationOrderBodyModel request)
        {
            var data = await _integrationRepository.order(request);

            return data;
        }
    }
}
