#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Users;

namespace Workflow.UnitTests.Users;

public class MockUserRepository : IUserRepository
{
    private readonly List<UserDto> _users = [];

    public bool UserExistsByEmail(string email)
    {
        return _users.Any(u => u.Email == email);
    }

    public bool UserExistsByUsername(string username)
    {
        return _users.Any(u => u.Username == username);
    }

    public List<UserDto> GetAllUsers()
    {
        return new List<UserDto>(_users);
    }

    public UserDto Register(RegisterDto dto)
    {
        var user = new UserDto(
            _users.Count + 1,
            dto.Email,
            dto.Username,
            dto.PasswordHash,
            "Employee",
            DateTime.Now
        );

        _users.Add(user);

        return user;
    }

    public UserDto? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public UserDto? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public void UpdatePasswordHash(int userId, string newPasswordHash)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return;
        _users.Remove(user);
        _users.Add(new UserDto(user.Id, user.Email, user.Username, newPasswordHash, user.Role, user.CreatedAt));
    }
}