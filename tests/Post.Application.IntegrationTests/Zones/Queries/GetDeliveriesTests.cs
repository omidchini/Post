using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Zones.Queries.GetDeliveries;
using Post.Domain.Entities;
using Post.Domain.ValueObjects;

namespace Post.Application.IntegrationTests.Zones.Queries {
    using static Testing;

    public class GetDeliveriesTests : TestBase {
        [Test]
        public async Task ShouldReturnPriorityLevels() {
            var query = new GetDeliveriesQuery();

            var result = await SendAsync(query);

            result.PriorityLevels.Should().NotBeEmpty();
        }

        [Test]
        public async Task ShouldReturnAllListsAndItems() {
            await AddAsync(new Zone {
                Title = "Zone1",
                Color = Color.Blue,
                Items = {
                    new Delivery {
                        Title = "Item1",
                        Done = true
                    },
                    new Delivery {
                        Title = "Item2",
                        Done = true
                    },
                    new Delivery {
                        Title = "Item3",
                        Done = true
                    },
                    new Delivery { Title = "Item4" },
                    new Delivery { Title = "Item5" },
                    new Delivery { Title = "Item6" },
                    new Delivery { Title = "Item7" }
                }
            });

            var query = new GetDeliveriesQuery();

            var result = await SendAsync(query);

            result.Zones.Should().HaveCount(1);
            result.Zones.First().Items.Should().HaveCount(7);
        }
    }
}