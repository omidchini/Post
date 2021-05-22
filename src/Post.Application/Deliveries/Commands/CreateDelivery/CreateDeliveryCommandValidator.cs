using FluentValidation;

namespace Post.Application.Deliveries.Commands.CreateDelivery {
    public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand> {
        public CreateDeliveryCommandValidator() {
            RuleFor(v => v.Title).MaximumLength(200).NotEmpty();
        }
    }
}