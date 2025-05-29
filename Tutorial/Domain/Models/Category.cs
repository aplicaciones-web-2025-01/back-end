using System.ComponentModel.DataAnnotations;

namespace learning_center_back.Security.Domain.Models;

public class Category
{
    public string Name { get; set; }
    public Tutorial Tutorial { get; set; }
    public int TutorialId { get; set; }
    
    
    
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int UserId { get; set; }
    public int UpdatedUserId { get; set; }
    public bool IsActive { get; set; }
}