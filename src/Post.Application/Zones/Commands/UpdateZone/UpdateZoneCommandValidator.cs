using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Post.Application.Common.Interfaces;

namespace Post.Application.Zones.Commands.UpdateZone {
    public class UpdateZoneCommandValidator : AbstractValidator<UpdateZoneCommand> {
        private readonly IApplicationDbContext _context;

        public UpdateZoneCommandValidator(IApplicationDbContext context) {
            _context = context;

            RuleFor(v => v.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.")
                .MustAsync(BeUniqueTitle)
                .WithMessage("The specified title already exists.");
        }

        public async Task<bool> BeUniqueTitle(UpdateZoneCommand model, string title, CancellationToken cancellationToken) {
            return await _context.Zones.Where(l => l.Id != model.Id).AllAsync(l => l.Title != title);
        }
    }
}