using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    internal class TaskRequestSettings : RequestSettings
    {
        public TaskRequestSettings(int lastTaskId, int tasksCountInPackage, string searchText, int userId, TaskStatusTypes statusType) : base(lastTaskId, tasksCountInPackage, searchText)
        {
            UserId = userId;
            StatusType = statusType;
        }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// 
        /// </summary>
        public TaskStatusTypes StatusType { get; }
    }
}