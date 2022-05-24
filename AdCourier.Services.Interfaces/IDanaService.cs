using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IDanaService
    {
        Task<dynamic> AddDanaJsonData(JsonBodyModel jsonBodyModel);
        Task<PohScore> AddPohScore(PohScore pohScoreModel);
    }
}
