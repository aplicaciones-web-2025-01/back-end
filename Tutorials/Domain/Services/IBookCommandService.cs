using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;

namespace learning_center_back.Tutorial.Domain.Services;

public interface IBookCommandService
{
    Task<Book> Handler(CreateBookCommand command);
}