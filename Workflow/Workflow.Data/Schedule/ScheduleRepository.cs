using MySql.Data.MySqlClient;

namespace Data.Schedule;

public class ScheduleRepository(IDbConnectionFactory factory) : IScheduleRepository
{
    public ScheduleDto CreateSchedule(CreateScheduleDto dto)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      INSERT INTO schedule (week_number, year, created_by)
                                                      VALUES (@week_number, @year, @created_by);
                                                      SELECT id, week_number, year, published, created_by, created_at
                                                      FROM schedule
                                                      WHERE id = LAST_INSERT_ID();
                                              """, connection);

        command.Parameters.AddWithValue("@week_number", dto.WeekNumber);
        command.Parameters.AddWithValue("@year", dto.Year);
        command.Parameters.AddWithValue("@created_by", dto.CreatedBy);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to create schedule.");

        return ReadScheduleDto(reader);
    }

    public ScheduleDto? GetScheduleById(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT id, week_number, year, published, created_by, created_at
                                                      FROM schedule
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return ReadScheduleDto(reader);
    }

    public ScheduleDto? GetScheduleByWeek(int weekNumber, int year)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT id, week_number, year, published, created_by, created_at
                                                      FROM schedule
                                                      WHERE week_number = @week_number AND year = @year;
                                              """, connection);

        command.Parameters.AddWithValue("@week_number", weekNumber);
        command.Parameters.AddWithValue("@year", year);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return ReadScheduleDto(reader);
    }

    public List<ScheduleDto> GetAllSchedules()
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT id, week_number, year, published, created_by, created_at
                                                      FROM schedule
                                                      ORDER BY year DESC, week_number DESC;
                                              """, connection);

        using var reader = command.ExecuteReader();
        var schedules = new List<ScheduleDto>();
        while (reader.Read())
            schedules.Add(ReadScheduleDto(reader));

        return schedules;
    }

    public void PublishSchedule(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      UPDATE schedule
                                                      SET published = TRUE
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    public void DeleteSchedule(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      DELETE FROM schedule
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    public ShiftDto CreateShift(CreateShiftDto dto)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      INSERT INTO shift (schedule_id, user_id, shift_date, start_time, end_time, created_by)
                                                      VALUES (@schedule_id, @user_id, @shift_date, @start_time, @end_time, @created_by);
                                                      SELECT id, schedule_id, user_id, shift_date, start_time, end_time, created_by, created_at
                                                      FROM shift
                                                      WHERE id = LAST_INSERT_ID();
                                              """, connection);

        command.Parameters.AddWithValue("@schedule_id", dto.ScheduleId);
        command.Parameters.AddWithValue("@user_id", dto.UserId);
        command.Parameters.AddWithValue("@shift_date", dto.ShiftDate.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@start_time", dto.StartTime.ToString("HH:mm:ss"));
        command.Parameters.AddWithValue("@end_time", dto.EndTime.ToString("HH:mm:ss"));
        command.Parameters.AddWithValue("@created_by", dto.CreatedBy);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to create shift.");

        return ReadShiftDto(reader);
    }

    public ShiftDto? GetShift(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT id, schedule_id, user_id, shift_date, start_time, end_time, created_by, created_at
                                                      FROM shift
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return ReadShiftDto(reader);
    }

    public ShiftDto UpdateShift(int id, CreateShiftDto dto)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      UPDATE shift
                                                      SET schedule_id = @schedule_id,
                                                          user_id = @user_id,
                                                          shift_date = @shift_date,
                                                          start_time = @start_time,
                                                          end_time = @end_time,
                                                          created_by = @created_by
                                                      WHERE id = @id;
                                                      SELECT id, schedule_id, user_id, shift_date, start_time, end_time, created_by, created_at
                                                      FROM shift
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@schedule_id", dto.ScheduleId);
        command.Parameters.AddWithValue("@user_id", dto.UserId);
        command.Parameters.AddWithValue("@shift_date", dto.ShiftDate.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@start_time", dto.StartTime.ToString("HH:mm:ss"));
        command.Parameters.AddWithValue("@end_time", dto.EndTime.ToString("HH:mm:ss"));
        command.Parameters.AddWithValue("@created_by", dto.CreatedBy);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) throw new Exception("Failed to update shift.");

        return ReadShiftDto(reader);
    }

    public void DeleteShift(int id)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      DELETE FROM shift
                                                      WHERE id = @id;
                                              """, connection);

        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    public List<ShiftDto> GetShiftsByScheduleId(int scheduleId)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT s.id, s.schedule_id, s.user_id, s.shift_date, s.start_time, s.end_time, s.created_by, s.created_at
                                                      FROM shift s
                                                      WHERE s.schedule_id = @schedule_id
                                                      ORDER BY s.shift_date, s.start_time;
                                              """, connection);

        command.Parameters.AddWithValue("@schedule_id", scheduleId);

        using var reader = command.ExecuteReader();
        var shifts = new List<ShiftDto>();
        while (reader.Read())
            shifts.Add(ReadShiftDto(reader));

        return shifts;
    }

    public List<ShiftDto> GetShiftsByUserIdAndScheduleId(int userId, int scheduleId)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT s.id, s.schedule_id, s.user_id, s.shift_date, s.start_time, s.end_time, s.created_by, s.created_at
                                                      FROM shift s
                                                      WHERE s.user_id = @user_id AND s.schedule_id = @schedule_id
                                                      ORDER BY s.shift_date, s.start_time;
                                              """, connection);

        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@schedule_id", scheduleId);

        using var reader = command.ExecuteReader();
        var shifts = new List<ShiftDto>();
        while (reader.Read())
            shifts.Add(ReadShiftDto(reader));

        return shifts;
    }

    public List<ShiftDto> GetShiftsByUserId(int userId)
    {
        using var connection = factory.CreateOpenConnection();

        using var command = new MySqlCommand("""
                                                      SELECT s.id, s.schedule_id, s.user_id, s.shift_date, s.start_time, s.end_time, s.created_by, s.created_at
                                                      FROM shift s
                                                      JOIN schedule sch ON sch.id = s.schedule_id
                                                      WHERE s.user_id = @user_id AND sch.published = TRUE
                                                      ORDER BY s.shift_date DESC, s.start_time;
                                              """, connection);

        command.Parameters.AddWithValue("@user_id", userId);

        using var reader = command.ExecuteReader();
        var shifts = new List<ShiftDto>();
        while (reader.Read())
            shifts.Add(ReadShiftDto(reader));

        return shifts;
    }

    private static ScheduleDto ReadScheduleDto(MySqlDataReader reader)
    {
        var id = reader.GetInt32("id");
        var weekNumber = reader.GetInt32("week_number");
        var year = reader.GetInt32("year");
        var published = reader.GetBoolean("published");
        var createdBy = reader.GetInt32("created_by");
        var createdAt = reader.GetDateTime("created_at");

        return new ScheduleDto(id, weekNumber, year, published, createdBy, createdAt);
    }

    private static ShiftDto ReadShiftDto(MySqlDataReader reader)
    {
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
