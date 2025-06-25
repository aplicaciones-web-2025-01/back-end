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
    private readonly IHashService _hashService;
    private readonly IJwtEncryptService _jwtEncryptService;

    public UserCommandService(IUserRepository userRepository, IUnitOfWork unitOfWork, IHashService hashService,IJwtEncryptService jwtEncryptService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtEncryptService = jwtEncryptService;
    }

    public async Task<User> Handle(SignUpCommand command)
    {
        var existingUser = await _userRepository.GetByUsernamelAsync(command.Username);
        if (existingUser != null)
            throw new UsernameAlreadyTakenException();

        var user = new User
        {
            Username = command.Username,
            PasswordHashed = _hashService.HashPassword(command.Password),
            Role = command.Role
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<string> Handle(LoginCommand command)
    {
        var user = await _userRepository.GetByUsernamelAsync(command.Username);
        if (user == null || !_hashService.VerifyPassword(command.Password, user.PasswordHashed))
            throw new InvalidCredentialsException();
        
        //coinciden

         var jwtToken=  _jwtEncryptService.Encrypt(user);
        

        return jwtToken;
    }

}