using Domain.Entities;
using Domain.POCO;
using Domain.ViewModels;
using Gestor_Estudiante_Materia.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Testing;
public class AssignCourseControllerTests : TestBase
{
    [Fact]
    public async Task Can_Assign_Course_To_Student()
    {
        var studentId = Guid.NewGuid();
        var courseId = 1;

        var mockService = new Mock<Application.Interfaces.Services.IAssignCourseService>();
        mockService.Setup(s => s.GetStudentOptionsAsync())
            .ReturnsAsync(new List<StudentOption> { new StudentOption { Id = studentId, Name = "Juan" } });
        mockService.Setup(s => s.GetCourseOptionsAsync())
            .ReturnsAsync(new List<CourseOption> { new CourseOption { Id = courseId, Name = "Matemáticas", Credits = 3 } });
        mockService.Setup(s => s.IsCourseAlreadyAssignedAsync(studentId, courseId)).ReturnsAsync(false);
        mockService.Setup(s => s.GetCourseAsync(courseId)).ReturnsAsync(new Course { Id = courseId, Name = "Matemáticas", Credits = 3 });
        mockService.Setup(s => s.GetAssignedHighCreditCoursesCountAsync(studentId)).ReturnsAsync(0);
        mockService.Setup(s => s.AssignCourseAsync(studentId, courseId)).Returns(Task.CompletedTask);

        var controller = new AssignCourseController(mockService.Object);
        SetupTempData(controller);
        var vm = new AssignCourseVM { StudentId = studentId, CourseId = courseId };

        var result = await controller.Create(vm);

        Assert.IsType<RedirectToActionResult>(result);
        mockService.Verify(s => s.AssignCourseAsync(studentId, courseId), Times.Once);
    }

    [Fact]
    public async Task Cannot_Assign_Same_Course_Twice()
    {
        var studentId = Guid.NewGuid();
        var courseId = 1;

        var mockService = new Mock<Application.Interfaces.Services.IAssignCourseService>();
        mockService.Setup(s => s.GetStudentOptionsAsync())
            .ReturnsAsync(new List<StudentOption> { new StudentOption { Id = studentId, Name = "Juan" } });
        mockService.Setup(s => s.GetCourseOptionsAsync())
            .ReturnsAsync(new List<CourseOption> { new CourseOption { Id = courseId, Name = "Matemáticas", Credits = 3 } });
        mockService.Setup(s => s.IsCourseAlreadyAssignedAsync(studentId, courseId)).ReturnsAsync(true);
        mockService.Setup(s => s.GetCourseAsync(courseId)).ReturnsAsync(new Course { Id = courseId, Name = "Matemáticas", Credits = 3 });
        mockService.Setup(s => s.GetAssignedHighCreditCoursesCountAsync(studentId)).ReturnsAsync(0);

        var controller = new AssignCourseController(mockService.Object);
        SetupTempData(controller);
        var vm = new AssignCourseVM { StudentId = studentId, CourseId = courseId };

        var result = await controller.Create(vm);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Contains("asignada", controller.ModelState[string.Empty].Errors.First().ErrorMessage);
        mockService.Verify(s => s.AssignCourseAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Cannot_Assign_More_Than_3_Courses_With_More_Than_4_Credits()
    {
        var studentId = Guid.NewGuid();
        var courseId = 4;

        var mockService = new Mock<Application.Interfaces.Services.IAssignCourseService>();
        mockService.Setup(s => s.GetStudentOptionsAsync())
            .ReturnsAsync(new List<StudentOption> { new StudentOption { Id = studentId, Name = "Juan" } });
        mockService.Setup(s => s.GetCourseOptionsAsync())
            .ReturnsAsync(new List<CourseOption> { new CourseOption { Id = courseId, Name = "C4", Credits = 8 } });
        mockService.Setup(s => s.IsCourseAlreadyAssignedAsync(studentId, courseId)).ReturnsAsync(false);
        mockService.Setup(s => s.GetCourseAsync(courseId)).ReturnsAsync(new Course { Id = courseId, Name = "C4", Credits = 8 });
        mockService.Setup(s => s.GetAssignedHighCreditCoursesCountAsync(studentId)).ReturnsAsync(3);

        var controller = new AssignCourseController(mockService.Object);
        SetupTempData(controller);
        var vm = new AssignCourseVM { StudentId = studentId, CourseId = courseId };

        var result = await controller.Create(vm);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Contains("3 materias con más de 4 créditos", controller.ModelState[string.Empty].Errors.First().ErrorMessage);
        mockService.Verify(s => s.AssignCourseAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Never);
    }
}
