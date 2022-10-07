using System;

namespace MyCourses.Models.Entities
{
    public partial class Lesson
    {
        public Lesson(string title, int courseId)
        {
            ChangeTitle(title);
            CourseId = courseId;
            Order = 1000;
            //Duration = TimeSpan.FromSeconds(0);
            Duration = "00:00:00";
        }
        public int Id { get; private set; }
        public int CourseId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Duration { get; private set; }//00:00:00        
        public byte[] RowVersion { get; private set; }
        public int Order { get; private set; }
        public virtual Course Course { get; set; }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("A lesson must have a title");
            }
            Title = title;
        }
        public void ChangeDescription(string description)
        {
            Description = description;
        }
        public void ChangeDuration(string duration)
        {
            Duration = duration;
        }
        public void ChangeOrder(int order)
        {
            Order = order;
        }
    }
}