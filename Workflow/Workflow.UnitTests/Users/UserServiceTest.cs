using Data.Users;
using NUnit.Framework;
using Workflow.Core.Users;

namespace Workflow.UnitTests.Users;

[TestFixture]
[TestOf(typeof(UserService))]
public class UserServiceTest
{
    private IUserRepository _userRepository;
    private IPasswordHasher _passwordHasher;
    private IUserService _userService;
    
    [SetUp]
    public void Setup()
    {
        _userRepository = new MockUserRepository();
        _passwordHasher = new MockPasswordHasher();
        
        _userService = new UserService(_userRepository, _passwordHasher);
    }

    [Test]
    public void CreateUser_ShouldCreateUser_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterRequest(
            "test@example.com",
            "user",
            "password123"
        );

        // Act
        var user = _userService.Register(request);
        
        // Assert
        Assert.That(user.Email, Is.EqualTo(request.Email));
        Assert.That(user.Username, Is.EqualTo(request.Username));
        // Assert.That(user.PasswordHash, Is.EqualTo("hashed_password123"));
    }
}