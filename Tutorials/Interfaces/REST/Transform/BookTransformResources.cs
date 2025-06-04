using learning_center_back.Tutorials.Domain.Models.Entities;
using learning_center_back.Tutorials.Interfaces.REST.Resources;

namespace learning_center_back.Tutorials.Interfaces.REST.Transform;

public static class BookTransformResources
{
    public static BookResource ToEntities(Book book)
    {
        return new BookResource(book.Id, book.Name, book.Description, book.PublishDate, book.Points);
    }
}