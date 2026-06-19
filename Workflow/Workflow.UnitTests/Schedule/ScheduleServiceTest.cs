using System;
using Data.Schedule;
using NUnit.Framework;
using Workflow.Core.Schedule;
using Workflow.Core.Schedule.Exceptions;

namespace Workflow.UnitTests.Schedule;

[TestFixture]
[TestOf(typeof(ScheduleService))]
public class ScheduleServiceTest
{
    private IScheduleRepository _scheduleRepository;
    private IScheduleService _scheduleService;

    [SetUp]
    public void Setup()
    {
        _scheduleRepository = new MockScheduleRepository();

        _scheduleService = new ScheduleService(_scheduleRepository);
    }

    [Test]
    public void GetScheduleByWeek_ShouldReturnSchedule_WhenScheduleExists()
    {
        // Arrange
        var created = _scheduleService.CreateSchedule(12, 2026, 1);

        // Act
        var result = _scheduleService.GetScheduleByWeek(12, 2026);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(created.Id));
        Assert.That(result.WeekNumber, Is.EqualTo(12));
        Assert.That(result.Year, Is.EqualTo(2026));
    }

    [Test]
    public void GetScheduleByWeek_ShouldReturnNull_WhenScheduleDoesNotExist()
    {
        // Act
        var result = _scheduleService.GetScheduleByWeek(99, 2026);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void CreateSchedule_ShouldCreateSchedule_WithValidData()
    {
        // Act
        var schedule = _scheduleService.CreateSchedule(10, 2026, 1);

        // Assert
        Assert.That(schedule, Is.Not.Null);
        Assert.That(schedule.WeekNumber, Is.EqualTo(10));
        Assert.That(schedule.Year, Is.EqualTo(2026));
        Assert.That(schedule.CreatedBy, Is.EqualTo(1));
        Assert.That(schedule.Published, Is.False);
    }

    [Test]
    public void PublishSchedule_ShouldMarkScheduleAsPublished()
    {
        // Arrange
        var schedule = _scheduleService.CreateSchedule(5, 2026, 1);

        // Act
        _scheduleService.PublishSchedule(schedule.Id);

        // Assert
        var updated = _scheduleService.GetScheduleByWeek(5, 2026);
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated.Published, Is.True);
    }

    [Test]
    public void DeleteSchedule_ShouldRemoveSchedule()
    {
        // Arrange
        var schedule = _scheduleService.CreateSchedule(8, 2026, 1);

        // Act
        _scheduleService.DeleteSchedule(schedule.Id);

        // Assert
        var result = _scheduleService.GetScheduleByWeek(8, 2026);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void AddShift_ShouldAddShift_WithValidData()
    {
        // Arrange
        var schedule = _scheduleService.CreateSchedule(15, 2026, 1);
        var shiftDate = new DateOnly(2026, 4, 13);
        var startTime = new TimeOnly(9, 0);
        var endTime = new TimeOnly(17, 0);

        // Act
        var shift = _scheduleService.AddShift(schedule.Id, 5, shiftDate, startTime, endTime, 1);

        // Assert
        Assert.That(shift, Is.Not.Null);
        Assert.That(shift.ScheduleId, Is.EqualTo(schedule.Id));
        Assert.That(shift.UserId, Is.EqualTo(5));
        Assert.That(shift.ShiftDate, Is.EqualTo(shiftDate));
        Assert.That(shift.StartTime, Is.EqualTo(startTime));
        Assert.That(shift.EndTime, Is.EqualTo(endTime));
    }

    [Test]
    public void UpdateShift_ShouldUpdateShift_WhenShiftExists()
    {
        // Arrange
        var schedule = _scheduleService.CreateSchedule(25, 2026, 1);
        var shift = _scheduleService.AddShift(schedule.Id, 2,
            new DateOnly(2026, 6, 1), new TimeOnly(8, 0), new TimeOnly(16, 0), 1);

        // Act
        var updated = _scheduleService.UpdateShift(shift.Id, 4,
            new DateOnly(2026, 6, 2), new TimeOnly(10, 0), new TimeOnly(18, 0));

        // Assert
        Assert.That(updated.UserId, Is.EqualTo(4));
        Assert.That(updated.ShiftDate, Is.EqualTo(new DateOnly(2026, 6, 2)));
        Assert.That(updated.StartTime, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(updated.EndTime, Is.EqualTo(new TimeOnly(18, 0)));
    }

    [Test]
    public void UpdateShift_ShouldThrowShiftNotFoundException_WhenShiftDoesNotExist()
    {
        // Act & Assert
        Assert.That(
            () => _scheduleService.UpdateShift(999, 1,
                new DateOnly(2026, 1, 1), new TimeOnly(9, 0), new TimeOnly(17, 0)),
            Throws.TypeOf<ShiftNotFoundException>()
        );
    }
}
