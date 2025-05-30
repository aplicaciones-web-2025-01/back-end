using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Shared.Domain;
using learning_center_back.Tutorial.Infraestructure;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;


namespace learning_center_back.Tutorial.Application.QueryServices;

public class BookQueryService : IBookQueryService
{
    private IBookRepository _bookRepository;

    public BookQueryService(IBookRepository BookRepository)
    {
        _bookRepository = BookRepository;
    }
    public async Task<IEnumerable<Book>> Handler(GetAllBooksQuery query)
    {
      return  await _bookRepository.ListAsync();
    }
}