using MyCourses.Models.InputModels.Lessons;
using MyCourses.Models.ViewModels.Lessons;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Application.Lessons
{
    public interface ILessonService
    {
        Task<LessonDetailViewModel> GetLessonAsync(int id);
        Task<LessonEditInputModel> GetLessonForEditingAsync(int id);
        Task<LessonDetailViewModel> CreateLessonAsync(LessonCreateInputModel inputModel);
        Task<LessonDetailViewModel> EditLessonAsync(LessonEditInputModel inputModel);
        Task DeleteLessonAsync(LessonDeleteInputModel id);
    }
}