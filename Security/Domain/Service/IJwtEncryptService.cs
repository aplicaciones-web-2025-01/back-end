using learning_center_back.Security.Domai_.Entities;

namespace learning_center_back.Shared.Application.Commands;

public interface IJwtEncryptService
{
    string Encrypt(User user);
    User Decrypt(string encrypted);
}