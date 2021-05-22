using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Post.Application.Common.Interfaces;

namespace Post.Application.Common.Behaviors {
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {
        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityService;

        private readonly ILogger<TRequest> _logger;

        private readonly Stopwatch _timer;

        public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService) {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500) {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId ?? string.Empty;
                var userName = string.Empty;

                if (!string.IsNullOrEmpty(userId)) {
                    userName = await _identityService.GetUserNameAsync(userId);
                }

                _logger.LogWarning("Post Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}", requestName, elapsedMilliseconds,
                    userId, userName, request);
            }

            return response;
        }
    }
}