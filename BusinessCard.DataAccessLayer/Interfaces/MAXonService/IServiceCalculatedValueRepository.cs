using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceCalculatedValueRepository : IRepository<ServiceCalculatedValue> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rateId">  </param>
        /// <returns>  </returns>
        public Task<ServiceCalculatedValue> GetCalculatedValueDataByRateAsync(int rateId);
    }
}