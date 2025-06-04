namespace learning_center_back.Shared.Domain.Models.Commands;

public record DeleteBookCommand(int Id); //primary constructor

/*
public record DeleteBookCommand
{
    public DeleteBookCommand(int Id)
    {
        Id = Id;
    }
    public int Id { get; init; }
}
*/