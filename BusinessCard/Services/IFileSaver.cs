using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BusinessCard.Services
{
    /// <summary>
    /// Сохраняющий файлы
    /// </summary>
    public interface IFileSaver
    {
        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <param name="file"> Файл </param>
        /// <param name="fileDirectory"> Директория, куда нужно сохранить файл </param>
        /// <param name="fileDirectory"> Нужна дополнительная часть в имя файла </param>
        /// <returns> Полный путь к сохранённому файлу </returns>
        public Task<string> SaveFileAsync(IFormFile file, string fileDirectory, bool needAdditionalNamePart = false);
    }
}