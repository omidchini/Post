using System.Threading.Tasks;

using Post.Domain.Common;

namespace Post.Application.Common.Interfaces {
    public interface IDomainEventService {
        Task Publish(DomainEvent domainEvent);
    }
}