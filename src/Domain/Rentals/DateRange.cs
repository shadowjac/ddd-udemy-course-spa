namespace Domain.Rentals;

public record DateRange
{
    protected DateRange(DateOnly startDate, DateOnly endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }

    public int NumberOfDays => EndDate.Day - StartDate.Day;

    public static DateRange Create(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date must be before end date");
        }

        var dateRange = new DateRange(startDate, endDate);
        return dateRange;
    }
}