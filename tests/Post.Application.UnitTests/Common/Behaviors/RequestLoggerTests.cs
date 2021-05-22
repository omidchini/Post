using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using Post.Application.Common.Behaviors;
using Post.Application.Common.Interfaces;
using Post.Application.Deliveries.Commands.CreateDelivery;

namespace Post.Application.UnitTests.Common.Behaviors {
    public class RequestLoggerTests {
        private readonly Mock<ICurrentUserService> _currentUserService;

        private readonly Mock<IIdentityService> _identityService;

        private readonly Mock<ILogger<CreateDeliveryCommand>> _logger;

        public RequestLoggerTests() {
            _logger = new Mock<ILogger<CreateDeliveryCommand>>();

            _currentUserService = new Mock<ICurrentUserService>();

            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated() {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new LoggingBehavior<CreateDeliveryCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateDeliveryCommand {
                ZoneId = 1,
                Title = "title"
            }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated() {
            var requestLogger = new LoggingBehavior<CreateDeliveryCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateDeliveryCommand {
                ZoneId = 1,
                Title = "title"
            }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
        }
    }
}