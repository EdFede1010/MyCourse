using Microsoft.AspNetCore.Mvc;
using MyCourses.Models.Exceptions;
using MyCourses.Models.InputModels.Courses;
using MyCourses.Models.Services.Application.Courses;
using MyCourses.Models.ViewModels.Courses;
using MyCourses.Models.ViewModels.Lessons;
using System;
using System.Threading.Tasks;

namespace MyCourses.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;
        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService = courseService;
        }

        public async Task<IActionResult> Index(CourseListInputModel input)
        {
            try
            {
                ViewData["Title"] = "Catalogo dei corsi";
                ListViewModel<CourseViewModel> courses = await courseService.GetCoursesAsync(input);

                CourseListViewModel viewModel = new CourseListViewModel
                {
                    Courses = courses,
                    Input = input
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel viewModel = await courseService.GetCourseAsync(id);
            ViewData["Title"] = viewModel.Title;
            return View(viewModel);
        }
        public IActionResult Create()
        {
            ViewData["Title"] = "Nuovo Corso";
            var courseCreateInputModel = new CourseCreateInputModel();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInputModel inputModel)
        {
            if (ModelState.IsValid)
                try
                {
                    CourseDetailViewModel course = await courseService.CreateCourseAsync(inputModel);
                    TempData["ConfirmationMessage"] = "Corso creato con successo!";
                    return RedirectToAction(nameof(Edit), new { id = course.Id });
                }
                catch (CourseTitleUnavailableException)
                {
                    ModelState.AddModelError(nameof(CourseDetailViewModel.Title), "Questo titolo esiste già");
                }
            ViewData["Title"] = "Nuovo Corso";
            return View(inputModel);
        }
        public async Task<IActionResult> IsTitleAvailable(string title, int id = 0)
        {
            bool result = await courseService.IsTitleAvailableAsync(title, id);
            return Json(result);
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Modifica il corso";
            CourseEditInputModel inputModel = await courseService.GetCourseForEditingAsync(id);
            return View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CourseEditInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CourseDetailViewModel course = await courseService.EditCourseAsync(inputModel);
                    TempData["ConfirmationMessage"] = "I tuoi dati sono stati salvati";
                    return RedirectToAction(nameof(Detail), new { id = inputModel.Id });
                }
                catch (CourseTitleUnavailableException)
                {
                    ModelState.AddModelError(nameof(CourseEditInputModel.Title), "Questo titolo esiste già");
                }
                catch (CourseImageInvalidException)
                {
                    ModelState.AddModelError(nameof(CourseEditInputModel.Image), "Immagine non valida");
                }
                catch (OptimisticConcurrencyException)
                {
                    ModelState.AddModelError("", "Il salvataggio non è andato a buon fine, qualcun'altro ha già aggiornato i dati, aggiornare la pagina.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            ViewData["Title"] = "Modifica il corso";
            return View(inputModel);
        }
    }
}