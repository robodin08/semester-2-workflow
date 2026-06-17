using Data.Users;
using NUnit.Framework;
using Workflow.Core.Users;
using Workflow.Core.Users.Exceptions;

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
        
        var userDto = _userRepository.GetUserById(user.Id);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }

    [Test]
    public void Register_ShouldThrowDuplicateUserException_WhenEmailAlreadyExists()
    {
        // Arrange
        var email = new Email("existing@example.com");
        var username = new Username("user1");
        var password = new Password("password123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        var registeredUser = _userService.Register(request);

        var duplicateRequest = new RegisterRequest(
            email.Value, "user2", "password456"
        );

        // Act & Assert
        Assert.That(
            () => _userService.Register(duplicateRequest),
            Throws.TypeOf<DuplicateUserException>()
        );

        var userDto = _userRepository.GetUserByEmail(email.Value);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }

    [Test]
    public void Register_ShouldThrowDuplicateUserException_WhenUsernameAlreadyExists()
    {
        // Arrange
        var email = new Email("user@example.com");
        var username = new Username("taken");
        var password = new Password("password123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        var registeredUser = _userService.Register(request);

        var duplicateRequest = new RegisterRequest(
            "other@example.com", username.Value, "password456"
        );

        // Act & Assert
        Assert.That(
            () => _userService.Register(duplicateRequest),
            Throws.TypeOf<DuplicateUserException>()
        );

        var userDto = _userRepository.GetUserByEmail(email.Value);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }

    [Test]
    public void Login_ShouldReturnUserResponse_WhenCredentialsAreValid()
    {
        // Arrange
        var email = new Email("test@example.com");
        var username = new Username("test");
        var password = new Password("password123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        _userService.Register(request);

        var loginRequest = new LoginRequest(
            email.Value, password.Value
        );

        // Act
        var user = _userService.Login(loginRequest);
        
        // Assert
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Email, Is.EqualTo(email.Value));
        Assert.That(user.Username, Is.EqualTo(username.Value));

        var userDto = _userRepository.GetUserByEmail(email.Value);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }

    [Test]
    public void Login_ShouldThrowInvalidCredentialException_WhenEmailDoesNotExist()
    {
        // Arrange
        var loginRequest = new LoginRequest(
            "unknown@example.com", "password123"
        );

        // Act & Assert
        Assert.That(
            () => _userService.Login(loginRequest),
            Throws.TypeOf<InvalidCredentialException>()
        );

        var userDto = _userRepository.GetUserByEmail("unknown@example.com");
        Assert.That(userDto, Is.Null);
    }

    [Test]
    public void Login_ShouldThrowInvalidCredentialException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var email = new Email("test@example.com");
        var username = new Username("test");
        var password = new Password("password123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        _userService.Register(request);

        var loginRequest = new LoginRequest(
            email.Value, "wrongpassword"
        );

        // Act & Assert
        Assert.That(
            () => _userService.Login(loginRequest),
            Throws.TypeOf<InvalidCredentialException>()
        );

        var userDto = _userRepository.GetUserByEmail(email.Value);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }

    [Test]
    public void GetUserById_ShouldReturnUserResponse_WhenUserExists()
    {
        // Arrange
        var email = new Email("test@example.com");
        var username = new Username("test");
        var password = new Password("password123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        var registeredUser = _userService.Register(request);

        // Act
        var user = _userService.GetUserById(registeredUser.Id);
        
        // Assert
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Id, Is.EqualTo(registeredUser.Id));
        Assert.That(user.Email, Is.EqualTo(email.Value));
        Assert.That(user.Username, Is.EqualTo(username.Value));

        var userDto = _userRepository.GetUserById(user.Id);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }

    [Test]
    public void GetUserById_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Act & Assert
        Assert.That(
            () => _userService.GetUserById(999),
            Throws.TypeOf<UserNotFoundException>()
        );

        Assert.That(_userRepository.GetUserById(999), Is.Null);
    }

    [Test]
    public void ChangePassword_ShouldUpdatePassword_WhenCurrentPasswordIsCorrect()
    {
        // Arrange
        var email = new Email("test@example.com");
        var username = new Username("test");
        var password = new Password("oldPassword123");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        var user = _userService.Register(request);

        // Act
        _userService.ChangePassword(user.Id, "oldPassword123", "newPassword456");

        // Assert
        var userDto = _userRepository.GetUserById(user.Id);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(_passwordHasher.Verify("newPassword456", userDto.PasswordHash), Is.True);
        Assert.That(_passwordHasher.Verify("oldPassword123", userDto.PasswordHash), Is.False);
    }

    [Test]
    public void ChangePassword_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        // Act & Assert
        Assert.That(
            () => _userService.ChangePassword(999, "current", "new"),
            Throws.TypeOf<UserNotFoundException>()
        );

        Assert.That(_userRepository.GetUserById(999), Is.Null);
    }

    [Test]
    public void ChangePassword_ShouldThrowInvalidCredentialException_WhenCurrentPasswordIsIncorrect()
    {
        // Arrange
        var email = new Email("test@example.com");
        var username = new Username("test");
        var password = new Password("correctPassword");
        
        var request = new RegisterRequest(
            email.Value, username.Value, password.Value
        );
        var user = _userService.Register(request);

        // Act & Assert
        Assert.That(
            () => _userService.ChangePassword(user.Id, "wrongPassword", "newPassword"),
            Throws.TypeOf<InvalidCredentialException>()
        );

        var userDto = _userRepository.GetUserById(user.Id);
        Assert.That(userDto, Is.Not.Null);
        Assert.That(userDto.Email, Is.EqualTo(email.Value));
        Assert.That(userDto.Username, Is.EqualTo(username.Value));
        Assert.That(_passwordHasher.Verify(password.Value, userDto.PasswordHash), Is.True);
    }
}