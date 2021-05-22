using MediatR;

namespace Post.Application.Zones.Commands.DeleteZone {
    public class DeleteZoneCommand : IRequest {
        public int Id { get; set; }
    }
}