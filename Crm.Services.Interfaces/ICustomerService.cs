using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customers> GetCustomerInformation(int customerId);
    }
}
