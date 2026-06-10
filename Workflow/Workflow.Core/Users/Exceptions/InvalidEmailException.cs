using Workflow.Core.Exceptions;

namespace Workflow.Core.Users.Exceptions;

public class InvalidEmailException(string message) : UserVisibleException(message);