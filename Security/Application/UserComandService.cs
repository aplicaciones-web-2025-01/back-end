using learning_center_back.Security.Domai_.Comands;
using learning_center_back.Security.Domai_.Entities;
using learning_center_back.Security.Domain.Exceptions;
using learning_center_back.Shared.Application.Commands;
using learning_center_back.Shared.Application.Commands.Repositories;
using learning_center_back.Shared.Domain;

namespace learning_center_back.Security.Application;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEncryptService _encryptService;

    public UserCommandService(IUserRepository userRepository, IUnitOfWork unitOfWork, IEncryptService encryptService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _encryptService = encryptService;
    }

    public async Task<User> Handle(SignUpCommand command)
    {
        var existingUser = await _userRepository.GetByUsernamelAsync(command.Username);
        if (existingUser != null)
            throw new UsernameAlreadyTakenException();

        var user = new User
        {
            Username = command.Username,
            PasswordHashed = _encryptService.HashPassword(command.Password),
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<User> Handle(LoginCommand command)
    {
        var user = await _userRepository.GetByUsernamelAsync(command.Username);
        if (user == null || !_encryptService.VerifyPassword(command.Password, user.PasswordHashed))
            throw new InvalidCredentialsException();

        return user;
    }

}