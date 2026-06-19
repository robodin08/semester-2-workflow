namespace Data.Schedule;

public class CreateScheduleDto(
    int weekNumber,
    int year,
    int createdBy)
{
    public int WeekNumber { get; } = weekNumber;
    public int Year { get; } = year;
    public int CreatedBy { get; } = createdBy;
}
