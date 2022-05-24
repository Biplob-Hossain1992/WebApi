using AdCourier.Domain.Entities.TestModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface ITestService
    {
        Task<dynamic> GetJsonData();
    }
}
