using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCourses.Customization.Identity;
using MyCourses.Models.Options;
using MyCourses.Models.Services.Application.Courses;
using MyCourses.Models.Services.Application.Lessons;
using MyCourses.Models.Services.Infrastructure;
using System;

namespace MyCourses
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }      

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddTransient<ICourseService, AdoNetCourseService>();
            //services.AddTransient<ILessonService, AdoNetLessonService>();
            //services.AddTransient<IDatabaseAccessor, SQLDBAccessor>();

            services.AddDefaultIdentity<IdentityUser>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredUniqueChars = 4;
            })
               .AddPasswordValidator<CommonPasswordValidator<IdentityUser>>()    
                .AddEntityFrameworkStores<MyCourseDbContext>();

            services.AddRazorPages();

            services.AddTransient<ICourseService, EfCoreCourseService>();
            services.AddTransient<ILessonService, EfCoreLessonService>();
            services.AddTransient<IDatabaseAccessor, SQLDBAccessor>();

            services.AddResponseCaching();
            services.AddMvc(options =>
            {
                var homeProfile = new CacheProfile();

                //homeProfile.Duration = Configuration.GetValue<int>("ResponseCache:Home:Duration");
                //homeProfile.Location = Configuration.GetValue<ResponseCacheLocation>("ResponseCache:Home:Location");
                //homeProfile.VaryByQueryKeys = new string[] { "page" };
                Configuration.Bind("ResponseCache:Home", homeProfile);

                options.CacheProfiles.Add("Home", homeProfile);
            })
            .AddRazorRuntimeCompilation();

            //services.AddScoped<MyCourseDbContext>();
            //services.AddDbContext<MyCourseDbContext>();
            try
            {
                services.AddTransient<ICachedCourseService, MemoryCacheCourseService>();
                services.AddTransient<ICachedLessonService, MemoryCacheLessonService>();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            services.AddSingleton<IImagePersister, MagickNetImagePersister>();

            services.AddDbContextPool<MyCourseDbContext>(optionsBuilder => {
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");
                optionsBuilder.UseSqlServer(connectionString);
            });

            //Configuration Options
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<CoursesOptions>(Configuration.GetSection("Courses"));
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseExceptionHandler("/Error");
            app.UseHsts();

            app.UseResponseCaching();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages(); 
            });
        }
    }
}