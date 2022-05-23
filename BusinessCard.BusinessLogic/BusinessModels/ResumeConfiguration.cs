using System;

namespace BusinessCard.BusinessLogicLayer.BusinessModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ResumeConfiguration
    {
        public ResumeConfiguration() { }

        /// <summary>
        /// 
        /// </summary>
        public bool NeedWork { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentResumeId { get; set; }
    }
}