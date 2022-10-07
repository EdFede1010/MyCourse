using System.ComponentModel.DataAnnotations;

namespace MyCourses.Models.InputModels.Lessons
{
    public class LessonDeleteInputModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}
