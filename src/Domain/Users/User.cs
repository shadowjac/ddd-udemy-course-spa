using Domain.Abstractions;
using Domain.Users.Events;

namespace Domain.Users;

public sealed class User : Entity
{
    private User()
    {
    }

    private User(Guid id, Name? name, LastName? lastName, Email? email) : base(id)
    {
        Name = name;
        LastName = lastName;
        Email = email;
    }

    public static User CreateInstance(Name? name, LastName? lastName, Email? email)
    {
        var user = new User(Guid.NewGuid(), name, lastName, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public Name? Name { get; private set; }
    public LastName? LastName { get; private set; }
    public Email? Email { get; private set; }
}