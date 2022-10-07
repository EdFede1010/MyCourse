using Microsoft.AspNetCore.Mvc;
using MyCourses.Controllers;
using System.ComponentModel.DataAnnotations;

namespace MyCourses.Models.InputModels.Courses
{
    public class CourseCreateInputModel
    {
        [Required(ErrorMessage = "Il titolo è obbligatorio"),
            MinLength(10, ErrorMessage = "Il titolo deve avere almeno {1} caratteri"),
            MaxLength(100, ErrorMessage = "Il titolo deve avere almeno {1} caratteri"),
            RegularExpression(@"^[\w\s\.]+$", ErrorMessage = "Sono stati inseriti caratteri non validi"),
            Remote(action: nameof(CoursesController.IsTitleAvailable), controller: "Courses", ErrorMessage = "Il titolo esiste già")]
        public string Title { get; set; }
    }
}