using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using Dapper;
using DapperAssistant;
using System.Data;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskRepository : StandardRepository<TaskCard>, ITaskRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        public TaskRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) => _dbConnectionKeeper = dbConnectionKeeper;

        public async Task<string> AddTask(TaskCard taskCard, int serviceId)
        {
            const string sqlProcedureName = "dbo.GetNextTaskNumberDetails";

            var parameters = new DynamicParameters();
            parameters.Add("@ServiceId", serviceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ServiceAbbreviation", SqlDbType.VarChar, size: 5, dbType: DbType.StringFixedLength, direction: ParameterDirection.Output);
            parameters.Add("@NextTaskNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var dbConnection = _dbConnectionKeeper.GetDbConnection())
            {
                using var transaction = dbConnection.BeginTransaction();
                await dbConnection.ExecuteAsync(sqlProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                taskCard.TaskNumber = parameters.Get<string>("@ServiceAbbreviation").Replace(" ", "") + parameters.Get<int>("@NextTaskNumber");
                await AddAsync(taskCard, dbConnection, transaction);
                transaction.Commit();
            }

            return taskCard.TaskNumber;
        }
    }
}