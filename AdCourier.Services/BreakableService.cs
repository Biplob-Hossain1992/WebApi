using System.Collections.Generic;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;

namespace AdCourier.Services
{
    public class BreakableService : IBreakableService
    {
        private readonly IBreakableRepository _breakableRepositor;
        public BreakableService(IBreakableRepository breakableRepositor)
        {
            _breakableRepositor = breakableRepositor;
        }
        public async Task<ExtraCharge> AddBreakableCharge(ExtraCharge breakable)
        {
            return await _breakableRepositor.AddBreakableCharge(breakable);
        }

        public async Task<ExtraCharge> GetBreakableCharge()
        {
            return await _breakableRepositor.GetBreakableCharge();
        }

        public async Task<ExtraCharge> UpdateBreakableCharge(int id, ExtraCharge breakable)
        {
            return await _breakableRepositor.UpdateBreakableCharge(id, breakable);
        }
    }
}
