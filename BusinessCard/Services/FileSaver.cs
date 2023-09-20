using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BusinessCard.Services
{
    /// <inheritdoc cref="IFileSaver"/>
    public class FileSaver : IFileSaver
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IWebHostEnvironment _appEnvironment;

        public FileSaver(IWebHostEnvironment appEnvironment) => _appEnvironment = appEnvironment;

        /// <inheritdoc/>
        public async Task<string> SaveFileAsync(IFormFile file, string fileDirectory, bool needAdditionalNamePart = false)
        {
            if (file is null)
                throw new Exception("Нет данных по файлу");

            var fileName = !needAdditionalNamePart
                ? file.FileName
                : Guid.NewGuid().ToString("N") + file.FileName;
            var fullFileName = Path.Combine(_appEnvironment.WebRootPath, fileDirectory, fileName);
            using var fileStream = new FileStream(fullFileName, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return fullFileName.Replace(_appEnvironment.WebRootPath, string.Empty);
        }
    }
}