using Data.Users;
using Workflow.Core.Users.Exceptions;

namespace Workflow.Core.Users;

public class UserService(IUserRepository repository, IPasswordHasher passwordHasher) : IUserService
{
    public User CreateUser(CreateUserRequest request)
    {
        var email = new Email(request.Email);
        var username = new Username(request.Username);
        var password = new Password(request.Password);
        
        if (repository.UserExistsByEmail(email.Value))
            throw new DuplicateUserException("Email already in use");

        if (repository.UserExistsByUsername(username.Value))
            throw new DuplicateUserException("Username already in use");
        
        var createUserDto = new CreateUserDto(email.Value, username.Value, passwordHasher.Hash(password.Value));
        
        var userDto = repository.CreateUser(createUserDto);

        return User.FromUserDto(userDto);
    }
}