using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Identity.Application.Users;
using Identity.Common;
using Identity.Domain.UserAggregate;
using Identity.Infrastructure.Config;

namespace Identity.Infrastructure
{
    public class LegacyUserRepository : IUserRepository, IDisposable
    {
        private readonly IDbConnection _db;
        
        public void Dispose()
        {
            _db.Close();
        }

        public UserSource SourceSystem { get; private set; }
        public LegacyUserRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _db = new MySqlConnection(connectionStrings.Value.LegacyUsers);
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

            if (_db.State != ConnectionState.Open)
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