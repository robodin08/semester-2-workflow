#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Data.Schedule;

namespace Workflow.UnitTests.Schedule;

public class MockScheduleRepository : IScheduleRepository
{
    private readonly List<ScheduleDto> _schedules = [];
    private readonly List<ShiftDto> _shifts = [];

    public ScheduleDto CreateSchedule(CreateScheduleDto dto)
    {
        var schedule = new ScheduleDto(
            _schedules.Count + 1,
            dto.WeekNumber,
            dto.Year,
            false,
            dto.CreatedBy,
            DateTime.Now
        );
        _schedules.Add(schedule);
        return schedule;
    }

    public ScheduleDto? GetScheduleById(int id)
        => _schedules.FirstOrDefault(s => s.Id == id);

    public ScheduleDto? GetScheduleByWeek(int weekNumber, int year)
        => _schedules.FirstOrDefault(s => s.WeekNumber == weekNumber && s.Year == year);

    public List<ScheduleDto> GetAllSchedules()
        => [.. _schedules];

    public void PublishSchedule(int id)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == id);
        if (schedule == null) return;
        _schedules.Remove(schedule);
        _schedules.Add(new ScheduleDto(
            schedule.Id, schedule.WeekNumber, schedule.Year,
            true, schedule.CreatedBy, schedule.CreatedAt
        ));
    }

    public void DeleteSchedule(int id)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == id);
        if (schedule != null)
            _schedules.Remove(schedule);
    }

    public ShiftDto CreateShift(CreateShiftDto dto)
    {
        var shift = new ShiftDto(
            _shifts.Count + 1,
            dto.ScheduleId,
            dto.UserId,
            dto.ShiftDate,
            dto.StartTime,
            dto.EndTime,
            dto.CreatedBy,
            DateTime.Now
        );
        _shifts.Add(shift);
        return shift;
    }

    public ShiftDto? GetShift(int id)
        => _shifts.FirstOrDefault(s => s.Id == id);

    public ShiftDto UpdateShift(int id, CreateShiftDto dto)
    {
        var existing = _shifts.FirstOrDefault(s => s.Id == id);
        if (existing == null) throw new InvalidOperationException("Shift not found");

        _shifts.Remove(existing);
        var updated = new ShiftDto(
            id, dto.ScheduleId, dto.UserId,
            dto.ShiftDate, dto.StartTime, dto.EndTime,
            dto.CreatedBy, existing.CreatedAt
        );
        _shifts.Add(updated);
        return updated;
    }

    public void DeleteShift(int id)
    {
        var shift = _shifts.FirstOrDefault(s => s.Id == id);
        if (shift != null)
            _shifts.Remove(shift);
    }

    public List<ShiftDto> GetShiftsByScheduleId(int scheduleId)
        => _shifts.Where(s => s.ScheduleId == scheduleId).ToList();

    public List<ShiftDto> GetShiftsByUserId(int userId)
        => _shifts.Where(s => s.UserId == userId).ToList();

    public List<ShiftDto> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId)
        => _shifts.Where(s => s.UserId == userId && s.ScheduleId == scheduleId).ToList();
}
