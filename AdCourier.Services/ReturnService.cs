using AdCourier.Domain.Entities.ViewModel.ReturnProducts;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class ReturnService: IReturnService
    {
        private readonly IReturnRepository _returnRepository;
        public ReturnService(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }

        public async Task<ReturnProductsViewModel> GetAllReturnProducts()
        {
            return await _returnRepository.GetAllReturnProducts();
        }

        public async Task<ReturnProductsViewModel> GetAllReturnProductsReport(string statusIds)
        {
            return await _returnRepository.GetAllReturnProductsReport(statusIds);
        }
    }
}
