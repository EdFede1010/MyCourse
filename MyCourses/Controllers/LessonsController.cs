using Microsoft.AspNetCore.Mvc;
using MyCourses.Models.Exceptions;
using MyCourses.Models.InputModels.Lessons;
using MyCourses.Models.Services.Application.Lessons;
using MyCourses.Models.ViewModels.Lessons;
using System;
using System.Threading.Tasks;

namespace MyCourses.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ICachedLessonService lessonService;
        public LessonsController(ICachedLessonService lessonService)
        {
            this.lessonService = lessonService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            LessonDetailViewModel viewModel = await lessonService.GetLessonAsync(id);
            ViewData["Title"] = viewModel.Title;
            return View(viewModel);
        }
        public IActionResult Create(int id)
        {
            ViewData["Title"] = "Nuova lezione";
            var inputModel = new LessonCreateInputModel();
            inputModel.CourseId = id;
            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LessonCreateInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                LessonDetailViewModel lesson = await lessonService.CreateLessonAsync(inputModel);
                TempData["ConfirmationMessage"] = "Lezione creata con successo! Ora aggiungi i dettagli.";
                return RedirectToAction(nameof(Edit), new { id = lesson.Id });
            }

            ViewData["Title"] = "Nuova lezione";
            return View(inputModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Modifica lezione";
            LessonEditInputModel inputModel = await lessonService.GetLessonForEditingAsync(id);
            return View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LessonEditInputModel inputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LessonDetailViewModel viewModel = await lessonService.EditLessonAsync(inputModel);
                    TempData["ConfirmationMessage"] = "Salvataggio avvenuto con successo!";
                    return RedirectToAction(nameof(Detail), new { id = viewModel.Id });
                }
                catch (OptimisticConcurrencyException)
                {
                    ModelState.AddModelError("", "Spiacenti, salvataggio non avvenuto, qualcuno ha già aggiornato questa lezione. Aggiornare la pagina e ripetere le modifiche.");
                }
            }
            ViewData["Title"] = "Modifica lezione";
            return View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(LessonDeleteInputModel inputModel)
        {
            try
            {
                if (inputModel.Id != 0)
                {
                    await lessonService.DeleteLessonAsync(inputModel);
                    TempData["ConfirmationMessage"] = "Lezione eliminata con successo";
                    return RedirectToAction(nameof(CoursesController.Detail), "Courses", new { id = inputModel.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Qualcosa è andato storto e la lezione non è stata cancellata");
                    return RedirectToAction(nameof(CoursesController.Edit), "Lessons", new { id = inputModel.Id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}