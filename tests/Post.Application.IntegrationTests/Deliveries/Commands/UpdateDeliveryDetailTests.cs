using System;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Deliveries.Commands.CreateDelivery;
using Post.Application.Deliveries.Commands.UpdateDelivery;
using Post.Application.Deliveries.Commands.UpdateDeliveryDetail;
using Post.Application.Zones.Commands.CreateZone;
using Post.Domain.Entities;
using Post.Domain.Enums;

namespace Post.Application.IntegrationTests.Deliveries.Commands {
    using static Testing;

    public class UpdateDeliveryDetailTests : TestBase {
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

            var command = new UpdateDeliveryDetailCommand {
                Id = itemId,
                ZoneId = listId,
                Note = "This is the note.",
                Priority = PriorityLevel.High
            };

            await SendAsync(command);

            var item = await FindAsync<Delivery>(itemId);

            item.ZoneId.Should().Be(command.ZoneId);
            item.Note.Should().Be(command.Note);
            item.Priority.Should().Be(command.Priority);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}