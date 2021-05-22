using Post.Domain.Common;
using Post.Domain.Entities;

namespace Post.Domain.Events {
    public class DeliveryCreatedEvent : DomainEvent {
        public DeliveryCreatedEvent(Delivery item) {
            Item = item;
        }

        public Delivery Item { get; }
    }
}