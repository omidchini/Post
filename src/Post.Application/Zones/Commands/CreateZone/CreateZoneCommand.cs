using MediatR;

namespace Post.Application.Zones.Commands.CreateZone {
    public class CreateZoneCommand : IRequest<int> {
        public string Title { get; set; }
    }
}