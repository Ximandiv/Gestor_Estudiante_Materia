using Application.Interfaces.Services;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Estudiante_Materia.Controllers;

public class AssignCourseController : Controller
{
    private readonly IAssignCourseService _assignCourseService;

    public AssignCourseController(IAssignCourseService assignCourseService)
    {
        _assignCourseService = assignCourseService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new AssignCourseVM
        {
            Students = await _assignCourseService.GetStudentOptionsAsync(),
            Courses = await _assignCourseService.GetCourseOptionsAsync()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AssignCourseVM vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Students = await _assignCourseService.GetStudentOptionsAsync();
            vm.Courses = await _assignCourseService.GetCourseOptionsAsync();
            return View(vm);
        }

        // Check if already assigned
        if (await _assignCourseService.IsCourseAlreadyAssignedAsync(vm.StudentId, vm.CourseId))
        {
            ModelState.AddModelError("", "Esta materia ya está asignada a este estudiante.");
        }

        // Get course and check credits
        var course = await _assignCourseService.GetCourseAsync(vm.CourseId);
        if (course == null)
        {
            ModelState.AddModelError("", "Materia no encontrada.");
        }
        else if (course.Credits > 4)
        {
            var count = await _assignCourseService.GetAssignedHighCreditCoursesCountAsync(vm.StudentId);
            if (count >= 3)
            {
                ModelState.AddModelError("", "El estudiante ya está inscrito en 3 materias con más de 4 créditos.");
            }
        }

        if (!ModelState.IsValid)
        {
            vm.Students = await _assignCourseService.GetStudentOptionsAsync();
            vm.Courses = await _assignCourseService.GetCourseOptionsAsync();
            return View(vm);
        }

        try
        {
            await _assignCourseService.AssignCourseAsync(vm.StudentId, vm.CourseId);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Un error ha ocurrido al asignar la materia: {ex.Message}");
            vm.Students = await _assignCourseService.GetStudentOptionsAsync();
            vm.Courses = await _assignCourseService.GetCourseOptionsAsync();
            return View(vm);
        }

        TempData["IndexModalMsg"] = "Materia asignada con éxito!";
        return RedirectToAction("Index", "Student");
    }

    [HttpGet]
    public async Task<IActionResult> Delete()
    {
        var vm = new AssignCourseVM
        {
            Students = await _assignCourseService.GetStudentOptionsAsync(),
            Courses = await _assignCourseService.GetCourseOptionsAsync()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(AssignCourseVM vm)
    {
        if (vm.StudentId == Guid.Empty || vm.CourseId == 0)
        {
            ModelState.AddModelError("", "Debes seleccionar un estudiante y una materia.");
        }
        else
        {
            var deleted = await _assignCourseService.DeleteAssignmentAsync(vm.StudentId, vm.CourseId);
            if (!deleted)
            {
                ModelState.AddModelError("", "El estudiante no tiene esta materia asignada.");
            }
            else
            {
                TempData["IndexModalMsg"] = "Asignación eliminada con éxito!";
                return RedirectToAction("Index", "Student");
            }
        }

        vm.Students = await _assignCourseService.GetStudentOptionsAsync();
        vm.Courses = await _assignCourseService.GetCourseOptionsAsync();
        return View("Delete", vm);
    }
}
