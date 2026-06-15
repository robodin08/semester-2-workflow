namespace Data.Schedule;

public interface IScheduleRepository
{
    ShiftDto CreateShift(CreateShiftDto schedule);
    ShiftDto? GetShift(int id);
}