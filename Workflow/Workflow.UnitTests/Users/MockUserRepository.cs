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

    public UserDto Register(RegisterDto dto)
    {
        var user = new UserDto(
            _users.Count + 1,
            dto.Email,
            dto.Username,
            dto.PasswordHash,
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
}