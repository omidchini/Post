using MediatR;

namespace Post.Application.Deliveries.Commands.UpdateDelivery {
    public class UpdateDeliveryCommand : IRequest {
        public bool Done { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }
    }
}