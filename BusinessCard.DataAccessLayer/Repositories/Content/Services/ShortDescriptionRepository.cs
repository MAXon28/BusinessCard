using BusinessCard.DataAccessLayer.Entities.Content.Services;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ShortDescriptionRepository : StandardRepository<ShortDescription>, IShortDescriptionRepository
    {
        public ShortDescriptionRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}