using Domain.Abstractions;

namespace Domain.Rentals;

public static class RentalErrors
{
    public static Error NotFound = new Error(
        "Rent.NotFound",
        "Rental with specified Id was not found");

    public static Error Overlap = new(
        "Rent.Overlap",
        "Rent has been taken for 2 or more customers at the same time");

    public static Error NotReserved = new(
        "Rent.NotReserved",
        "Book is not reserved");

    public static Error NotConfirmed = new(
        "Rent.NotConfirmed",
        "Book is not confirmed");


    public static Error AlreadyStarted = new(
        "Rent.AlreadyStarted",
        "Book has already started");
}