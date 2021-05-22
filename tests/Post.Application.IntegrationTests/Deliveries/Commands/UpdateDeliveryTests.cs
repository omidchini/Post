using System;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Deliveries.Commands.CreateDelivery;
using Post.Application.Deliveries.Commands.UpdateDelivery;
using Post.Application.Zones.Commands.CreateZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Deliveries.Commands {
    using static Testing;

    public class UpdateDeliveryTests : TestBase {
        [Test]
        public void ShouldRequireValidDeliveryId() {
            var command = new UpdateDeliveryCommand {
                Id = 99,
                Title = "New Delivery"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldUpdateDelivery() {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateZoneCommand { Title = "New Zone" });

            var itemId = await SendAsync(new CreateDeliveryCommand {
                ZoneId = listId,
                Title = "New Delivery"
            });

            var command = new UpdateDeliveryCommand {
                Id = itemId,
                Title = "Updated Delivery Title"
            };

            await SendAsync(command);

            var item = await FindAsync<Delivery>(itemId);

            item.Title.Should().Be(command.Title);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}