namespace learning_center_back.Security.Domain.Models;

public class Tutorial
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public int Points { get; set; }
    public List<Category> Categories { get; set; } = new List<Category>();
    
    
    
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int UserId { get; set; }
    public int UpdatedUserId { get; set; }
    public bool IsActive { get; set; }
    
}