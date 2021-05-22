using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Post.Domain.Entities;

namespace Post.Infrastructure.Persistence.Configurations {
    public class ZoneConfiguration : IEntityTypeConfiguration<Zone> {
        public void Configure(EntityTypeBuilder<Zone> builder) {
            builder.Property(t => t.Title).HasMaxLength(200).IsRequired();

            builder.OwnsOne(b => b.Color);
        }
    }
}