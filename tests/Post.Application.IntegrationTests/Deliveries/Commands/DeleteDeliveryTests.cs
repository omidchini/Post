using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Deliveries.Commands.CreateDelivery;
using Post.Application.Deliveries.Commands.DeleteDelivery;
using Post.Application.Zones.Commands.CreateZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Deliveries.Commands {
    using static Testing;

    public class DeleteDeliveryTests : TestBase {
        [Test]
        public void ShouldRequireValidDeliveryId() {
            var command = new DeleteDeliveryCommand { Id = 99 };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteDelivery() {
            var listId = await SendAsync(new CreateZoneCommand { Title = "New Zone" });

            var itemId = await SendAsync(new CreateDeliveryCommand {
                ZoneId = listId,
                Title = "New Zone"
            });

            await SendAsync(new DeleteDeliveryCommand { Id = itemId });

            var list = await FindAsync<Delivery>(listId);

            list.Should().BeNull();
        }
    }
}