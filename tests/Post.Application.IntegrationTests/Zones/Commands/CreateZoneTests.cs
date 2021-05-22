using System;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Zones.Commands.CreateZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Zones.Commands {
    using static Testing;

    public class CreateZoneTests : TestBase {
        [Test]
        public void ShouldRequireMinimumFields() {
            var command = new CreateZoneCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireUniqueTitle() {
            await SendAsync(new CreateZoneCommand { Title = "Zone" });

            var command = new CreateZoneCommand { Title = "Zone" };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateZone() {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateZoneCommand { Title = "Zone" };

            var id = await SendAsync(command);

            var list = await FindAsync<Zone>(id);

            list.Should().NotBeNull();
            list.Title.Should().Be(command.Title);
            list.CreatedBy.Should().Be(userId);
            list.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}