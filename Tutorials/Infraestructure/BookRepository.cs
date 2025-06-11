using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Infraestructure.Persistence.Repositories;
using learning_center_back.Shared.Infrastructure.Persistence.Configuration;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace learning_center_back.Tutorials.Infraestructure;

public class BookRepository(LearningCenterContext context) : BaseRepository<Book>(context), IBookRepository
{
    public async Task<Book?> GetByNameAsync(string name)
    {
        return await context.Set<Book>().FirstOrDefaultAsync(book => book.Name == name);
    }

    public async Task<IEnumerable<Book>> GetAllWithChaptersAsync()
    {
        return await context.Set<Book>()
            .Include(book => book.Chapters)
            .Include(c => c.Category).ToListAsync();
    }
}