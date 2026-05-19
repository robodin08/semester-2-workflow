namespace Data.Users;

public interface IUserRepository
{
    bool UserExistsByEmail(string email);
    bool UserExistsByUsername(string username);
    
    UserDto Register(RegisterUserDto dto);
    
    UserDto? GetUserByEmail(string email);
    
    UserDto? GetUserById(int id);
}