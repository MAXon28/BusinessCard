using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <inheritdoc cref="IBiographyRepository"/>
    internal class BiographyRepository : StandardRepository<Biography>, IBiographyRepository
    {
        public BiographyRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<int> UpdateBiographyAsync(string text)
        {
            const string sqlQuery = @"update Biography
                                      set Text = @text";
            using var connection = _dbConnectionKeeper.GetDbConnection();
            return await connection.ExecuteAsync(sqlQuery, new { text });
        }
    }
}