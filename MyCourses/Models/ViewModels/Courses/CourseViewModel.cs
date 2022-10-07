using MyCourses.Models.Entities;
using MyCourses.Models.Enums;
using MyCourses.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyCourses.Models.ViewModels.Courses
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }
        public Money FullPrice { get; set; }
        public Money CurrentPrice { get; set; }

        public static CourseViewModel FromDataRow(DataRow courseRow)
        {
            try
            {
                var courseViewModel = new CourseViewModel
                {
                    Title = Convert.ToString(courseRow["Title"]),
                    ImagePath = Convert.ToString(courseRow["ImagePath"]),
                    Author = Convert.ToString(courseRow["Author"]),
                    Rating = Convert.ToDouble(courseRow["Rating"]),
                    FullPrice = new Money(
                        Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                        Convert.ToDecimal(courseRow["FullPrice_Amount"])
                    ),
                    CurrentPrice = new Money(
                        Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                        Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                    ),
                    Id = Convert.ToInt32(courseRow["Id"])
                };
                return courseViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static CourseViewModel FromEntity(Course course)
        {
            try
            {
                return new CourseViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    ImagePath = course.ImagePath,
                    Author = course.Author,
                    Rating = course.Rating,
                    CurrentPrice = course.CurrentPrice,
                    FullPrice = course.FullPrice
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}