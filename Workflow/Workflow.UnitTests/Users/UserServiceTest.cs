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
        var email = new Email("test@example.com");
        var username = new Username("test");
        var password = new Password("password123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );

        // Act
        var user = _userService.Register(request);
        
        // Assert
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Email, Is.EqualTo(email.Value));
        Assert.That(user.Username, Is.EqualTo(username.Value));
        // Assert.That(_passwordHasher.Verify(password.Value, user.PasswordHash), Is.True);
        
        var userDto = _userRepository.GetUserById(user.Id);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }
}