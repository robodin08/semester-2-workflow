using Data.Schedule;

namespace Workflow.Core.Schedule;

public interface IScheduleService
{
    ScheduleDto? GetScheduleByWeek(int weekNumber, int year);
    ScheduleDto CreateSchedule(int weekNumber, int year, int createdBy);
    List<ScheduleDto> GetAllSchedules();
    void PublishSchedule(int id);
    void DeleteSchedule(int id);

    ShiftDto AddShift(int scheduleId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime, int createdBy);
    void RemoveShift(int shiftId);
    ShiftDto UpdateShift(int shiftId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime);
    List<ShiftDto> GetShiftsByScheduleId(int scheduleId);
    List<ShiftDto> GetShiftsByUserId(int userId);
    List<ShiftDto> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId);
}