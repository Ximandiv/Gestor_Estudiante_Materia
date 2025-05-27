using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;
public class CourseRepository : ICourseRepository
{
    private readonly SchoolDbContext _context;

    public CourseRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetCoursesAsync(int page, int pageSize)
    {
        return await _context.Courses
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCoursesAsync()
    {
        return await _context.Courses.CountAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Courses.Where(c => c.Id == id).ExecuteDeleteAsync();
    }

    public async Task<List<Student>> GetAssignedStudentsAsync(int courseId)
    {
        return await _context.CourseStudents
            .Where(cs => cs.CourseId == courseId)
            .Join(_context.Students, cs => cs.StudentId, s => s.Id, (cs, s) => s)
            .ToListAsync();
    }
}
