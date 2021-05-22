using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Post.Domain.Entities;

namespace Post.Infrastructure.Persistence.Configurations {
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery> {
        public void Configure(EntityTypeBuilder<Delivery> builder) {
            builder.Ignore(e => e.DomainEvents);

            builder.Property(t => t.Title).HasMaxLength(200).IsRequired();
        }
    }
}