using MediatR;

using Post.Domain.Common;

namespace Post.Application.Common.Models {
    public class DomainEventNotification<TDomainEvent> : INotification
        where TDomainEvent : DomainEvent {
        public DomainEventNotification(TDomainEvent domainEvent) {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}