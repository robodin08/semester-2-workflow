using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models;
using Workflow.Core.Authorization;
using Workflow.Core.Schedule;
using Workflow.Core.Users;

namespace Web.Controllers;

[Authorize]
public class ScheduleController(IScheduleService scheduleService, IUserService userService) : Controller
{
    [Authorize]
    public IActionResult MySchedule(int? weekNumber, int? year)
    {
        var now = DateTime.UtcNow;
        var (currentWeek, currentYear) = weekNumber.HasValue
            ? (weekNumber.Value, year ?? now.Year)
            : (GetIsoWeek(now), now.Year);

        var userId = GetCurrentUserId();
        var schedule = scheduleService.GetScheduleByWeek(currentWeek, currentYear);

        var days = new List<ScheduleDayViewModel>();
        var weekStart = DateOnly.FromDateTime(ISOWeek.ToDateTime(currentYear, currentWeek, DayOfWeek.Monday));

        for (var i = 0; i < 7; i++)
        {
            var date = weekStart.AddDays(i);
            var dayShifts = new List<ShiftViewModel>();

            if (schedule != null)
            {
                var shifts = scheduleService.GetShiftsByUserIdAndScheduleId(userId, schedule.Id)
                    .Where(s => s.ShiftDate == date)
                    .OrderBy(s => s.StartTime)
                    .ToList();

                dayShifts = shifts.Select(s => new ShiftViewModel
                {
                    ShiftId = s.Id,
                    UserId = s.UserId,
                    Username = "",
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                }).ToList();
            }

            days.Add(new ScheduleDayViewModel
            {
                DayName = date.ToString("ddd", new CultureInfo("nl-NL")),
                Date = date,
                Shifts = dayShifts,
            });
        }

        var model = new ScheduleWeekViewModel
        {
            WeekNumber = currentWeek,
            Year = currentYear,
            Days = days,
        };

        var (prevWeek, prevYear) = GetPreviousWeek(currentWeek, currentYear);
        var (nextWeek, nextYear) = GetNextWeek(currentWeek, currentYear);
        ViewBag.PreviousWeek = prevWeek;
        ViewBag.PreviousYear = prevYear;
        ViewBag.NextWeek = nextWeek;
        ViewBag.NextYear = nextYear;
        ViewBag.IsManager = User.IsInRole(Role.Manager);

        return View(model);
    }

    [Authorize(Roles = "Manager")]
    public IActionResult Index()
    {
        var schedules = scheduleService.GetAllSchedules();
        var users = userService.GetAllUsers();

        var model = schedules.Select(s =>
        {
            var shiftCount = scheduleService.GetShiftsByScheduleId(s.Id).Count;
            var creator = users.FirstOrDefault(u => u.Id == s.CreatedBy);
            return new ScheduleListViewModel
            {
                Id = s.Id,
                WeekNumber = s.WeekNumber,
                Year = s.Year,
                Published = s.Published,
                CreatedBy = s.CreatedBy,
                CreatedAt = s.CreatedAt,
                ShiftCount = shiftCount,
            };
        }).ToList();

        return View(model);
    }
    
    [Authorize(Roles = "Manager")]
    public IActionResult Create()
    {
        var now = DateTime.UtcNow;
        var (currentWeek, currentYear) = (GetIsoWeek(now), now.Year);

        var model = new CreateScheduleViewModel
        {
            WeekNumber = currentWeek,
            Year = currentYear,
        };

        PopulateEmployeeDropdown();
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateScheduleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            PopulateEmployeeDropdown();
            return View(model);
        }

        var userId = GetCurrentUserId();

        var existing = scheduleService.GetScheduleByWeek(model.WeekNumber, model.Year);
        if (existing != null)
        {
            ModelState.AddModelError("", $"Schedule for week {model.WeekNumber}, {model.Year} already exists.");
            PopulateEmployeeDropdown();
            return View(model);
        }

        var schedule = scheduleService.CreateSchedule(model.WeekNumber, model.Year, userId);

        foreach (var shift in model.Shifts)
        {
            scheduleService.AddShift(schedule.Id, shift.UserId, shift.ShiftDate, shift.StartTime, shift.EndTime, userId);
        }

        TempData["Success"] = $"Schedule for week {model.WeekNumber}, {model.Year} created.";
        return RedirectToAction(nameof(Edit), new { id = schedule.Id });
    }

    [Authorize(Roles = "Manager")]
    public IActionResult Edit(int id)
    {
        var schedule = scheduleService.GetScheduleByWeek(0, 0); // placeholder - need proper method
        var scheduleDto = scheduleService.GetAllSchedules().FirstOrDefault(s => s.Id == id);

        if (scheduleDto == null)
            return NotFound();

        var users = userService.GetAllUsers();
        var shifts = scheduleService.GetShiftsByScheduleId(id);

        var weekStart = DateOnly.FromDateTime(ISOWeek.ToDateTime(scheduleDto.Year, scheduleDto.WeekNumber, DayOfWeek.Monday));

        var model = new CreateScheduleViewModel
        {
            WeekNumber = scheduleDto.WeekNumber,
            Year = scheduleDto.Year,
            Shifts = shifts.Select(s =>
            {
                var user = users.FirstOrDefault(u => u.Id == s.UserId);
                return new ShiftEntryViewModel
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    Username = user?.Username ?? "",
                    ShiftDate = s.ShiftDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                };
            }).ToList(),
        };

        ViewBag.ScheduleId = id;
        ViewBag.Published = scheduleDto.Published;
        ViewBag.WeekStart = weekStart;
        PopulateEmployeeDropdown();
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public IActionResult AddShift(int scheduleId, int userId, DateOnly shiftDate, TimeOnly startTime, TimeOnly endTime)
    {
        var userIdClaim = GetCurrentUserId();
        scheduleService.AddShift(scheduleId, userId, shiftDate, startTime, endTime, userIdClaim);

        TempData["Success"] = "Shift added.";
        return RedirectToAction(nameof(Edit), new { id = scheduleId });
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveShift(int shiftId, int scheduleId)
    {
        scheduleService.RemoveShift(shiftId);

        TempData["Success"] = "Shift removed.";
        return RedirectToAction(nameof(Edit), new { id = scheduleId });
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public IActionResult Publish(int id)
    {
        scheduleService.PublishSchedule(id);

        TempData["Success"] = $"Schedule published.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        scheduleService.DeleteSchedule(id);

        TempData["Success"] = "Schedule deleted.";
        return RedirectToAction(nameof(Index));
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }

    private void PopulateEmployeeDropdown()
    {
        var users = userService.GetAllUsers();
        ViewBag.Employees = users.Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.Username,
        }).ToList();
    }

    private static int GetIsoWeek(DateTime date)
    {
        return ISOWeek.GetWeekOfYear(date);
    }

    private static (int week, int year) GetPreviousWeek(int week, int year)
    {
        if (week > 1) return (week - 1, year);
        return (ISOWeek.GetWeeksInYear(year - 1), year - 1);
    }

    private static (int week, int year) GetNextWeek(int week, int year)
    {
        if (week < ISOWeek.GetWeeksInYear(year)) return (week + 1, year);
        return (1, year + 1);
    }
}
