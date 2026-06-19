using Data.Schedule;

namespace Workflow.Core.Schedule;

public class Schedule(int id, int weekNumber, int year, bool published, int createdBy, DateTime createdAt)
{
    public int Id { get; } = id;
    public int WeekNumber { get; } = weekNumber;
    public int Year { get; } = year;
    public bool Published { get; } = published;
    public int CreatedBy { get; } = createdBy;
    public DateTime CreatedAt { get; } = createdAt;

    public static Schedule FromScheduleDto(ScheduleDto dto)
    {
        return new Schedule(dto.Id, dto.WeekNumber, dto.Year, dto.Published, dto.CreatedBy, dto.CreatedAt);
    }
}
