using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IDanaRepository
    {
        Task<JsonData> AddDanaJsonData(JsonBodyModel jsonBodyModel);
        Task<PohScore> AddPohScore(PohScore pohScoreModel);
    }
}
