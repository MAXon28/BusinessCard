using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITaskRepository : IRepository<TaskCard>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCard">  </param>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<string> AddTask(TaskCard taskCard, int serviceId);
    }
}