using System;
using System.Collections.Generic;

namespace Post.Domain.Common {
    public interface IHasDomainEvent {
        public List<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent {
        protected DomainEvent() {
            DateOccurred = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }
    }
}