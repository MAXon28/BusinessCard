using BusinessCard.DataAccessLayer.Entities;

namespace BusinessCard.DataAccessLayer.Repositories
{
    public class FactAboutMeRepository : StandardRepository<FactAboutMe>, IFactAboutMeRepository
    {
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        public FactAboutMeRepository(DbConnectionKeeper dbConnectionKeeper) : base (dbConnectionKeeper) { }
    }
}