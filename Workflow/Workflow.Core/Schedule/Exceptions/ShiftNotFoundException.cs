using Workflow.Core.Exceptions;

namespace Workflow.Core.Schedule.Exceptions;

public class ShiftNotFoundException(string message) : UserVisibleException(message);