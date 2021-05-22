using MediatR;

namespace Post.Application.Zones.Queries.GetDeliveries {
    public class GetDeliveriesQuery : IRequest<DeliveriesVm> { }
}