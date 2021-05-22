using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Interfaces;
using Post.Domain.Entities;
using Post.Domain.Events;

namespace Post.Application.Deliveries.Commands.CreateDelivery {
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, int> {
        private readonly IApplicationDbContext _context;

        public CreateDeliveryCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<int> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken) {
            var entity = new Delivery {
                ZoneId = request.ZoneId,
                Title = request.Title,
                Done = false
            };

            entity.DomainEvents.Add(new DeliveryCreatedEvent(entity));

            _context.Deliveries.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}