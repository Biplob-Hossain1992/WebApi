using AdCourier.Context;
using AdCourier.Domain.Entities.ProcedureDataModel;
using AdCourier.Domain.Interfaces;

namespace AdCourier.Infrastructure.Data
{
    public class OrderGenericRepository : GenericRepository<OrderModel>, IOrderGenericRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public OrderGenericRepository(SqlServerContext sqlServerContext) : base(sqlServerContext)
        {
            this._sqlServerContext = sqlServerContext;
        }
    }
}
