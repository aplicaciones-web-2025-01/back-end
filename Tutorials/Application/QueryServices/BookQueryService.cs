using learning_center_back.Shared.Domain;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Domain.Models.Commands;
using learning_center_back.Tutorials.Domain.Models.Entities;

namespace learning_center_back.Tutorials.Application.QueryServices
{
    public class BookQueryService : IBookQueryService
    {
        private readonly IBookRepository _bookRepository;

        public BookQueryService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooksQuery query)
        {
            //var books = await _bookRepository.ListAsync();
            var books = await _bookRepository.GetAllWithChaptersAsync();
            return books?.Where(book => book.IsActive) ?? Enumerable.Empty<Book>();
        }

        public async Task<Book?> Handle(GetBookByIdQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var book = await _bookRepository.FindByIdAsync(query.BookId);
            return book?.IsActive == true ? book : null;
        }
    }
}