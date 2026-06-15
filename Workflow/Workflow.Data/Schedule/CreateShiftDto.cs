namespace Data.Schedule;

public class CreateShiftDto(
    int scheduleId,
    int userId,
    DateOnly shiftDate,
    TimeOnly startTime,
    TimeOnly endTime,
    int createdBy)
{
    public int ScheduleId { get; } = scheduleId;
    public int UserId { get; } = userId;
    public DateOnly ShiftDate { get; } = shiftDate;
    public TimeOnly StartTime { get; } = startTime;
    public TimeOnly EndTime { get; } = endTime;
    public int CreatedBy { get; } = createdBy;
}