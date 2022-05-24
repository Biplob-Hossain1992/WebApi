using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.ReturnProducts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IReturnService
    {
        Task<ReturnProductsViewModel> GetAllReturnProducts();
        Task<ReturnProductsViewModel> GetAllReturnProductsReport(string statusIds);
    }
}
