using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class BiographyRepository : StandardRepository<Biography>, IBiographyRepository
    {
        public BiographyRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<Biography>> GetSortedBiographyByPriorityAsync()
        {
            const string sqlQuery = @"SELECT * 
                                      FROM Biography
                                      ORDER BY Priority";


            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Biography>(sqlQuery);
        }
    }
}