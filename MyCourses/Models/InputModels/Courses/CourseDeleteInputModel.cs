using System.ComponentModel.DataAnnotations;

namespace MyCourses.Models.InputModels.Courses
{
    public class CourseDeleteInputModel
    {
        [Required]
        public int Id { get; set; }
    }
}
