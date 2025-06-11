using learning_center_back.Tutorials.Domain.Models.Entities;
using learning_center_back.Tutorials.Interfaces.REST.Resources;

namespace learning_center_back.Tutorials.Interfaces.REST.Transform;

public static class BookResourceFromEntityAssembler
{
    public static BookResource ToResourceFromEntity(Book book)
    {
        List<ChapterResource> chapters = new List<ChapterResource>();

        foreach (var bookChapter in book.Chapters)
        {
            chapters.Add(new ChapterResource(bookChapter.Title, bookChapter.Number));
        }

        return new BookResource(book.Id, book.Name, book.Description, book.PublishDate, book.Points, chapters);
    }
}