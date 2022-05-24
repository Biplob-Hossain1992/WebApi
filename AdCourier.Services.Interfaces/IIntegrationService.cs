using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.IntegrationBody;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IIntegrationService
    {
        Task<string> order(IntegrationOrderBodyModel request);
    }
}
