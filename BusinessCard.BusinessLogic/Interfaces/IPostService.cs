using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    public interface IPostService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewsPackageNumber">  </param>
        /// <returns>  </returns>
        public Task<List<PostDto>> GetPostsAsync(int reviewsPackageNumber);
    }
}