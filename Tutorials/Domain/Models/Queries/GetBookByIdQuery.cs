namespace learning_center_back.Tutorials.Domain.Models.Commands
{
    public record GetBookByIdQuery
    {
        public GetBookByIdQuery(int bookId)
        {
            BookId = bookId;
        }

        public int BookId { get; init; }
    }
}
