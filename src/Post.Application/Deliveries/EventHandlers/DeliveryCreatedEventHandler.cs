using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Post.Application.Common.Models;
using Post.Domain.Events;

namespace Post.Application.Deliveries.EventHandlers {
    public class DeliveryCreatedEventHandler : INotificationHandler<DomainEventNotification<DeliveryCreatedEvent>> {
        private readonly ILogger<DeliveryCompletedEventHandler> _logger;

        public DeliveryCreatedEventHandler(ILogger<DeliveryCompletedEventHandler> logger) {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<DeliveryCreatedEvent> notification, CancellationToken cancellationToken) {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Post Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}