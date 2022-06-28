using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    [SqlTable("ClickTypes")]
    public class ClickType
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ActionName { get; set; }
    }
}