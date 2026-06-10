using Workflow.Core.Exceptions;

namespace Workflow.Core.Users.Exceptions;

public class InvalidUserException(string message) : UserVisibleException(message);