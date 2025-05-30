using System.ComponentModel.DataAnnotations;
using learning_center_back.Shared.Domain.Model.Entities;

namespace learning_center_back.Tutorials.Domain.Models.Entities;
    

public class Category : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }
    public Book Book{ get; set; }
    public int TutorialId { get; set; }
}
    