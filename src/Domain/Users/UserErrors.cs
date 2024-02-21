using Domain.Abstractions;

namespace Domain.Users;

public static class UserErrors
{
    // error not found static
    public static Error NotFound => new(
        "User.NotFound",
        "User not found");

    //invalid credentials
    public static Error InvalidCredentials => new(
        "User.InvalidCredentials",
        "Invalid credentials");
}