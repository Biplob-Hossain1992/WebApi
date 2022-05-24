using AdCourier.Domain.Entities.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IBreakableRepository
    {
        Task<ExtraCharge> AddBreakableCharge(ExtraCharge breakable);
        Task<ExtraCharge> GetBreakableCharge();
        Task<ExtraCharge> UpdateBreakableCharge(int id, ExtraCharge breakable);
    }
}
