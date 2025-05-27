using Domain.Entities;
using Domain.POCO;
using Domain.ViewModels;

namespace Application.Interfaces.Services;
public interface IAssignCourseService
{
    Task<List<StudentOption>> GetStudentOptionsAsync();
    Task<List<CourseOption>> GetCourseOptionsAsync();
    Task<bool> IsCourseAlreadyAssignedAsync(Guid studentId, int courseId);
    Task<Course?> GetCourseAsync(int courseId);
    Task<int> GetAssignedHighCreditCoursesCountAsync(Guid studentId);
    Task AssignCourseAsync(Guid studentId, int courseId);
    Task<bool> DeleteAssignmentAsync(Guid studentId, int courseId);
}
