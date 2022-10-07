using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCourses.Models.Services.Application.Courses;
using MyCourses.Models.ViewModels.Courses;
using MyCourses.Models.ViewModels.Home;

namespace MyCourse.Controllers
{

    public class HomeController : Controller
    {
  
        public async Task<IActionResult> Index([FromServices] ICachedCourseService courseService)
        {
            ViewData["Title"] = "Benvenuto su MyCourse!";
            List<CourseViewModel> bestRatingCourses = await courseService.GetBestRatingCoursesAsync();
            List<CourseViewModel> mostRecentCourses = await courseService.GetMostRecentCoursesAsync();
            List<CourseViewModel> leastRatingCourses = await courseService.GetLeastRatingCoursesAsync();

            HomeViewModel viewModel = new HomeViewModel
            {
                BestRatingCourses = bestRatingCourses,
                MostRecentCourses = mostRecentCourses,
                LeastRatingCourses = leastRatingCourses
            };

            return View(viewModel);
        }
    }
}