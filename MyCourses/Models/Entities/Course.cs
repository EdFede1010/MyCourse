using Microsoft.Extensions.Logging;
using MyCourses.Models.Enums;
using MyCourses.Models.ValueTypes;
using System;
using System.Collections.Generic;

namespace MyCourses.Models.Entities
{
    public partial class Course
    {
        public Course(string title, string author)
        {
            ChangeTitle(title);
            ChangeAuthor(author);
            ChangeStatus(CourseStatus.Draft);

            Lessons = new HashSet<Lesson>();
            CurrentPrice = new Money(Currency.EUR, 1);
            FullPrice = new Money(Currency.EUR, 1);
            ImagePath = "/Courses/default.png";
        }


        public int Id               { get; private set; }
        public string Title         { get; private set; }
        public string Description   { get; private set; }
        public string ImagePath     { get; private set; }
        public string Author        { get; private set; }
        public string Email         { get; private set; }
        public Single Rating        { get; private set; }
        public Money FullPrice      { get; private set; }
        public Money CurrentPrice   { get; private set; }
        public byte[] RowVersion    { get; private set; }
        public CourseStatus Status  { get; private set; }


        public void ChangeStatus(CourseStatus newStatus)
        {
            Status = newStatus;
        }
        public void ChangeTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                Console.WriteLine("Corso senza titolo");
                throw new ArgumentException("The course must have a title");
            }
            Title = newTitle;
        }                
        public void ChangePrices(Money newFullPrice, Money newDiscountPrice)
        {
            if (newFullPrice == null || newDiscountPrice == null)
            {
                throw new ArgumentException("Prices can't be null");
            }
            if (newFullPrice.Currency != newDiscountPrice.Currency)
            {
                throw new ArgumentException("Prices don't match");
            }
            if (newFullPrice.Amount < newDiscountPrice.Amount)
            {
                throw new ArgumentException("Full price must be higher then discounted price!");
            }
            FullPrice = newFullPrice;
            CurrentPrice = newDiscountPrice;
        }
        public void ChangeEmail (string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            Email = newEmail;
        }
        public void ChangeDescription(string newDescription)
        {
            if (string.IsNullOrEmpty(newDescription))
            {
                throw new ArgumentException("Email cannot be empty");
            }
            Description = newDescription;
        }
        internal void ChangePath(string newImagePath)
        {
            if (string.IsNullOrEmpty(newImagePath)) {
                throw new NotImplementedException();
            }
            ImagePath = newImagePath;
        }
        public void ChangeAuthor(string newAuthor)
        {
            if (string.IsNullOrEmpty(newAuthor)) {
                throw new ArgumentException("The Author must have a name");
            }
            Author = newAuthor;
        }
        public virtual ICollection<Lesson> Lessons { get; private set; }
    }
}