using Domain.Abstractions;

namespace Domain.Reviews;

public static class ReviewErrors
{
    public static readonly Error NotEligible = new("Review.NotEligible",
        "You cannot give a review because is not completed");
}