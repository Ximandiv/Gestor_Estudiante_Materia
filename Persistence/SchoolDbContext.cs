using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

/// <summary>
/// Represents the database context for the school application,
/// managing entities such as students, courses, and their associations.
/// For any dotnet ef make sure to do it at root location using
/// --project Persistence --startup-project Gestor_Estudiante_Materia
/// </summary>
public class SchoolDbContext : DbContext
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<CourseStudents> CourseStudents { get; set; } = null!;

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
    {
    }
}
