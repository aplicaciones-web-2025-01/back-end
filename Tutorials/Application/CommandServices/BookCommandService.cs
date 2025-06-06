using System.Data;
using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;
using learning_center_back.Tutorials.Domain.Models.Exceptions;

namespace learning_center_back.Tutorials.Application.CommandServices
{
    public class BookCommandService(IBookRepository bookRepository, IUnitOfWork unitOfWork) : IBookCommandService
    {
        private readonly IBookRepository _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<Book> Handle(CreateBookCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            /*var books = await _bookRepository.ListAsync(); // get all rows  SELECT* FROM BOOKS
            if (books.Any(book => book.Name == command.Name))
                throw new DuplicateNameException($"A book with the name '{command.Name}' already exists.");*/
            

                //!command.Chapters.Any()  command.Chapters.Count >0
            
            var existingBook = await _bookRepository.GetByNameAsync(command.Name);//  SELECT* FROM BOOKS WHERE name = {command.Name}}
            if(existingBook != null) throw new DuplicateNameException($"A book with the name '{command.Name}' already exists.");
            
            if (command.Chapters == null || !command.Chapters.Any())
                throw new NotChapterFoundException();
         
            
            var book = new Book(command.Name, command.Description, command.PublishDate, command.Points)
            {
                UserId = 1
            };
            
            command.Chapters.ForEach(chapter =>
            {
                book.Chapters.Add(new Chapter(chapter.Title, chapter.Number, chapter.Content, book));
            });
            
            //cfg.CreateMap<Chapters, ChaptersCommand>()
            // public Chapter(string title, int number, string content, Book book)
            
            
            
            
            await _bookRepository.AddAsync(book);
            await _unitOfWork.CompleteAsync();

            return book;
        }

        public async Task<bool> Handle(DeleteBookCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var book = await _bookRepository.FindByIdAsync(command.Id);
            if (book is null) return false;

            book.IsActive = false;
            book.ModifiedDate = DateTime.UtcNow;
            book.UpdatedUserId = 87; // Placeholder for dynamic user ID.

            _bookRepository.Update(book);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}