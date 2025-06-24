namespace learning_center_back.Security.Domain.Exceptions;

public class UsernameAlreadyTakenException : Exception
{
    public UsernameAlreadyTakenException() : base("Username already taken") { }
}
