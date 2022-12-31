using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog.PostDetails
{
    /// <summary>
    /// Поля постов
    /// </summary>
    [SqlTable("PostFields")]
    public class PostField
    {
        /// <summary>
        /// Идентификатор поля
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование поля
        /// </summary>
        public string Name { get; set; }
    }
}