using MediatR;

using Post.Domain.Enums;

namespace Post.Application.Deliveries.Commands.UpdateDeliveryDetail {
    public class UpdateDeliveryDetailCommand : IRequest {
        public int Id { get; set; }

        public int ZoneId { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; set; }
    }
}