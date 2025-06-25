using learning_center_back.Security.Domai_.Entities;
using learning_center_back.Shared.Application.Commands.Repositories;
using learning_center_back.Shared.Infraestructure.Persistence.Repositories;
using learning_center_back.Shared.Infrastructure.Persistence.Configuration;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace learning_center_back.Security.Infraestrucutre;

public class UserRepository(LearningCenterContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByUsernamelAsync(string username)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
    }
}