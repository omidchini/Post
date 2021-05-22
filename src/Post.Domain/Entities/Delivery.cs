using System;
using System.Collections.Generic;

using Post.Domain.Common;
using Post.Domain.Enums;
using Post.Domain.Events;

namespace Post.Domain.Entities {
    public class Delivery : AuditableEntity, IHasDomainEvent {
        private bool _done;

        public List<DomainEvent> DomainEvents { get; set; } = new();

        public bool Done {
            get => _done;
            set {
                if (value && (_done == false)) {
                    DomainEvents.Add(new DeliveryCompletedEvent(this));
                }

                _done = value;
            }
        }

        public int Id { get; set; }

        public Zone Zone { get; set; }

        public int ZoneId { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; set; }

        public DateTime? Reminder { get; set; }

        public string Title { get; set; }
    }
}