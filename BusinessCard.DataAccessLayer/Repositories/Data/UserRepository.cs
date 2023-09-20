using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using Dapper;
using DapperAssistant;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <inheritdoc cref="IUserRepository"/>
    internal class UserRepository : StandardRepository<User>, IUserRepository
    {
        public UserRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<User> GetSmallUserDataAsync(int userId)
        {
            var sqlQuery = @"SELECT u.Id,
                                    u.Surname,
                                    u.Name,
                                    r.Name AS RoleName
                             FROM Users u
                                INNER JOIN Roles r
                                ON u.RoleId = r.Id
                             WHERE u.Id = @userId";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<User, string, User>(
                    sqlQuery,
                    (user, roleName) =>
                    {
                        user.RoleName = roleName;
                        return user;
                    },
                    new { userId },
                    splitOn: "RoleName")).FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateUserProfileAsync(int userId, string surname, string name, string middleName, string newEmail, string newPhoneNumber)
        {
            var sqlQuery = @"UPDATE Users
                             SET Surname = @surname, Name = @name, MiddleName = @middleName, Email = @newEmail, PhoneNumber = @newPhoneNumber
                             WHERE Id = @userId";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, new { userId, surname, name, middleName, newEmail, newPhoneNumber }) == 1;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateUserPasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var sqlQuery = @"UPDATE Users
                             SET Password = @newPassword
                             WHERE Id = @userId AND Password = @currentPassword";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, new { userId, currentPassword, newPassword }) == 1;
        }
    }
}