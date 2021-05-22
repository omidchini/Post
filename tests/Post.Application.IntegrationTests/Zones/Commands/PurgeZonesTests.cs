using System;
using System.Threading.Tasks;

using FluentAssertions;

using NUnit.Framework;

using Post.Application.Common.Exceptions;
using Post.Application.Common.Security;
using Post.Application.Zones.Commands.CreateZone;
using Post.Application.Zones.Commands.PurgeZone;
using Post.Domain.Entities;

namespace Post.Application.IntegrationTests.Zones.Commands {
    using static Testing;

    public class PurgeZonesTests : TestBase {
        [Test]
        public void ShouldDenyAnonymousUser() {
            var command = new PurgeZonesCommand();

            command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<UnauthorizedAccessException>();
        }

        [Test]
        public async Task ShouldDenyNonAdministrator() {
            await RunAsDefaultUserAsync();

            var command = new PurgeZonesCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ForbiddenAccessException>();
        }

        [Test]
        public async Task ShouldAllowAdministrator() {
            await RunAsAdministratorAsync();

            var command = new PurgeZonesCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ForbiddenAccessException>();
        }

        [Test]
        public async Task ShouldDeleteAllLists() {
            await RunAsAdministratorAsync();

            await SendAsync(new CreateZoneCommand { Title = "Zone1" });

            await SendAsync(new CreateZoneCommand { Title = "Zone2" });

            await SendAsync(new CreateZoneCommand { Title = "Zone3" });

            await SendAsync(new PurgeZonesCommand());

            var count = await CountAsync<Zone>();

            count.Should().Be(0);
        }
    }
}