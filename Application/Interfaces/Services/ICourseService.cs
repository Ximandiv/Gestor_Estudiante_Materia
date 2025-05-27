using Domain.Entities;
using Domain.ViewModels;

namespace Application.Interfaces.Services;
public interface ICourseService
{
    Task<List<CourseVM>> GetCourseListAsync(int page, int pageSize);
    Task<int> GetTotalCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task CreateCourseAsync(Course course);
    Task UpdateCourseAsync(Course course);
    Task DeleteCourseAsync(int id);
    Task<List<Student>> GetAssignedStudentsAsync(int courseId);
}
