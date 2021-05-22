using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;

namespace Post.Application.Zones.Commands.UpdateZone {
    public class UpdateZoneCommandHandler : IRequestHandler<UpdateZoneCommand> {
        private readonly IApplicationDbContext _context;

        public UpdateZoneCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateZoneCommand request, CancellationToken cancellationToken) {
            var entity = await _context.Zones.FindAsync(request.Id);

            if (entity == null) {
                throw new NotFoundException(nameof(Zone), request.Id);
            }

            entity.Title = request.Title;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}