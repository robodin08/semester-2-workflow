using Workflow.Core.Exceptions;

namespace Workflow.Core.Users.Exceptions;

public class InvalidUsernameException(string message) : UserVisibleException(message);