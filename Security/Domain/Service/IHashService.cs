namespace learning_center_back.Shared.Application.Commands;

public interface IHashService
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHashed);
}