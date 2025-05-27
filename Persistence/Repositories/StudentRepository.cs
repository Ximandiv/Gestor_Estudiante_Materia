using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly SchoolDbContext _context;

    public StudentRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetStudentsAsync(int page, int pageSize)
    {
        return await _context.Students
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalStudentsAsync()
    {
        return await _context.Students.CountAsync();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Students.Where(s => s.Id == id).ExecuteDeleteAsync();
    }

    public async Task<List<Course>> GetAssignedCoursesAsync(Guid studentId)
    {
        return await _context.CourseStudents
            .Where(cs => cs.StudentId == studentId)
            .Join(_context.Courses, cs => cs.CourseId, c => c.Id, (cs, c) => c)
            .ToListAsync();
    }
}
