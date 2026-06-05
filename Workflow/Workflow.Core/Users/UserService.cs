using Data.Users;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IUserService
{
    public User Register(RegisterRequest request)
    {
        var email = new Email(request.Email);
        var username = new Username(request.Username);
        var password = new Password(request.Password);

        if (userRepository.UserExistsByEmail(email.Value))
            throw new DuplicateUserException("Email already in use");

        if (userRepository.UserExistsByUsername(username.Value))
            throw new DuplicateUserException("Username already in use");

        var passwordHash = passwordHasher.Hash(password.Value);

        var userDto = userRepository.Register(new RegisterModel(email.Value, username.Value, passwordHash));

        return User.FromUserDto(userDto);
    }

    public User Login(LoginRequest request)
    {
        var email = new Email(request.Email);

        var userDto = userRepository.GetUserByEmail(email.Value);
        if (userDto == null || !passwordHasher.Verify(request.Password, userDto.PasswordHash))
            throw new InvalidCredentialException("Invalid email or password");

        return User.FromUserDto(userDto);
    }

    public User? GetUserById(int id)
    {
        var userDto = userRepository.GetUserById(id);
        return userDto == null ? null : User.FromUserDto(userDto);
    }
}