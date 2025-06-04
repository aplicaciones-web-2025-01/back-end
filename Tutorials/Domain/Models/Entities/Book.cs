using learning_center_back.Shared.Domain.Model.Entities;

namespace learning_center_back.Tutorials.Domain.Models.Entities
{
    public class Book : BaseEntity
    {

        public Book(String name, String description, DateTime publishedDate, int points)
        {
             Name = name;
             Description = description;
             PublishDate = publishedDate;
             Points = points;
        }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public int Points { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
