using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Post.Application.Common.Exceptions;
using Post.Application.Common.Interfaces;
using Post.Domain.Entities;

namespace Post.Application.Zones.Commands.DeleteZone {
    public class DeleteZoneCommandHandler : IRequestHandler<DeleteZoneCommand> {
        private readonly IApplicationDbContext _context;

        public DeleteZoneCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteZoneCommand request, CancellationToken cancellationToken) {
            var entity = await _context.Zones.Where(l => l.Id == request.Id).SingleOrDefaultAsync(cancellationToken);

            if (entity == null) {
                throw new NotFoundException(nameof(Zone), request.Id);
            }

            _context.Zones.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}