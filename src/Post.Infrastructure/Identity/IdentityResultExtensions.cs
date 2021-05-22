using System.Linq;

using Microsoft.AspNetCore.Identity;

using Post.Application.Common.Models;

namespace Post.Infrastructure.Identity {
    public static class IdentityResultExtensions {
        public static Result ToApplicationResult(this IdentityResult result) {
            return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}