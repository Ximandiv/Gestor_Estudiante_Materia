using Domain.Entities;
using Domain.POCO;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;
public class AssignCourseRepository : IAssignCourseRepository
{
    private readonly SchoolDbContext _context;

    public AssignCourseRepository(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentOption>> GetStudentOptionsAsync()
    {
        return await _context.Students
            .Select(s => new StudentOption { Id = s.Id, Name = s.Name })
            .ToListAsync();
    }

    public async Task<List<CourseOption>> GetCourseOptionsAsync()
    {
        return await _context.Courses
            .Select(c => new CourseOption { Id = c.Id, Name = c.Name, Credits = c.Credits })
            .ToListAsync();
    }

    public async Task<bool> IsCourseAlreadyAssignedAsync(Guid studentId, int courseId)
    {
        return await _context.CourseStudents
            .AnyAsync(cs => cs.StudentId == studentId && cs.CourseId == courseId);
    }

    public async Task<Course?> GetCourseAsync(int courseId)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<int> GetAssignedHighCreditCoursesCountAsync(Guid studentId)
    {
        return await _context.CourseStudents
            .Where(cs => cs.StudentId == studentId)
            .Join(_context.Courses, cs => cs.CourseId, c => c.Id, (cs, c) => c)
            .CountAsync(c => c.Credits > 4);
    }

    public async Task AssignCourseAsync(Guid studentId, int courseId)
    {
        var assignment = new CourseStudents
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId
        };
        await _context.CourseStudents.AddAsync(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAssignmentAsync(Guid studentId, int courseId)
    {
        var assignment = await _context.CourseStudents
            .FirstOrDefaultAsync(cs => cs.StudentId == studentId && cs.CourseId == courseId);

        if (assignment == null)
            return false;

        _context.CourseStudents.Remove(assignment);
        await _context.SaveChangesAsync();
        return true;
    }
}
