using Workflow.Core.Exceptions;

namespace Workflow.Core.Users.Exceptions;

public class DuplicateUserException(string message) : UserVisibleException(message);