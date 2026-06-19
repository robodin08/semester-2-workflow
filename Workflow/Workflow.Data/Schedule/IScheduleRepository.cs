namespace Data.Schedule;

public interface IScheduleRepository
{
    ScheduleDto CreateSchedule(CreateScheduleDto dto);
    ScheduleDto? GetScheduleById(int id);
    ScheduleDto? GetScheduleByWeek(int weekNumber, int year);
    List<ScheduleDto> GetAllSchedules();
    void PublishSchedule(int id);
    void DeleteSchedule(int id);

    ShiftDto CreateShift(CreateShiftDto dto);
    ShiftDto? GetShift(int id);
    ShiftDto UpdateShift(int id, CreateShiftDto dto);
    void DeleteShift(int id);
    List<ShiftDto> GetShiftsByScheduleId(int scheduleId);
    List<ShiftDto> GetShiftsByUserId(int userId);
    List<ShiftDto> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId);
}