using MediatR;

namespace Post.Application.Deliveries.Commands.DeleteDelivery {
    public class DeleteDeliveryCommand : IRequest {
        public int Id { get; set; }
    }
}