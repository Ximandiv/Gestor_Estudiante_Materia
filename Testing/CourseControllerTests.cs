using Domain.Entities;
using Domain.ViewModels;
using Gestor_Estudiante_Materia.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Testing;
public class CourseControllerTests : TestBase
{
    [Fact]
    public async Task Can_Create_Course()
    {
        var mockService = new Mock<Application.Interfaces.Services.ICourseService>();
        var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<CourseController>>();

        // Setup: service should accept the course and do nothing (void)
        mockService.Setup(s => s.CreateCourseAsync(It.IsAny<Course>())).Returns(Task.CompletedTask);

        var controller = new CourseController(mockService.Object, logger);
        SetupTempData(controller);
        var vm = new CourseVM { Name = "Matemáticas", Code = "MAT101", Credits = 5 };

        var result = await controller.Create(vm);

        Assert.IsType<RedirectToActionResult>(result);
        mockService.Verify(s => s.CreateCourseAsync(It.Is<Course>(c =>
            c.Name == vm.Name && c.Code == vm.Code && c.Credits == vm.Credits)), Times.Once);
    }

    [Fact]
    public async Task Can_Edit_Course()
    {
        var mockService = new Mock<Application.Interfaces.Services.ICourseService>();
        var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<CourseController>>();

        var course = new Course { Id = 1, Name = "Matemáticas", Code = "MAT101", Credits = 5 };
        mockService.Setup(s => s.GetCourseByIdAsync(course.Id)).ReturnsAsync(course);
        mockService.Setup(s => s.UpdateCourseAsync(It.IsAny<Course>())).Returns(Task.CompletedTask);

        var controller = new CourseController(mockService.Object, logger);
        SetupTempData(controller);
        var vm = new CourseVM { Id = course.Id.ToString(), Name = "Física", Code = "FIS101", Credits = 4 };

        var result = await controller.Edit(vm);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Física", course.Name); // The controller should update the entity
        mockService.Verify(s => s.UpdateCourseAsync(course), Times.Once);
    }

    [Fact]
    public async Task Can_Delete_Course()
    {
        var mockService = new Mock<Application.Interfaces.Services.ICourseService>();
        var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<CourseController>>();

        var course = new Course { Id = 1, Name = "Matemáticas", Code = "MAT101", Credits = 5 };
        mockService.Setup(s => s.GetCourseByIdAsync(course.Id)).ReturnsAsync(course);
        mockService.Setup(s => s.DeleteCourseAsync(course.Id)).Returns(Task.CompletedTask);

        var controller = new CourseController(mockService.Object, logger);
        SetupTempData(controller);
        var result = await controller.Delete(course.Id.ToString());

        Assert.IsType<RedirectToActionResult>(result);
        mockService.Verify(s => s.DeleteCourseAsync(course.Id), Times.Once);
    }

    [Fact]
    public void CourseVM_Credits_Validation()
    {
        var vm = new CourseVM { Name = "Matemáticas", Code = "MAT101", Credits = 0 };
        var ctx = new ValidationContext(vm);
        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(vm, ctx, results, true);

        Assert.False(valid);
        Assert.Contains(results, r => r.ErrorMessage.Contains("Los créditos deben ser mayores que 0"));
    }
}
