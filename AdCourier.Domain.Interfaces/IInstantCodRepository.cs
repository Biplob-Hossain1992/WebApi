using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IInstantCodRepository
    {
        Task<CourierOrders> UpdateInstantCodOrder(CourierOrders courierOrders);
        Task<IEnumerable<dynamic>> GetInstantCodOrders(RequestBodyModel request);
        Task<dynamic> GetInstantCodCollectionDetails(RequestBodyModel request);
        Task<IEnumerable<CourierLocation>> GetCourierLocations(int isActive);
        Task<CourierLocation> AddCourierLocation(CourierLocation courierLocation);
        Task<bool> CheckInstaCod(RequestBodyModel request);

    }
}
