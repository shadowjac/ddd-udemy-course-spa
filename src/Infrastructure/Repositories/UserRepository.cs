using Domain.Users;
using Domain.Users.Repositories;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext context) : Repository<User>(context), IUserRepository;