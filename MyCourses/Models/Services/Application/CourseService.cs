using MyCourses.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MyCourses.Models.Services.Application
{
    public class CourseService : ICourseService
    {
        public List<CourseViewModel> GetCourses()
        {
            var courseList = new List<CourseViewModel>();
            var rand = new Random();
            

            for (int i=1; i<=20; i++)
            {
                var price = Convert.ToDecimal(rand.NextDouble() * 100 + 10);
                
                var course = new CourseViewModel
                {
                    Id = i,
                    Title = $"Corso n° {i}",
                    CurrentPrice = (float)price,
                    FullPrice = (float)(rand.NextDouble() > 0.5 ? price : ((price /10)*13)),
                    Author = "Nome Cognome",
                    Rating = (float)(rand.NextDouble() * 5.0),
                    ImagePath = "https://apocalottimismo.it/wp-content/uploads/2018/10/libro.jpg"
                };
                courseList.Add(course);
            }
            return courseList;
        }
        public CourseDetailViewModel GetCourse(int id)
        {
            var rand = new Random();
            var price = Convert.ToDecimal(rand.NextDouble() * 100 + 10);
            var course = new CourseDetailViewModel
            {
                Id = id,
                Title = $"Corso n° {id}",
                CurrentPrice = (float)price,
                FullPrice = (float)(rand.NextDouble() > 0.5 ? price : ((price / 10) * 13)),
                Author = "Nome Cognome",
                Rating = (float)(rand.NextDouble() * 5.0),
                ImagePath = "https://apocalottimismo.it/wp-content/uploads/2018/10/libro.jpg",
                Description = $"Descrizione {id}",
                Lessons = new List<LessonViewModel>()
            };

            for (int i = 1; i <= 5; i++)
            {
                var lesson = new LessonViewModel
                {
                    Title = $"Lezione {i}",
                    Duration = TimeSpan.FromSeconds(rand.Next(40, 90))
                };
                course.Lessons.Add(lesson);
            }
            return course;
        }

    }
}