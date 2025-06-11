namespace learning_center_back.Tutorials.Interfaces.REST.Resources;

public record BookResource(int Id, string Name, string Description, DateTime PublishDate, int Points, List<ChapterResource> Chapters)
{
}

/*    public BookResource(int id, string name, string description, DateTime publicationDate, int points)
{
    Id = id;
    Name = name;
    Description = description;
    PublishDate = publicationDate;
    Points = points;
}

public int Id { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public DateTime PublishDate { get; set; }
public int Points { get; set; }
*/