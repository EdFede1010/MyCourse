using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyCourses.Models.Exceptions;

namespace MyCourses.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {

#if DEBUG
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            switch (feature.Error)
            {
                case CourseNotFoundException:
                    ViewData["Title"] = "Corso non trovato!";
                    Response.StatusCode = 404;
                    return View("CourseNotFound");

                case LessonNotFoundException:
                    ViewData["Title"] = "Lezione non trovata!";
                    Response.StatusCode = 404;
                    return View("CourseNotFound");

                default:
                    ViewData["Title"] = "Errore";
                    return View();
            }
#endif
        }
    }
}
