using Data.Schedule;
using Workflow.Core.Schedule.Exceptions;

namespace Workflow.Core.Schedule;

public class ScheduleService(IScheduleRepository scheduleRepository) : IScheduleService
{
    public Schedule? GetScheduleByWeek(int weekNumber, int year)
    {
        var dto = scheduleRepository.GetScheduleByWeek(weekNumber, year);
        return dto == null ? null : Schedule.FromScheduleDto(dto);
    }

    public Schedule CreateSchedule(int weekNumber, int year, int createdBy)
    {
        var dto = scheduleRepository.CreateSchedule(new CreateScheduleDto(weekNumber, year, createdBy));
        return Schedule.FromScheduleDto(dto);
    }

    public List<Schedule> GetAllSchedules()
    {
        return scheduleRepository.GetAllSchedules().Select(Schedule.FromScheduleDto).ToList();
    }

    public void PublishSchedule(int id)
    {
        scheduleRepository.PublishSchedule(id);
    }

    public void DeleteSchedule(int id)
    {
        scheduleRepository.DeleteSchedule(id);
    }

    public Shift AddShift(int scheduleId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime,
        int createdBy)
    {
        var dto = scheduleRepository.CreateShift(new CreateShiftDto(scheduleId, userId, shiftDate, startTime, endTime,
            createdBy));
        return Shift.FromShiftDto(dto);
    }

    public void RemoveShift(int shiftId)
    {
        scheduleRepository.DeleteShift(shiftId);
    }

    public Shift UpdateShift(int shiftId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime)
    {
        var existing = scheduleRepository.GetShift(shiftId);
        if (existing == null) throw new ShiftNotFoundException("Shift not found.");

        var dto = scheduleRepository.UpdateShift(shiftId, new CreateShiftDto(
            existing.ScheduleId, userId, shiftDate, startTime, endTime, existing.CreatedBy
        ));
        return Shift.FromShiftDto(dto);
    }

    public List<Shift> GetShiftsByScheduleId(int scheduleId)
    {
        return scheduleRepository.GetShiftsByScheduleId(scheduleId).Select(Shift.FromShiftDto).ToList();
    }

    public List<Shift> GetShiftsByUserId(int userId)
    {
        return scheduleRepository.GetShiftsByUserId(userId).Select(Shift.FromShiftDto).ToList();
    }

    public List<Shift> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId)
    {
        return scheduleRepository.GetShiftsByUserIdAndScheduleId(userId, scheduleId).Select(Shift.FromShiftDto)
            .ToList();
    }
}