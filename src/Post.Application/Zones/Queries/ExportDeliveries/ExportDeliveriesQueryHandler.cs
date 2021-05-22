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

        private readonly IMapper _mapper;

        public ExportDeliveriesQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder) {
            _context = context;
            _mapper = mapper;
            _fileBuilder = fileBuilder;
        }

        public async Task<ExportDeliveriesVm> Handle(ExportDeliveriesQuery request, CancellationToken cancellationToken) {
            var vm = new ExportDeliveriesVm();

            var records = await _context.Deliveries.Where(t => t.ZoneId == request.ZoneId)
                                        .ProjectTo<DeliveryRecord>(_mapper.ConfigurationProvider)
                                        .ToListAsync(cancellationToken);

            vm.Content = _fileBuilder.BuildDeliveriesFile(records);
            vm.ContentType = "text/csv";
            vm.FileName = "Deliveries.csv";

            return await Task.FromResult(vm);
        }
    }
}