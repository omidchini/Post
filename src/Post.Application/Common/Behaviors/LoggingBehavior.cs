﻿using System.Threading;
using System.Threading.Tasks;

using MediatR.Pipeline;

using Microsoft.Extensions.Logging;

using Post.Application.Common.Interfaces;

namespace Post.Application.Common.Behaviors {
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> {
        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityService;

        private readonly ILogger _logger;

        public LoggingBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService) {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken) {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId)) {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("Post Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
        }
    }
}