using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Infraestructure.Persistence.Configuration;

namespace learning_center_back.Shared.Infraestructure.Persistence.Repositories;

public class UnitOfWork(LearningCenterContext context) : IUnitOfWork
{
    /// <inheritdoc />
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}