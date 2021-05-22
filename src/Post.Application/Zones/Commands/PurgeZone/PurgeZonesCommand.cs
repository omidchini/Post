using MediatR;

using Post.Application.Common.Security;

namespace Post.Application.Zones.Commands.PurgeZone {
    [Authorize(Roles = "Administrator")]
    [Authorize(Policy = "CanPurge")]
    public class PurgeZonesCommand : IRequest { }
}