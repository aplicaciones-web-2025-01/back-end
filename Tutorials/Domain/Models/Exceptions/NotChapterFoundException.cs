namespace learning_center_back.Tutorials.Domain.Models.Exceptions;

public class NotChapterFoundException : Exception
{
    public NotChapterFoundException() : base("Not chapters found")
    {
    }

    public NotChapterFoundException(string message)
        : base(message)
    {
    }

    public NotChapterFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
