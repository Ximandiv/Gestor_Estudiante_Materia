using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface IStudentRepository
{
    Task<List<Student>> GetStudentsAsync(int page, int pageSize);
    Task<int> GetTotalStudentsAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Guid id);
    Task<List<Course>> GetAssignedCoursesAsync(Guid studentId);
}
