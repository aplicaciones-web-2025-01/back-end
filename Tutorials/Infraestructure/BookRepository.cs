using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Infraestructure.Persistence.Configuration;
using learning_center_back.Shared.Infraestructure.Persistence.Repositories;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Entities;

namespace learning_center_back.Tutorial.Infraestructure;

public class BookRepository(LearningCenterContext context) : BaseRepository<Book>(context), IBookRepository
{

}