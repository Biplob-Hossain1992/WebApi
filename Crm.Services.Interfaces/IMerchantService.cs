using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Services.Interfaces
{
    public interface IMerchantService
    {
        Task<UserProfile> GetMerchantInformation(int merchantId);
    }
}
