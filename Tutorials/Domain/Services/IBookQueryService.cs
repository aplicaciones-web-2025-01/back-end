using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;

namespace learning_center_back.Tutorial.Domain.Services;

public interface IBookQueryService
{
    Task<IEnumerable<Book>> Handle(GetAllBooksQuery query);
    Task<Book> Handle(GetBookByIdQuery query);
}