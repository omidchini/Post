using MediatR;

namespace Post.Application.Deliveries.Commands.CreateDelivery {
    public class CreateDeliveryCommand : IRequest<int> {
        public int ZoneId { get; set; }

        public string Title { get; set; }
    }
}