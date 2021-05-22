using MediatR;

using Post.Application.Common.Models;
using Post.Application.Zones.Queries.GetDeliveries;

namespace Post.Application.Deliveries.Queries.GetDeliveriesWithPagination {
    public class GetDeliveriesWithPaginationQuery : IRequest<PaginatedList<DeliveryDto>> {
        public int ZoneId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}