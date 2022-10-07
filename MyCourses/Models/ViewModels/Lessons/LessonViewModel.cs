using MyCourses.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourses.Models.ViewModels.Lessons
{
    public class LessonViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public int Id { get; set; }
        internal static LessonViewModel FromDataRow(DataRow lessonRow)
        {
            var lessonViewModel = new LessonViewModel
            {
                Id = Convert.ToInt32(lessonRow["Id"]),
                Title = lessonRow["Title"].ToString(),
                Duration = Convert.ToString(lessonRow["Duration"]),
            };
            return lessonViewModel;
        }
        public static LessonViewModel FromEntity(Lesson lesson)
        {
            var lessonViewModel = new LessonViewModel
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Duration = lesson.Duration,
                Description = lesson.Description
            };
            return lessonViewModel;
        }
    }
}
