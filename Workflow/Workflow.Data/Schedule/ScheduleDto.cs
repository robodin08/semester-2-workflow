namespace Data.Schedule;

public class ScheduleDto(
    int id,
    int weekNumber,
    int year,
    bool published,
    int createdBy,
    DateTime createdAt)
{
    public int Id { get; } = id;
    public int WeekNumber { get; } = weekNumber;
    public int Year { get; } = year;
    public bool Published { get; } = published;
    public int CreatedBy { get; } = createdBy;
    public DateTime CreatedAt { get; } = createdAt;
}
