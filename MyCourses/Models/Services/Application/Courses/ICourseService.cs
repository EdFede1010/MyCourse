using MyCourses.Models.InputModels.Courses;
using MyCourses.Models.ViewModels.Courses;
using MyCourses.Models.ViewModels.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Application.Courses
{
    public interface ICourseService
    {
        Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model);
        Task<CourseDetailViewModel> GetCourseAsync(int id);
        Task<List<CourseViewModel>> GetMostRecentCoursesAsync();
        Task<List<CourseViewModel>> GetBestRatingCoursesAsync();
        Task<List<CourseViewModel>> GetLeastRatingCoursesAsync();
        Task<CourseDetailViewModel> CreateCourseAsync(CourseCreateInputModel inputModel);
        Task<bool> IsTitleAvailableAsync(string title, int id);
        Task<CourseEditInputModel> GetCourseForEditingAsync(int id);
        Task<CourseDetailViewModel> EditCourseAsync(CourseEditInputModel inputModel);
        Task DeleteCourseAsync(CourseDeleteInputModel inputModel);
    }
}