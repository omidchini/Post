using MediatR;

namespace Post.Application.Zones.Commands.UpdateZone {
    public class UpdateZoneCommand : IRequest {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}