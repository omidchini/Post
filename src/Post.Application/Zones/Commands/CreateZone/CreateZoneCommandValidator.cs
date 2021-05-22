using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Post.Application.Common.Interfaces;

namespace Post.Application.Zones.Commands.CreateZone {
    public class CreateZoneCommandValidator : AbstractValidator<CreateZoneCommand> {
        private readonly IApplicationDbContext _context;

        public CreateZoneCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.")
                .MustAsync(BeUniqueTitle)
                .WithMessage("The specified title already exists.");
        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken) {
            return await _context.Zones.AllAsync(l => l.Title != title, cancellationToken);
        }
    }
}