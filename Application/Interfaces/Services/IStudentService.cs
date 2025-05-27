using Domain.Entities;
using Domain.ViewModels;

namespace Application.Interfaces.Services;
public interface IStudentService
{
    Task<StudentListVM> GetStudentListAsync(int page, int pageSize);
    Task<Student?> GetStudentByIdAsync(Guid id);
    Task CreateStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(Guid id);
    Task<List<Course>> GetAssignedCoursesAsync(Guid studentId);
}
