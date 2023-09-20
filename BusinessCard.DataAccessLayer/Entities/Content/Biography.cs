using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// Биография
    /// </summary>
    [NeedInsertId]
    public class Biography
    {
        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }
    }
}