using BusinessCard.DataAccessLayer.Entities.MAXonBlog.PostDetails;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostElementRepository : IRepository<PostElement> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postKey">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<PostElement>> GetPostElementsAsync(string postKey);
    }
}