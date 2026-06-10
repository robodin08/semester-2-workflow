using Workflow.Core.Exceptions;

namespace Workflow.Core.Users.Exceptions;

public class InvalidPasswordException(string message) : UserVisibleException(message);