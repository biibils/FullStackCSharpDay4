using Microsoft.EntityFrameworkCore;
using StudentMVC.Models;

namespace StudentMVC.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Student>().ToTable("Students");
        modelBuilder.Entity<Attendance>().ToTable("Attendance");
        modelBuilder.Entity<Course>().ToTable("Course");

        modelBuilder
            .Entity<Course>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Courses)
            .UsingEntity(j => j.ToTable("CourseStudents"));
    }
}
