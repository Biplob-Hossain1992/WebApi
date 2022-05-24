using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IInstantCodService
    {
        Task<IEnumerable<CourierOrders>> AddInstantCodOrder(List<CourierOrders> courierOrders);
        Task<dynamic> GetInstantCodCollectionDetails(RequestBodyModel request);
        Task<bool> CheckInstaCod(RequestBodyModel request);
    }
}
