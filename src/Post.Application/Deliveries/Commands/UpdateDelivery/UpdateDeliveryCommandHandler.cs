using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;

namespace Post.Application.Deliveries.Commands.UpdateDelivery {
    public class UpdateDeliveryCommandHandler : IRequestHandler<UpdateDeliveryCommand> {
        private readonly IApplicationDbContext _context;

        public UpdateDeliveryCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken) {
            var entity = await _context.Deliveries.FindAsync(request.Id);

            if (entity == null) {
                throw new NotFoundException(nameof(Delivery), request.Id);
            }

            entity.Title = request.Title;
            entity.Done = request.Done;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}