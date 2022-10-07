using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyCourses.Models.Exceptions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Infrastructure
{
    public class MagickNetImagePersister : IImagePersister
    {
        private readonly IWebHostEnvironment env;
        private readonly SemaphoreSlim semaphore;

        public MagickNetImagePersister(IWebHostEnvironment env)
        {
            ResourceLimits.Width = 4000;
            ResourceLimits.Height = 4000;
            semaphore = new SemaphoreSlim(2);
            this.env = env;
        }
        public async Task<string> SaveCourseImageAsync(int courseId, IFormFile formFile)
        {
            await semaphore.WaitAsync();
            try
            {
                //Salvare il file
                string path = $"/Courses/{courseId}.jpg";
                string physicalPath = Path.Combine(env.WebRootPath, "Courses", $"{courseId}.jpg");

                using Stream inputStream = formFile.OpenReadStream();
                using MagickImage image = new MagickImage(inputStream);

                //Modifico l'immagine
                int width = 300;
                int height = 300;
                MagickGeometry resizeGeometry = new MagickGeometry(width, height)
                {
                    FillArea = true
                };
                image.Resize(resizeGeometry);
                image.Crop(width, height, Gravity.Northwest);

                image.Quality = 65;
                image.Write(physicalPath, MagickFormat.Jpg);
                //Restituiamo il percorso del file
                return path;
            }catch (Exception ex)
            {
                throw new ImagePersistenceException(ex);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
