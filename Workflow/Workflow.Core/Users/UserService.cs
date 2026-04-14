using Data.Users;

namespace Workflow.Core.Users;

public class UserService(IUserRepository repository) : IUserService
{
    public User CreateUser(CreateUserRequest request)
    {
        request.Validate();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var userDto = repository.CreateUser(request.Email, request.Username, passwordHash);
        return User.FromUserDto(userDto);
    }
    
     public bool VerifyPassword(UserDto user, string password)
     {
         return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
     }
}