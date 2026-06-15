using Data.Users;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IUserService
{
    public UserResponse Register(RegisterRequest request)
    {
        var email = new Email(request.Email);
        var username = new Username(request.Username);
        var password = new Password(request.Password);

        if (userRepository.UserExistsByEmail(email.Value))
            throw new DuplicateUserException("Email already in use");

        if (userRepository.UserExistsByUsername(username.Value))
            throw new DuplicateUserException("Username already in use");

        var passwordHash = passwordHasher.Hash(password.Value);

        var userDto = userRepository.Register(new RegisterDto(email.Value, username.Value, passwordHash));

        var user = User.FromUserDto(userDto);
        return UserResponse.FromUser(user);
    }

    public UserResponse Login(LoginRequest request)
    {
        var email = new Email(request.Email);
        var password = new Password(request.Password);

        var userDto = userRepository.GetUserByEmail(email.Value);
        if (userDto == null || !passwordHasher.Verify(password.Value, userDto.PasswordHash))
            throw new InvalidCredentialException("Invalid email or password");

        var user = User.FromUserDto(userDto);
        return UserResponse.FromUser(user);
    }

    public UserResponse GetUserById(int id)
    {
        var userDto = userRepository.GetUserById(id);
        if (userDto == null)
            throw new UserNotFoundException($"User with id {id} not found");
        
        var user = User.FromUserDto(userDto);
        return UserResponse.FromUser(user);
    }
}