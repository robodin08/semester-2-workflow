using MySql.Data.MySqlClient;

namespace Data.Schedule;

public class ScheduleRepository(IDbConnectionFactory factory) : IScheduleRepository
{
    public ShiftDto CreateShift(CreateShiftDto schedule)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                             INSERT INTO schedule (schedule_id, user_id, shift_date, start_time, end_time, created_by)
                                                             VALUES (@schedule_id, @user_id, @shift_date, @start_time, @end_time, @created_by);
                                                             SELECT id, schedule_id, user_id, shift_date, start_time, end_time, created_by, created_at
                                                             FROM schedule
                                                             WHERE id = LAST_INSERT_ID();
                                             """, connection);

        command.Parameters.AddWithValue("@schedule_id", schedule.ScheduleId);
        command.Parameters.AddWithValue("@user_id", schedule.UserId);
        command.Parameters.AddWithValue("@shift_date", schedule.ShiftDate);
        command.Parameters.AddWithValue("@start_time", schedule.StartTime);
        command.Parameters.AddWithValue("@end_time", schedule.EndTime);
        command.Parameters.AddWithValue("@created_by", schedule.CreatedBy);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to create user.");

        var id = reader.GetInt32("id");
        var scheduleId = reader.GetInt32("schedule_id");
        var userId = reader.GetInt32("user_id");

        var shiftDate = DateOnly.FromDateTime(reader.GetDateTime("shift_date"));

        var startTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan("start_time"));
        var endTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan("end_time"));

        var createdBy = reader.GetInt32("created_by");
        var createdAt = reader.GetDateTime("created_at");

        return new ShiftDto(
            id,
            scheduleId,
            userId,
            shiftDate,
            startTime,
            endTime,
            createdBy,
            createdAt
        );
    }

    public ShiftDto? GetShift(int shiftId)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                             SELECT id, schedule_id, user_id, shift_date, start_time, end_time, created_by, created_at
                                                             FROM schedule
                                                             WHERE id = @id;
                                             """, connection);

        command.Parameters.AddWithValue($"@id", shiftId);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        var id = reader.GetInt32("id");
        var scheduleId = reader.GetInt32("schedule_id");
        var userId = reader.GetInt32("user_id");

        var shiftDate = DateOnly.FromDateTime(reader.GetDateTime("shift_date"));

        var startTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan("start_time"));
        var endTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan("end_time"));

        var createdBy = reader.GetInt32("created_by");
        var createdAt = reader.GetDateTime("created_at");

        return new ShiftDto(
            id,
            scheduleId,
            userId,
            shiftDate,
            startTime,
            endTime,
            createdBy,
            createdAt
        );
    }
}