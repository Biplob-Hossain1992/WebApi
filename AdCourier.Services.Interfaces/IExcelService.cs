using AdCourier.Domain.Entities.ProcedureDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IExcelService
    {
        Task<int> ExportExcel(IQueryable<OrderModel> orderModel);
    }
}
