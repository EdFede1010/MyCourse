using System;

namespace MyCourses.Models.Exceptions
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException() : base($"Cound't update row because it was apdated by another user") { }
    }
}
