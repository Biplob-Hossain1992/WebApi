using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AdCourier.Infrastructure.Data
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public VoucherRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }


        public async Task<VouchersViewModel> CheckVoucher(VouchersViewModel vouchersViewModel)
        {
            var voucherData = await _sqlServerContext.Vouchers.FirstOrDefaultAsync(x => x.CourierUserId == vouchersViewModel.CourierUserId && x.IsActive == true && vouchersViewModel.DeliveryRangeId == x.DeliveryRangeId);
            var returnVoucher = new VouchersViewModel();

            if (voucherData != null)
            {
                returnVoucher.VoucherId = voucherData.VoucherId;
                returnVoucher.MerchantMobile = voucherData.MerchantMobile;
                returnVoucher.VoucherCode = voucherData.VoucherCode;
                returnVoucher.ApplicableQuantity = voucherData.ApplicableQuantity;
                returnVoucher.StartTime = voucherData.StartTime;
                returnVoucher.EndTime = voucherData.EndTime;
                returnVoucher.VoucherDiscount = voucherData.VoucherDiscount;
                returnVoucher.CourierUserId = voucherData.CourierUserId;
                returnVoucher.IsActive = voucherData.IsActive;
                returnVoucher.DeliveryRangeId = voucherData.DeliveryRangeId;
                returnVoucher.Message = "";

                if (voucherData.VoucherCode != vouchersViewModel.VoucherCode)
                {
                    returnVoucher.Message = "Voucher code is invalid";
                    return returnVoucher;
                }
                else if (!(DateTime.Now >= voucherData.StartTime && DateTime.Now <= voucherData.EndTime))
                {
                    returnVoucher.Message = "Voucher code is expired";
                    return returnVoucher;
                }
                else if (voucherData.ApplicableQuantity == 0)
                {
                    returnVoucher.Message = "Voucher limit is over";
                    return returnVoucher;
                }
                else
                {
                    return returnVoucher;
                }

            }
            returnVoucher.Message = "No voucher is assigned";
            return returnVoucher; 
        }
    }
}
