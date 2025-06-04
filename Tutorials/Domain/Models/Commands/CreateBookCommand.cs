namespace learning_center_back.Shared.Domain.Models.Commands;

public record CreateBookCommand
{
    public CreateBookCommand(string name, string description, DateTime publishDate,int points)
    {
        Name  = name;
        Description = description;
        PublishDate = publishDate;
        Points = points;
    }
    
    
    
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public int Points { get; set; }
    
};