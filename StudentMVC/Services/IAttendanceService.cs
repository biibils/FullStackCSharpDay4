using StudentMVC.Models;

namespace StudentMVC.Services;

public interface IAttendanceService
{
    void CreateAttendance(Attendance attendance);
    IEnumerable<Attendance> GetAll();
}
