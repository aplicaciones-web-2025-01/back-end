using learning_center_back.Shared.Domain.Model.Entities;

namespace learning_center_back.Security.Domai_.Entities;

public class User : BaseEntity
{
    public String Username { get; set; }
    public String PasswordHashed { get; set; }

    public String Role { get; set; }
}