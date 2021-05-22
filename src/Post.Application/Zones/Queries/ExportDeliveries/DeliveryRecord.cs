using Post.Application.Common.Mappings;
using Post.Domain.Entities;

namespace Post.Application.Zones.Queries.ExportDeliveries {
    public class DeliveryRecord : IMapFrom<Delivery> {
        public bool Done { get; set; }

        public string Title { get; set; }
    }
}