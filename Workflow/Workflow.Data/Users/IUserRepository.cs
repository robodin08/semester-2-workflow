namespace Data.Users;

public interface IUserRepository
{
    bool UserExistsByEmail(string email);
    bool UserExistsByUsername(string username);
    
    UserDto CreateUser(CreateUserDto dto);
    
    UserDto GetUserByEmail(string email);
}