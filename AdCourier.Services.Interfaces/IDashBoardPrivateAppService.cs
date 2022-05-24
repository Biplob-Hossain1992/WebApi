using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.BodyModel.Report;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IDashBoardPrivateAppService
    {
        Task<dynamic> GetThirdPartyPaymentInfo(RequestBodyModel request);
        Task<dynamic> GetStatusWiseOrderInfo();
        Task<dynamic> GetDeliveryTigerPaymentInfo(RequestBodyModel request);
        Task<dynamic> GetDtOrders(OrderBodyModel orderBodyModel);
    }
}
