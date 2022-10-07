using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MyCourses.Models.InputModels.Courses;
using MyCourses.Models.Options;
using MyCourses.Models.ViewModels.Courses;
using MyCourses.Models.ViewModels.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Application.Courses
{
    public class MemoryCacheCourseService : ICachedCourseService
    {
        private readonly ICourseService courseService;
        private readonly IMemoryCache memoryCache;
        private readonly CoursesOptions coursesOptions;
        private readonly ILogger<MemoryCacheCourseService> logger;

        public MemoryCacheCourseService(ILogger<MemoryCacheCourseService> logger, ICourseService courseService, IMemoryCache memoryCache)
        {
            this.courseService = courseService;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public MemoryCacheCourseService(ILogger<MemoryCacheCourseService> logger, ICourseService courseService, IMemoryCache memoryCache, CoursesOptions coursesOptions)
        {
            this.courseService = courseService;
            this.memoryCache = memoryCache;
            this.coursesOptions = coursesOptions;
            this.logger = logger;
        }
        public Task<List<CourseViewModel>> GetBestRatingCoursesAsync()
        {
            try
            {
                return memoryCache.GetOrCreateAsync($"BestRatingCourses", cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    return courseService.GetBestRatingCoursesAsync();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                logger.LogDebug(ex.Message);
                throw;
            }
        }

        public Task<List<CourseViewModel>> GetMostRecentCoursesAsync()
        {
            try
            {
                return memoryCache.GetOrCreateAsync($"MostRecentCourses", cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    return courseService.GetMostRecentCoursesAsync();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<List<CourseViewModel>> GetLeastRatingCoursesAsync()
        {
            try
            {
                return memoryCache.GetOrCreateAsync($"LeastRatingCourses", cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    return courseService.GetLeastRatingCoursesAsync();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                logger.LogDebug(ex.Message);
                throw;
            }
        }

        public Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
            try
            {
                return memoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    return courseService.GetCourseAsync(id);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            try
            {
                return memoryCache.GetOrCreateAsync($"Courses{model.Search}-{model.Page}-{model.OrderBy}-{model.Ascending}", cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    return courseService.GetCoursesAsync(model);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Task<CourseDetailViewModel> CreateCourseAsync(CourseCreateInputModel inputModel)
        {
            return courseService.CreateCourseAsync(inputModel);
        }

        public Task<bool> IsTitleAvailableAsync(string title, int id = 0)
        {
            return courseService.IsTitleAvailableAsync(title, id);
        }

        public Task<CourseEditInputModel> GetCourseForEditingAsync(int id)
        {
            return courseService.GetCourseForEditingAsync(id);
        }

        public async Task<CourseDetailViewModel> EditCourseAsync(CourseEditInputModel inputModel)
        {
            CourseDetailViewModel viewModel = await courseService.EditCourseAsync(inputModel);
            memoryCache.Remove($"Course{inputModel.Id}");
            return viewModel;
        }

        public async Task DeleteCourseAsync(CourseDeleteInputModel inputModel)
        {
            await courseService.DeleteCourseAsync(inputModel);
            memoryCache.Remove($"Course{inputModel.Id}");
        }
    }
}