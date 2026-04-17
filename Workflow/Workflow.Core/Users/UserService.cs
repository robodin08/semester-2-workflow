using Data.Users;

namespace Workflow.Core.Users;

public class UserService(IUserRepository repository) : IUserService
{
    public User CreateUser(CreateUserRequest request)
    {
        UserValidator.ValidateCreateUserRequest(request);

        if (UserExists(request.Email, request.Username))
        {
            throw new Exception("A user with this email or username already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var createUserModel = new CreateUserModel(request.Email, request.Username, passwordHash);
        var userModel = repository.CreateUser(createUserModel);

        return User.FromUserModel(userModel);
    }

    public User GetUserByEmail(string email)
    {
        var userModel = repository.GetUserByEmail(email);

        return User.FromUserModel(userModel);
    }

    public User GetUserByUsername(string username)
    {
        var userModel = repository.GetUserByUsername(username);

        return User.FromUserModel(userModel);
    }

    private bool VerifyPassword(UserModel user, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    private bool UserExists(string email, string username)
    {
        try
        {
            repository.GetUserByEmail(email);
            return true;
        }
        catch
        {
            // ignored
        }

        try
        {
            repository.GetUserByUsername(username);
            return true;
        }
        catch
        {
            // ignored
        }

        return false;
    }
}