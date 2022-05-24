using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.ViewModel.ReturnProducts;

namespace AdCourier.Domain.Interfaces
{
    public interface IReturnRepository
    {
        Task<ReturnProductsViewModel> GetAllReturnProducts();
        Task<ReturnProductsViewModel> GetAllReturnProductsReport(string statusIds);
    }
}
