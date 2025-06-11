using System.Data;
using FluentValidation;
using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Domain.Models.Commands;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;
using learning_center_back.Tutorials.Domain.Models.Exceptions;
using learning_center_back.Tutorials.Domain.Models.Validadors;
using NuGet.Packaging.Licenses;

namespace learning_center_back.Tutorials.Application.CommandServices
{
    public class BookCommandService(
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateBookCommand> validator) : IBookCommandService
    {
        private readonly IBookRepository _bookRepository =
            bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        private readonly IValidator<CreateBookCommand> _validator =
            validator ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Book> Handle(CreateBookCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException(string.Join(", ", errors));
            }

            //ValidateBook(command);
            /*var books = await _bookRepository.ListAsync(); // get all rows  SELECT* FROM BOOKS
            if (books.Any(book => book.Name == command.Name))
                throw new DuplicateNameException($"A book with the name '{command.Name}' already exists.");*/

            //!command.Chapters.Any()  command.Chapters.Count > 0

            var existingBook =
                await _bookRepository.GetByNameAsync(command.Name); //  SELECT* FROM BOOKS WHERE name = {command.Name}}
            if (existingBook != null)
                throw new DuplicateNameException($"A book with the name '{command.Name}' already exists.");

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

        private void ValidateBook(CreateBookCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrEmpty(command.Name))
                throw new ArgumentException("Book name is required");

            if (command.Name.Length > 20)
                throw new ArgumentException("Book name MAX LENGHT 20  CHARACTERS");

            if (command.Description.Length > 100)
                throw new ArgumentException("Book description MAX LENGHT 100  CHARACTERS");

            if (command.Description.Length < 10)
                throw new ArgumentException("Book description Min LENGHT 10  CHARACTERS");
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

        public async Task<bool> Handle(UpdateBookCommand command, int Id)
        {
            var book = await _bookRepository.FindByIdAsync(Id);
            if (book is null) throw new DataException("Book not found.");


            book.Name = command.Name;
            book.Description = command.Description;
            book.Points = command.Points;
            book.ModifiedDate = DateTime.UtcNow;
            book.UpdatedUserId = 87;

            _bookRepository.Update(book);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}