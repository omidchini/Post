using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Post.Domain.Entities;

namespace Post.Application.Common.Interfaces {
    public interface IApplicationDbContext {
        DbSet<Delivery> Deliveries { get; set; }

        DbSet<Zone> Zones { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}