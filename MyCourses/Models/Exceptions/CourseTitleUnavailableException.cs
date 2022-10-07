﻿using System;

namespace MyCourses.Models.Exceptions
{
    public class CourseTitleUnavailableException : Exception
    {
        public CourseTitleUnavailableException(string title, Exception innerException) : base($"Course title '{title}' existed", innerException)
        {
        }
    }
}