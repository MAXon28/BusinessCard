using BusinessCard.DataAccessLayer.Entities.Content.Services;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceRepository : IRepository<Service> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        //public Task<List<Service>> GetAllServicesWithShortDescriptions();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<string> GetFullDescriptionByServiceId(int serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<Service> GetService(int serviceId);
    }
}