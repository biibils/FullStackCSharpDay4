using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMVC.Models;

public class Attendance
{
    [Key]
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Note { get; set; }
}

public enum AttendanceStatus
{
    Hadir,
    Telat,
    Absen,
    Izin,
    Sakit,
}
