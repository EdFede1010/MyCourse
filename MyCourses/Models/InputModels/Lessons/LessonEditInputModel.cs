using MyCourses.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System;

namespace MyCourses.Models.InputModels.Lessons
{
    public class LessonEditInputModel
    {
        [Required]
        public int Id { get; set; }

        public int CourseId { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio"),
         MinLength(5, ErrorMessage = "Il titolo dev'essere di almeno {1} caratteri"),
         MaxLength(100, ErrorMessage = "Il titolo dev'essere di al massimo {1} caratteri"),
         RegularExpression(@"^[0-9A-z\u00C0-\u00ff\s\.']+$", ErrorMessage = "Titolo non valido"), //Questa espressione regolare include anche le lettere accentate
         Display(Name = "Titolo")]
        public string Title { get; set; }

        [MinLength(10, ErrorMessage = "La descrizione dev'essere di almeno {1} caratteri"),
         MaxLength(8000, ErrorMessage = "La descrizione dev'essere di massimo {1} caratteri"),
         Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Display(Name = "Durata stimata"),
        Required(ErrorMessage = "La durata è richiesta")]
        public string Duration { get; set; }

        [Display(Name = "Ordine"),
        Required(ErrorMessage = "L'ordine è richiesto")]
        public int Order { get; set; }
        public byte[] RowVersion { get; set; }

        public static LessonEditInputModel FromDataRow(DataRow courseRow)
        {
            var lessonEditInputModel = new LessonEditInputModel
            {
                Id = Convert.ToInt32(courseRow["Id"]),
                Title = Convert.ToString(courseRow["Title"]),
                Duration = Convert.ToString(courseRow["Duration"]),
                Order = Convert.ToInt32(courseRow["Order"]),
                RowVersion = (byte[])courseRow["RowVersion"],
            };
            return lessonEditInputModel;
        }

       public static LessonEditInputModel FromEntity(Lesson lesson)
        {
            return new LessonEditInputModel
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description,
                Duration = lesson.Duration,
                Order = lesson.Order,
                RowVersion = lesson.RowVersion
            };
        }
    }
}