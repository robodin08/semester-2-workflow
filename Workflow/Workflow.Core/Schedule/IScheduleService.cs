namespace Workflow.Core.Schedule;

public interface IScheduleService
{
    Schedule? GetScheduleByWeek(int weekNumber, int year);
    Schedule CreateSchedule(int weekNumber, int year, int createdBy);
    List<Schedule> GetAllSchedules();
    void PublishSchedule(int id);
    void DeleteSchedule(int id);

    Shift AddShift(int scheduleId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime, int createdBy);
    void RemoveShift(int shiftId);
    Shift UpdateShift(int shiftId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime);
    List<Shift> GetShiftsByScheduleId(int scheduleId);
    List<Shift> GetShiftsByUserId(int userId);
    List<Shift> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId);
}