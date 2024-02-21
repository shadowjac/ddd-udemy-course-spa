using FluentValidation;

namespace Application.Vehicles.SearchVehicles;

public class QueryValidator : AbstractValidator<SearchVehiclesQuery>
{
    public QueryValidator()
    {
        RuleFor(x => x.Test)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(10);
    }
}