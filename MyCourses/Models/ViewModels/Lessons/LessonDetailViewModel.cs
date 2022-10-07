﻿using MyCourses.Models.Entities;
using System;
using System.Data;

namespace MyCourses.Models.ViewModels.Lessons
{
    public class LessonDetailViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }

        public static LessonDetailViewModel FromDataRow(DataRow dataRow)
        {
            var lessonViewModel = new LessonDetailViewModel
            {
                Id = Convert.ToInt32(dataRow["Id"]),
                CourseId = Convert.ToInt32(dataRow["CourseId"]),
                Title = Convert.ToString(dataRow["Title"]),
                Duration = Convert.ToString(dataRow["Duration"]),
                Description = Convert.ToString(dataRow["Description"])
            };
            return lessonViewModel;
        }

        public static LessonDetailViewModel FromEntity(Lesson lesson)
        {
            return new LessonDetailViewModel
            {
                Id = lesson.Id,
                CourseId = lesson.CourseId,
                Title = lesson.Title,
                Duration = lesson.Duration,
                Description = lesson.Description
            };
        }
    }
}