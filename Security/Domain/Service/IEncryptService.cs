namespace learning_center_back.Shared.Application.Commands;

public interface IEncryptService
{
     string HashPassword(string password);
    
     bool VerifyPassword(string password,string passwordHashed);
}