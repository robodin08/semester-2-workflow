using Data.Schedule;

namespace Workflow.Core.Schedule;

public class ScheduleService(IScheduleRepository scheduleRepository) : IScheduleService
{
    public ScheduleDto? GetScheduleByWeek(int weekNumber, int year)
    {
        return scheduleRepository.GetScheduleByWeek(weekNumber, year);
    }

    public ScheduleDto CreateSchedule(int weekNumber, int year, int createdBy)
    {
        return scheduleRepository.CreateSchedule(new CreateScheduleDto(weekNumber, year, createdBy));
    }

    public List<ScheduleDto> GetAllSchedules()
    {
        return scheduleRepository.GetAllSchedules();
    }

    public void PublishSchedule(int id)
    {
        scheduleRepository.PublishSchedule(id);
    }

    public void DeleteSchedule(int id)
    {
        scheduleRepository.DeleteSchedule(id);
    }

    public ShiftDto AddShift(int scheduleId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime, int createdBy)
    {
        return scheduleRepository.CreateShift(new CreateShiftDto(scheduleId, userId, shiftDate, startTime, endTime, createdBy));
    }

    public void RemoveShift(int shiftId)
    {
        scheduleRepository.DeleteShift(shiftId);
    }

    public ShiftDto UpdateShift(int shiftId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime)
    {
        var existing = scheduleRepository.GetShift(shiftId);
        if (existing == null) throw new InvalidOperationException("Shift not found.");

        return scheduleRepository.UpdateShift(shiftId, new CreateShiftDto(
            existing.ScheduleId, userId, shiftDate, startTime, endTime, existing.CreatedBy
        ));
    }

    public List<ShiftDto> GetShiftsByScheduleId(int scheduleId)
    {
        return scheduleRepository.GetShiftsByScheduleId(scheduleId);
    }

    public List<ShiftDto> GetShiftsByUserId(int userId)
    {
        return scheduleRepository.GetShiftsByUserId(userId);
    }

    public List<ShiftDto> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId)
    {
        return scheduleRepository.GetShiftsByUserIdAndScheduleId(userId, scheduleId);
    }
}