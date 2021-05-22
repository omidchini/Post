using FluentValidation;

namespace Post.Application.Deliveries.Queries.GetDeliveriesWithPagination {
    public class GetDeliveriesWithPaginationQueryValidator : AbstractValidator<GetDeliveriesWithPaginationQuery> {
        public GetDeliveriesWithPaginationQueryValidator() {
            RuleFor(x => x.ZoneId).NotNull().NotEmpty().WithMessage("ZoneId is required.");

            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}