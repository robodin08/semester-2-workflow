#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Users;

namespace Workflow.UnitTests.Users;

public class MockUserRepository : IUserRepository
{
    private readonly List<UserModel> _users = [];

    public bool UserExistsByEmail(string email)
    {
        return _users.Any(u => u.Email == email);
    }

    public bool UserExistsByUsername(string username)
    {
        return _users.Any(u => u.Username == username);
    }

    public UserModel Register(RegisterModel model)
    {
        var user = new UserModel(
            _users.Count + 1,
            model.Email,
            model.Username,
            model.PasswordHash,
            DateTime.Now
        );

        _users.Add(user);

        return user;
    }

    public UserModel? GetUserByEmail(string email)
    {
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public UserModel? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
}