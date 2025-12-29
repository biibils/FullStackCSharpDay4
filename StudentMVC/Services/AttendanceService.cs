using StudentMVC.Models;

namespace StudentMVC.Services;

public class AttendanceService : IAttendanceService
{
    private readonly List<Attendance> _attendances = [];

    public void CreateAttendance(Attendance attendance)
    {
        attendance.Id = _attendances.Count + 1;
        _attendances.Add(attendance);
    }

    public IEnumerable<Attendance> GetAll()
    {
        return _attendances;
    }
}
