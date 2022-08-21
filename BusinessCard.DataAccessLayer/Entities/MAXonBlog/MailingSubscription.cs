using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("MailingSubscriptions")]
    public class MailingSubscription
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Users")]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Channels")]
        public int ChannelId { get; set; }
    }
}