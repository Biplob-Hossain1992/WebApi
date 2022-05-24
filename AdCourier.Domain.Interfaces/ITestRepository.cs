using AdCourier.Domain.Entities.TestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface ITestRepository
    {
        Task<JsonModel> GetJsonData();
    }
}
