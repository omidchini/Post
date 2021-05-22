using System.Collections.Generic;

using Post.Domain.Common;
using Post.Domain.ValueObjects;

namespace Post.Domain.Entities {
    public class Zone : AuditableEntity {
        public Color Color { get; set; } = Color.White;

        public int Id { get; set; }

        public IList<Delivery> Items { get; } = new List<Delivery>();

        public string Title { get; set; }
    }
}