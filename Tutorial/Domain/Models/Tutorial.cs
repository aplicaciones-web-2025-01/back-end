using learning_center_back.Shared.Domain.Models;

namespace learning_center_back.Security.Domain.Models;

public class Tutorial: BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public int Points { get; set; }
    public List<Category> Categories { get; set; } = new List<Category>();
    
    
    
}