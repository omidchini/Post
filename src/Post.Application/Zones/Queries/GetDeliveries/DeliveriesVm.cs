using System.Collections.Generic;

namespace Post.Application.Zones.Queries.GetDeliveries {
    public class DeliveriesVm {
        public IList<ZoneDto> Zones { get; set; }

        public IList<PriorityLevelDto> PriorityLevels { get; set; }
    }
}