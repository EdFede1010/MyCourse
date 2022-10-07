using System;
using System.Collections.Generic;

namespace MyCourses.Models.ViewModels.Lessons
{
    public class ListViewModel<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
