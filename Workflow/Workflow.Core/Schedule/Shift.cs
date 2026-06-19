using Data.Schedule;

namespace Workflow.Core.Schedule;

public class Shift(
    int id,
    int scheduleId,
    int userId,
    DateOnly shiftDate,
    TimeOnly startTime,
    TimeOnly endTime,
    int createdBy,
    DateTime createdAt)
{
    public int Id { get; } = id;
    public int ScheduleId { get; } = scheduleId;
    public int UserId { get; } = userId;
    public DateOnly ShiftDate { get; } = shiftDate;
    public TimeOnly StartTime { get; } = startTime;
    public TimeOnly EndTime { get; } = endTime;
    public int CreatedBy { get; } = createdBy;
    public DateTime CreatedAt { get; } = createdAt;

    public static Shift FromShiftDto(ShiftDto dto)
    {
        return new Shift(dto.Id, dto.ScheduleId, dto.UserId, dto.ShiftDate, dto.StartTime, dto.EndTime, dto.CreatedBy,
            dto.CreatedAt);
    }
}