namespace learning_center_back.Tutorials.Domain.Models.Commands;

public record UpdateBookCommand(int Id, String Name, String Description, DateTime PublishDate, int Points);
