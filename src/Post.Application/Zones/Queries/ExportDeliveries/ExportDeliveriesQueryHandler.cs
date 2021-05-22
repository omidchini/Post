using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Post.Application.Common.Interfaces;

namespace Post.Application.Zones.Queries.ExportDeliveries {
    public class ExportDeliveriesQueryHandler : IRequestHandler<ExportDeliveriesQuery, ExportDeliveriesVm> {
        private readonly IApplicationDbContext _context;

        private readonly ICsvFileBuilder _fileBuilder;

        private readonly IDateTime _dateTime;

        private readonly IMapper _mapper;

        public ExportDeliveriesQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder, IDateTime dateTime) {
            _context = context;
            _mapper = mapper;
            _fileBuilder = fileBuilder;
            _dateTime = dateTime;
        }

        public async Task<ExportDeliveriesVm> Handle(ExportDeliveriesQuery request, CancellationToken cancellationToken) {
            var vm = new ExportDeliveriesVm();

            var zone = await _context.Zones.SingleAsync(t => t.Id == request.ZoneId, cancellationToken);
            var records = await _context.Deliveries.Where(t => t.ZoneId == request.ZoneId)
                                        .ProjectTo<DeliveryRecord>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);

            vm.Content = _fileBuilder.BuildDeliveriesFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = $"{zone.Id}_{zone.Title}_Deliveries_{_dateTime.Now:yyyyMMddHHmmss}.csv";

            return await Task.FromResult(vm);
        }
    }
}