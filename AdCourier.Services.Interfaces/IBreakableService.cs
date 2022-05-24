using AdCourier.Domain.Entities.DataModel;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IBreakableService
    {
        Task<ExtraCharge> AddBreakableCharge(ExtraCharge breakable);
        Task<ExtraCharge> GetBreakableCharge();
        Task<ExtraCharge> UpdateBreakableCharge(int id, ExtraCharge breakable);
        
    }
}
