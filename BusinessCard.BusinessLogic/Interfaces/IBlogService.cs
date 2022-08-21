using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<BlogIngormation> GetBlogInformationAsync(int? userId);
    }
}