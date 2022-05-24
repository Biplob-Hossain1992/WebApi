using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.BodyModel.CollectorAssign
{
    public class MultipleCollectorAssign
    {
        public int[] CollectorAssignId { get; set; }
        public int CollectorId { get; set; }
        public int UpdatedBy { get; set; }
    }
}
