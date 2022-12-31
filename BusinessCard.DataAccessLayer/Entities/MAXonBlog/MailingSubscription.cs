using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog
{
    /// <summary>
    /// Подписка на рассылку
    /// </summary>
    [SqlTable("MailingSubscriptions")]
    public class MailingSubscription : Subscription { }
}