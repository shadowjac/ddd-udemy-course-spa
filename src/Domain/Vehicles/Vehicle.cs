using Domain.Abstractions;
using Domain.Shared;

namespace Domain.Vehicles;

public sealed class Vehicle : Entity
{
    private Vehicle()
    {
    }

    private Vehicle(Guid id,
        Model? model,
        Vin? vin,
        Address? address,
        Money price,
        Money maintenanceCost,
        IEnumerable<Accessories> accessories,
        DateTime? lastRentalDate) : base(id)
    {
        Model = model;
        Vin = vin;
        Address = address;
        Price = price;
        MaintenanceCost = maintenanceCost;
        LastRentalDate = lastRentalDate;
        Accessories = accessories;
    }

    public Model? Model { get; private set; }
    public Vin? Vin { get; private set; }
    public Address? Address { get; private set; }
    public Money Price { get; private set; }
    public Money MaintenanceCost { get; private set; }
    public DateTime? LastRentalDate { get; internal set; }
    public IEnumerable<Accessories> Accessories { get; private set; } = new List<Accessories>();


    public static Vehicle CreateInstance(Guid id, Model? model, Vin? vin, Address? address, Money price,
        Money maintenanceCost, IEnumerable<Accessories> accessories, DateTime? lastRentalDate)
    {
        return new Vehicle(id, model, vin, address, price, maintenanceCost, accessories, lastRentalDate);
    }
}