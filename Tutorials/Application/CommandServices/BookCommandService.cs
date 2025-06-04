using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain.Models.Entities;

namespace learning_center_back.Tutorial.Application.CommandServices;

public class BookCommandService : IBookCommandService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookCommandService(IBookRepository bookRepository,IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Book> Handler(CreateBookCommand command)
    {
        var book = new Book(command.Name,command.Description,command.PublishDate,command.Points);

        await _bookRepository.AddAsync(book);

        await _unitOfWork.CompleteAsync();
        
        return book;
    }
}