using AutoMapper;

using Post.Application.Common.Mappings;
using Post.Domain.Entities;

namespace Post.Application.Zones.Queries.GetDeliveries {
    public class DeliveryDto : IMapFrom<Delivery> {
        public bool Done { get; set; }

        public int Id { get; set; }

        public int ZoneId { get; set; }

        public string Note { get; set; }

        public int Priority { get; set; }

        public string Title { get; set; }

        public void Mapping(Profile profile) {
            profile.CreateMap<Delivery, DeliveryDto>().ForMember(d => d.Priority, opt => opt.MapFrom(s => (int)s.Priority));
        }
    }
}