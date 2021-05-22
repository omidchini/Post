using MediatR;

namespace Post.Application.Zones.Queries.ExportDeliveries {
    public class ExportDeliveriesQuery : IRequest<ExportDeliveriesVm> {
        public int ZoneId { get; set; }
    }
}