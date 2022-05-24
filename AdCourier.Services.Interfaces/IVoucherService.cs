using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IVoucherService
    {
        Task<VouchersViewModel> CheckVoucher(VouchersViewModel vouchersViewModel);
    }
}
