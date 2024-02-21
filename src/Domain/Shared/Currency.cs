namespace Domain.Shared;

public record Currency
{
    public static Currency Dollar = new("USD");
    public static Currency Euro = new("EUR");
    public static Currency None = new(string.Empty);

    private Currency(string? code)
    {
        Code = code;
    }

    public string? Code { get; init; }

    public static readonly IReadOnlyCollection<Currency> SupportedCurrencies = new[] { Dollar, Euro };

    public static Currency From(string? code) =>
        SupportedCurrencies.FirstOrDefault(curr => curr.Code == code) 
        ?? throw new ApplicationException("Currency not supported") ;
}