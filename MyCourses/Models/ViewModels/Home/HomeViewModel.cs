using System.Collections.Generic;
using MyCourses.Models.ViewModels.Courses;

namespace MyCourses.Models.ViewModels.Home
{
    public class HomeViewModel : CourseViewModel
    {
        public List<CourseViewModel> MostRecentCourses { get; set; }
        public List<CourseViewModel> BestRatingCourses { get; set; }
        public List<CourseViewModel> LeastRatingCourses { get; set; }
    }
}
