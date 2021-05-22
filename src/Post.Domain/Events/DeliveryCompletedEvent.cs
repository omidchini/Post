using Post.Domain.Common;
using Post.Domain.Entities;

namespace Post.Domain.Events {
    public class DeliveryCompletedEvent : DomainEvent {
        public DeliveryCompletedEvent(Delivery item) {
            Item = item;
        }

        public Delivery Item { get; }
    }
}