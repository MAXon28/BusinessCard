using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog.PostDetails
{
    /// <summary>
    /// Данные поста
    /// </summary>
    [SqlTable("PostElements")]
    public class PostElement 
    {
        /// <summary>
        /// Идентификатор данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Позиция элемента в посте
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Идентификатор соответствующего поля
        /// </summary>
        [SqlForeignKey("PostFields")]
        public int FieldId { get; set; }

        /// <summary>
        /// Уникальный текстовый ключ поста
        /// </summary>
        [SqlForeignKey("Posts")]
        public string PostKey { get; set; }

        /// <summary>
        /// Наименование поля элемента
        /// </summary>
        [NotSqlColumn]
        public string FieldName { get; set; }
    }
}