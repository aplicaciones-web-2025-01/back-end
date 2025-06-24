using learning_center_back.Security.Domai_.Entities;
using learning_center_back.Shared.Domain;

namespace learning_center_back.Shared.Application.Commands.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByUsernamelAsync(string usernamel);
}