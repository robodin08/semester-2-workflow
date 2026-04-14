namespace Data.Users;

public interface IUserRepository
{
    UserDto CreateUser(string email, string username, string passwordHash);
}