using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;

namespace Post.Application.Deliveries.Commands.UpdateDeliveryDetail {
    public class UpdateDeliveryDetailCommandHandler : IRequestHandler<UpdateDeliveryDetailCommand> {
        private readonly IApplicationDbContext _context;

        public UpdateDeliveryDetailCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateDeliveryDetailCommand request, CancellationToken cancellationToken) {
            var entity = await _context.Deliveries.FindAsync(request.Id);

            if (entity == null) {
                throw new NotFoundException(nameof(Delivery), request.Id);
            }

            entity.ZoneId = request.ZoneId;
            entity.Priority = request.Priority;
            entity.Note = request.Note;
            entity.Title = request.Title;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}