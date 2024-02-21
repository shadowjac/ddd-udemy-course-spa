using Domain.Abstractions;

namespace Domain.Reviews;

public sealed record Rating
{
    public static readonly Error Invalid = new(
        "Rating.Invalid",
        "Rating is invalid");

    public int Value { get; init; }

    private Rating(int value) => Value = value;


    public static Result<Rating> Create(int value) =>
        value is < 1 or > 5
            ? Result.Failure<Rating>(Invalid)
            : new Rating(value);
}