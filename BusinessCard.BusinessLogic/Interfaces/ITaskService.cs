using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.DTOs.Results;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTaskDto">  </param>
        /// <returns>  </returns>
        public Task<TaskCreationResult> AddNewTaskAsync(NewTaskDto newTaskDto);
    }
}