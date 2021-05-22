using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Post.Application.Common.Interfaces;
using Post.Application.Common.Mappings;
using Post.Application.Common.Models;
using Post.Application.Zones.Queries.GetDeliveries;

namespace Post.Application.Deliveries.Queries.GetDeliveriesWithPagination {
    public class GetDeliveriesWithPaginationQueryHandler : IRequestHandler<GetDeliveriesWithPaginationQuery, PaginatedList<DeliveryDto>> {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        public GetDeliveriesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<DeliveryDto>> Handle(GetDeliveriesWithPaginationQuery request, CancellationToken cancellationToken) {
            return await _context.Deliveries.Where(x => x.ZoneId == request.ZoneId)
                                 .OrderBy(x => x.Title)
                                 .ProjectTo<DeliveryDto>(_mapper.ConfigurationProvider)
                                 .PaginatedListAsync(request.PageNumber, request.PageSize);
            ;
        }
    }
}