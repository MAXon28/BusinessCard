using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("JobOpenings")]
    public class Vacancy
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Schedule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SalaryFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? SalaryTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}