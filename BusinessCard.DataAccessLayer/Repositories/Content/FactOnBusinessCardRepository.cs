using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class FactOnBusinessCardRepository : StandardRepository<FactOnBusinessCard>, IFactOnBusinessCardRepository
    {
        public FactOnBusinessCardRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}