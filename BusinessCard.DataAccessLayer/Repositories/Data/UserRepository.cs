using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <inheritdoc cref="IUserRepository"/>
    public class UserRepository : StandardRepository<User>, IUserRepository
    {
        public UserRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}