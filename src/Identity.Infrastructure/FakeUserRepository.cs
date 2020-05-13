using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Identity.Application.Users;
using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.Infrastructure
{
    public class FakeUserRepository : IUserRepository, IDisposable
    {
        private readonly IDbConnection _db;
        private const string _connString = "host=127.0.0.1;port=13306;user id=root;password=123456;database=spike;";
        public void Dispose()
        {
            _db.Close();
        }

        public UserSource SourceSystem { get; private set; }
        public FakeUserRepository()
        {
            _db = new MySqlConnection(_connString);
            SourceSystem = UserSource.Legacy;
        }

        public async Task<User> Get(string username)
        {
            return await _db.QueryFirstOrDefaultAsync<User>(
                @"
                SELECT firstname, lastname, username, email as 'emailAddress', status, 1 AS 'source', passwordHash AS '_passwordHash', status as userStatus 
                FROM user WHERE username=@username"
                , new { @username = username }
            );
        }
        public async Task<bool> CheckExists(string username)
        {
            return await _db.QueryFirstAsync<int>("SELECT COUNT(*) FROM user WHERE username=@username", new { @username = username }) > 0;
        }

        public async Task<bool> AddUser(User user)
        {
            _db.Open();
            var tran = _db.BeginTransaction();
            try {
                await _db.ExecuteAsync(
                    @"INSERT INTO user (firstname, lastname, username, email, status, passwordHash)
                    VALUES(@firstname, @lastname, @username, @email, @status, @passwordHash)",
                    new {
                        @username = user.Username,
                        @firstname = user.FirstName,
                        @lastname = user.LastName,
                        @email = user.EmailAddress,
                        @passwordHash = user.GetPasswordHash(),
                        @status = user.UserStatus
                    });
                tran.Commit();
                return true;
            } 
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

    }
}