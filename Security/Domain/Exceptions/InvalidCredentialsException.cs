namespace learning_center_back.Security.Domain.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Invalid username or password") { }
}
 