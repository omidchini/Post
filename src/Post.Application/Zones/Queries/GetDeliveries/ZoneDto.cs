using System.Collections.Generic;

using Post.Application.Common.Mappings;
using Post.Domain.Entities;

namespace Post.Application.Zones.Queries.GetDeliveries {
    public class ZoneDto : IMapFrom<Zone> {
        public ZoneDto() {
            Items = new List<DeliveryDto>();
        }

        public string Color { get; set; }

        public int Id { get; set; }

        public IList<DeliveryDto> Items { get; set; }

        public string Title { get; set; }
    }
}