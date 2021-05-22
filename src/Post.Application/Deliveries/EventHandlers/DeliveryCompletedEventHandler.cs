using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Post.Application.Common.Models;
using Post.Domain.Events;

namespace Post.Application.Deliveries.EventHandlers {
    public class DeliveryCompletedEventHandler : INotificationHandler<DomainEventNotification<DeliveryCompletedEvent>> {
        private readonly ILogger<DeliveryCompletedEventHandler> _logger;

        public DeliveryCompletedEventHandler(ILogger<DeliveryCompletedEventHandler> logger) {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<DeliveryCompletedEvent> notification, CancellationToken cancellationToken) {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Post Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}