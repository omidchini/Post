using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Interfaces;

namespace Post.Application.Zones.Commands.PurgeZone {
    public class PurgeZonesCommandHandler : IRequestHandler<PurgeZonesCommand> {
        private readonly IApplicationDbContext _context;

        public PurgeZonesCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(PurgeZonesCommand request, CancellationToken cancellationToken) {
            _context.Zones.RemoveRange(_context.Zones);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}