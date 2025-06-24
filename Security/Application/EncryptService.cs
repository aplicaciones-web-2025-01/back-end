using learning_center_back.Shared.Application.Commands;
using Org.BouncyCastle.Crypto.Generators;

namespace learning_center_back.Security.Application;

public class EncryptService :IEncryptService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword( string password,string passwordHashed)
    {
        return BCrypt.Net.BCrypt.Verify( password, passwordHashed);
    }
}