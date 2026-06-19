using Workflow.Core.Exceptions;

namespace Workflow.Core.Users.Exceptions;

public class UserNotFoundException(string message) : UserVisibleException(message);