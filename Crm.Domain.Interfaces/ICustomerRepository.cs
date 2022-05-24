using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customers> GetCustomerInformation(int customerId);
    }
}
