using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;

namespace Post.Application.Deliveries.Commands.DeleteDelivery {
    public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand> {
        private readonly IApplicationDbContext _context;

        public DeleteDeliveryCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken) {
            var entity = await _context.Deliveries.FindAsync(request.Id);

            if (entity == null) {
                throw new NotFoundException(nameof(Delivery), request.Id);
            }

            _context.Deliveries.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}