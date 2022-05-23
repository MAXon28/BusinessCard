using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BusinessCard.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSaver
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IWebHostEnvironment _appEnvironment;

        public FileSaver(IWebHostEnvironment appEnvironment) => _appEnvironment = appEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">  </param>
        /// <param name="fileDirectory">  </param>
        /// <returns>  </returns>
        public async Task<string> SaveFileAsync(IFormFile file, string fileDirectory)
        {
            var fullFileName = string.Empty;

            if (!(file is null))
            {
                fullFileName = Path.Combine(_appEnvironment.WebRootPath, fileDirectory, Guid.NewGuid().ToString("N") + file.FileName);
                using var fileStream = new FileStream(fullFileName, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }

            return fullFileName;
        }
    }
}