using FluentValidation;

namespace Post.Application.Deliveries.Commands.UpdateDelivery {
    public class UpdateDeliveryCommandValidator : AbstractValidator<UpdateDeliveryCommand> {
        public UpdateDeliveryCommandValidator() {
            RuleFor(v => v.Title).MaximumLength(200).NotEmpty();
        }
    }
}