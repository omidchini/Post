using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Zones.Commands.CreateZone;
using Post.Application.Zones.Commands.DeleteZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Zones.Commands {
    using static Testing;

    public class DeleteZoneTests : TestBase {
        [Test]
        public void ShouldRequireValidZoneId() {
            var command = new DeleteZoneCommand { Id = 99 };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteZone() {
            var listId = await SendAsync(new CreateZoneCommand { Title = "New Zone" });

            await SendAsync(new DeleteZoneCommand { Id = listId });

            var list = await FindAsync<Zone>(listId);

            list.Should().BeNull();
        }
    }
}