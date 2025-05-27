using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services;
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<List<CourseVM>> GetCourseListAsync(int page, int pageSize)
    {
        var courses = await _courseRepository.GetCoursesAsync(page, pageSize);
        return courses.ConvertAll(c => new CourseVM(c));
    }

    public Task<int> GetTotalCoursesAsync()
        => _courseRepository.GetTotalCoursesAsync();

    public Task<Course?> GetCourseByIdAsync(int id)
        => _courseRepository.GetByIdAsync(id);

    public Task CreateCourseAsync(Course course)
        => _courseRepository.AddAsync(course);

    public Task UpdateCourseAsync(Course course)
        => _courseRepository.UpdateAsync(course);

    public Task DeleteCourseAsync(int id)
        => _courseRepository.DeleteAsync(id);

    public Task<List<Student>> GetAssignedStudentsAsync(int courseId)
        => _courseRepository.GetAssignedStudentsAsync(courseId);
}
