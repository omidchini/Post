using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Post.Application.Common.Interfaces;
using Post.Domain.Entities;

namespace Post.Application.Zones.Commands.CreateZone {
    public class CreateZoneCommandHandler : IRequestHandler<CreateZoneCommand, int> {
        private readonly IApplicationDbContext _context;

        public CreateZoneCommandHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<int> Handle(CreateZoneCommand request, CancellationToken cancellationToken) {
            var entity = new Zone { Title = request.Title };

            _context.Zones.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}