using Diploma.Domain.Models;

namespace Diploma.Domain.Repositories.Users;

public sealed class UsersRepository : RepositoryBase<UserInfo>, IUsersRepository
{
    public UsersRepository(DiplomaContext context) : base(context) { }
}