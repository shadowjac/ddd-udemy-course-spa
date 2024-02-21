namespace Domain.Abstractions;

public record Error(
    string Code,
    string Description)
{
    public static Error None = new(String.Empty, String.Empty);
    public static Error NullValue = new("Error.NullValue", "Null value");
}