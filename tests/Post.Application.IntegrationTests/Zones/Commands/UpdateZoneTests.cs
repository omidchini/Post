using System;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Zones.Commands.CreateZone;
using Post.Application.Zones.Commands.UpdateZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Zones.Commands {
    using static Testing;

    public class UpdateZoneTests : TestBase {
        [Test]
        public void ShouldRequireValidZoneId() {
            var command = new UpdateZoneCommand {
                Id = 99,
                Title = "New Title"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireUniqueTitle() {
            var listId = await SendAsync(new CreateZoneCommand { Title = "New Zone" });

            await SendAsync(new CreateZoneCommand { Title = "Other Zone" });

            var command = new UpdateZoneCommand {
                Id = listId,
                Title = "Other Zone"
            };

            FluentActions.Invoking(() => SendAsync(command))
                         .Should()
                         .Throw<ValidationException>()
                         .Where(ex => ex.Errors.ContainsKey("Title"))
                         .And.Errors["Title"]
                         .Should()
                         .Contain("The specified title already exists.");
        }

        [Test]
        public async Task ShouldUpdateZone() {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateZoneCommand { Title = "New Zone" });

            var command = new UpdateZoneCommand {
                Id = listId,
                Title = "Updated Zone Title"
            };

            await SendAsync(command);

            var list = await FindAsync<Zone>(listId);

            list.Title.Should().Be(command.Title);
            list.LastModifiedBy.Should().NotBeNull();
            list.LastModifiedBy.Should().Be(userId);
            list.LastModified.Should().NotBeNull();
            list.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}