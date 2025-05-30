using System.ComponentModel.DataAnnotations;
using learning_center_back.Shared.Domain.Models;

namespace learning_center_back.Security.Domain.Models;

public class Category : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }
    public Tutorial Tutorial { get; set; }
    public int TutorialId { get; set; }
}
    