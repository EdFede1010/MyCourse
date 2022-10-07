using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourses.Models.Entities;
using MyCourses.Models.Enums;
using MyCourses.Models.Exceptions;
using MyCourses.Models.InputModels.Courses;
using MyCourses.Models.Options;
using MyCourses.Models.Services.Infrastructure;
using MyCourses.Models.ViewModels.Courses;
using MyCourses.Models.ViewModels.Lessons;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Application.Courses
{
    public class EfCoreCourseService : ICourseService
    {
        private readonly ILogger<EfCoreCourseService> logger;
        private readonly MyCourseDbContext dbContext;
        private readonly IOptionsMonitor<CoursesOptions> coursesOptions;
        private readonly IImagePersister imagePersister;
        public EfCoreCourseService(ILogger<EfCoreCourseService> logger, MyCourseDbContext dbContext, IOptionsMonitor<CoursesOptions> coursesOptions, IImagePersister imagePersister)
        {
            this.coursesOptions = coursesOptions;
            this.imagePersister = imagePersister;
            this.logger = logger;
            this.dbContext = dbContext;
        }
        public async Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
            try
            {
                IQueryable<CourseDetailViewModel> queryLinq = dbContext.Courses
                    .AsNoTracking()
                    .Include(course => course.Lessons)
                    .Where(course => course.Id == id)
                    .Select(course => CourseDetailViewModel.FromEntity(course)); //Usando metodi statici come FromEntity, la query potrebbe essere inefficiente. Mantenere il mapping nella lambda oppure usare un extension method personalizzato

                CourseDetailViewModel viewModel = await queryLinq.FirstOrDefaultAsync();
                if (viewModel == null)
                {
                    logger.LogWarning("Course {id} not found", id);
                    throw new CourseNotFoundException(id);
                }
                return viewModel;
            }
            catch (CourseNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<CourseViewModel>> GetBestRatingCoursesAsync()
        {
            try
            {
                CourseListInputModel inputModel = new CourseListInputModel(
                    search: "",
                    page: 1,
                    orderby: "Rating",
                    ascending: false,
                    limit: coursesOptions.CurrentValue.InHome,
                    orderOptions: coursesOptions.CurrentValue.Order);

                ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
                return result.Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<CourseViewModel>> GetLeastRatingCoursesAsync()
        {
            try
            {
                CourseListInputModel inputModel = new CourseListInputModel(
                    search: "",
                    page: 1,
                    orderby: "Rating",
                    ascending: true,
                    limit: coursesOptions.CurrentValue.InHome,
                    orderOptions: coursesOptions.CurrentValue.Order);

                ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
                return result.Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<CourseViewModel>> GetMostRecentCoursesAsync()
        {
            try
            {
                CourseListInputModel inputModel = new CourseListInputModel(
                    search: "",
                    page: 1,
                    orderby: "Id",
                    ascending: false,
                    limit: coursesOptions.CurrentValue.InHome,
                    orderOptions: coursesOptions.CurrentValue.Order);

                ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
                return result.Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            try
            {
                IQueryable<Course> baseQuery = dbContext.Courses;

                baseQuery = (model.OrderBy, model.Ascending) switch
                {
                    ("Title", true) => baseQuery.OrderBy(course => course.Title),
                    ("Title", false) => baseQuery.OrderByDescending(course => course.Title),
                    ("Rating", true) => baseQuery.OrderBy(course => course.Rating),
                    ("Rating", false) => baseQuery.OrderByDescending(course => course.Rating),
                    ("CurrentPrice", true) => baseQuery.OrderBy(course => course.CurrentPrice.Amount),
                    ("CurrentPrice", false) => baseQuery.OrderByDescending(course => course.CurrentPrice.Amount),
                    ("Id", true) => baseQuery.OrderBy(course => course.Id),
                    ("Id", false) => baseQuery.OrderByDescending(course => course.Id),
                    _ => baseQuery
                };

                IQueryable<Course> queryLinq = baseQuery
                    .Where(course => course.Title.Contains(model.Search))
                    .AsNoTracking();

                List<CourseViewModel> courses = await queryLinq
                    .Skip(model.Offset)
                    .Take(model.Limit)
                    .Select(course => CourseViewModel.FromEntity(course))
                    .ToListAsync();

                int totalCount = await queryLinq.CountAsync();

                ListViewModel<CourseViewModel> result = new()
                {
                    Results = courses,
                    TotalCount = totalCount
                };

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CourseDetailViewModel> CreateCourseAsync(CourseCreateInputModel inputModel)
        {

            string title = inputModel.Title;
            string author = "Mario Rossi";

            var course = new Course(title, author);

            dbContext.Add(course);
            await dbContext.SaveChangesAsync();

            return CourseDetailViewModel.FromEntity(course);
        }

        public async Task<CourseEditInputModel> GetCourseForEditingAsync(int id)
        {
            try
            {
                IQueryable<CourseEditInputModel> queryLinq = dbContext.Courses
                    .AsNoTracking()
                    .Where(course => course.Id == id)
                    .Select(course => CourseEditInputModel.FromEntity(course)); //Usando metodi statici come FromEntity, la query potrebbe essere inefficiente. Mantenere il mapping nella lambda oppure usare un extension method personalizzato

                CourseEditInputModel viewModel = await queryLinq.FirstOrDefaultAsync();

                if (viewModel == null)
                {
                    logger.LogWarning("Course {id} not found", id);
                    throw new CourseNotFoundException(id);
                }
                return viewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CourseDetailViewModel> EditCourseAsync(CourseEditInputModel inputModel)
        {
            try
            {
                Course course = await dbContext.Courses.FindAsync(inputModel.Id);

                if (course == null)
                {
                    throw new CourseNotFoundException(inputModel.Id);
                }

                course.ChangeTitle(inputModel.Title);
                course.ChangePrices(inputModel.FullPrice, inputModel.CurrentPrice);
                course.ChangeDescription(inputModel.Description);
                course.ChangeEmail(inputModel.Email);

                dbContext.Entry(course).Property(course => course.RowVersion).OriginalValue = inputModel.RowVersion;

                if (inputModel.Image != null)
                {
                    try
                    {
                        string imagePath = await imagePersister.SaveCourseImageAsync(inputModel.Id, inputModel.Image);
                        course.ChangePath(imagePath);
                    }
                    catch (Exception ex)
                    {
                        throw new CourseImageInvalidException(inputModel.Id, ex);
                    }
                }
                //dbContext.Update(course);

                await dbContext.SaveChangesAsync();

                return CourseDetailViewModel.FromEntity(course);
            }
            catch (DbUpdateException exc) when ((exc.InnerException as SqlException)?.ErrorCode == 2601)
            {
                throw new CourseTitleUnavailableException(inputModel.Title, exc);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new OptimisticConcurrencyException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsTitleAvailableAsync(string title, int id)
        {
            bool titleExist = await dbContext.Courses.AnyAsync(course => EF.Functions.Like(course.Title, title) && course.Id != id);
            return !titleExist;
        }

        public async Task DeleteCourseAsync(CourseDeleteInputModel inputmodel)
        {
            Course course = await dbContext.Courses.FindAsync(inputmodel.Id);

            if (course == null)
            {
                throw new CourseNotFoundException(inputmodel.Id);
            }
            course.ChangeStatus(CourseStatus.Deleted);
            await dbContext.SaveChangesAsync();
        }
    }
}