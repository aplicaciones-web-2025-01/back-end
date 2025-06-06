using learning_center_back.Shared.Domain;
using learning_center_back.Tutorials.Domain.Models.Entities;

namespace learning_center_back.Tutorials.Domain;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<Book?> GetByNameAsync(string name);
}