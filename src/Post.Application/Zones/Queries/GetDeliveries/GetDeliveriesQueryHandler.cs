using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Post.Application.Common.Interfaces;
using Post.Domain.Enums;

namespace Post.Application.Zones.Queries.GetDeliveries {
    public class GetDeliveriesQueryHandler : IRequestHandler<GetDeliveriesQuery, DeliveriesVm> {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        public GetDeliveriesQueryHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeliveriesVm> Handle(GetDeliveriesQuery request, CancellationToken cancellationToken) {
            return new DeliveriesVm {
                PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                                     .Cast<PriorityLevel>()
                                     .Select(p => new PriorityLevelDto {
                                         Value = (int)p,
                                         Name = p.ToString()
                                     })
                                     .ToList(),
                Zones = await _context.Zones.AsNoTracking().ProjectTo<ZoneDto>(_mapper.ConfigurationProvider).OrderBy(t => t.Title).ToListAsync(cancellationToken)
            };
        }
    }
}