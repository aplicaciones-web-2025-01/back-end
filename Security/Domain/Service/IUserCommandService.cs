using learning_center_back.Security.Domai_.Comands;
using learning_center_back.Security.Domai_.Entities;

namespace learning_center_back.Shared.Application.Commands;

public interface IUserCommandService
{
    Task<User> Handle(SignUpCommand command );
    Task<User> Handle(LoginCommand loginCommand);
}