using SqlQueryStrings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer
{
    public abstract class StandardRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        private readonly string _insertQuery;

        private readonly string _selectAllQuery;

        private readonly string _selectQuery;

        private readonly string _updateQuery;

        private readonly string _deleteQuery;

        public StandardRepository(DbConnectionKeeper dbConnectionKeeper)
        {
            _dbConnectionKeeper = dbConnectionKeeper;

            var queriesDictionary = SqlQueryBuilder.GetQueries(typeof(TEntity));

            _insertQuery = queriesDictionary["INSERT"];
            _selectAllQuery = queriesDictionary["SELECT_ALL"];
            _selectQuery = queriesDictionary["SELECT"];
            _updateQuery = queriesDictionary["UPDATE"];
            _deleteQuery = queriesDictionary["DELETE"];
        }

        public async virtual Task<int> AddAsync(TEntity newData)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            var inserted = await dbConnection.InsertAsync(newData);

            return 1;
        }

        public async virtual Task<List<TEntity>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async virtual Task<TEntity> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async virtual Task UpdateAsync(int id, TEntity updateData)
        {
            throw new System.NotImplementedException();
        }

        public async virtual Task<bool> DeleteAsync(int id)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            var type = typeof(TEntity);

            //Console.WriteLine(type.ToString());

            //var entity = await dbConnection.GetAsync<TEntity>(id);


            //return await dbConnection.DeleteAsync(entity);
        }

        private void SetInsertQuery(string entityName)
        {

        }
    }
}