using System;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Deliveries.Commands.CreateDelivery;
using Post.Application.Zones.Commands.CreateZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Deliveries.Commands {
    using static Testing;

    public class CreateDeliveryTests : TestBase {
        [Test]
        public void ShouldRequireMinimumFields() {
            var command = new CreateDeliveryCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateDelivery() {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateZoneCommand { Title = "New Zone" });

            var command = new CreateDeliveryCommand {
                ZoneId = listId,
                Title = "Delvery1"
            };

            var itemId = await SendAsync(command);

            var item = await FindAsync<Delivery>(itemId);

            item.Should().NotBeNull();
            item.ZoneId.Should().Be(command.ZoneId);
            item.Title.Should().Be(command.Title);
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 10000);
            item.LastModifiedBy.Should().BeNull();
            item.LastModified.Should().BeNull();
        }
    }
}