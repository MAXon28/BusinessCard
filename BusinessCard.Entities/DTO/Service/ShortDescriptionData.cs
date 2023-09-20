namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Данные краткого описания сервиса
    /// </summary>
    public class ShortDescriptionData
    {
        /// <summary>
        /// Идентификатор описания
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Описание сервиса
        /// </summary>
        public string Text { get; set; }
    }
}