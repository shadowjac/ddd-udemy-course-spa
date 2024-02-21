namespace Domain.Shared;

public record Money(decimal Amount, Currency Currency)
{

    public static Money operator +(Money first, Money other)
    {
        if(first.Currency != other.Currency)
        {
            throw new InvalidOperationException("Cannot add money of different currencies");
        }
        return new Money(first.Amount + other.Amount, first.Currency);
    }
    
    //zero with none currency
    public static Money Zero() => new Money(0, Currency.None);
    
    //zero with currency
    public static Money Zero(Currency currency) => new Money(0, currency);
    
    //isZero function
    public bool IsZero() => this == Zero(Currency);
    
    
}