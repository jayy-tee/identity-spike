using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Identity.Application.Users;
using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.Infrastructure
{
    public class ModernUserRepository : IUserRepository, IDisposable
    {
        private readonly IDbConnection _db;
        private const string _connString = "Server=win10vm.local;Database=Identity_Spike;User Id=identity_spike;Password=IdentitySpikepa44word;";
        public void Dispose()
        {
            _db.Close();
        }

        public UserSource SourceSystem { get; private set; }
        public ModernUserRepository()
        {
            _db = new SqlConnection(_connString);
            SourceSystem = UserSource.New;
        }

        public async Task<User> Get(string username)
        {
            return await _db.QueryFirstOrDefaultAsync<User>(
                @"
                SELECT firstname, lastname, username, email as 'emailAddress', status, 2 AS 'source', passwordHash AS '_passwordHash',
                passwordSalt AS '_passwordSalt', status as userStatus 
                FROM dbo.Users WHERE username=@username"
                , new { @username = username }
            );
        }
        public async Task<bool> CheckExists(string username)
        {
            return await _db.QueryFirstAsync<int>("SELECT COUNT(*) FROM dbo.Users WHERE username=@username", new { @username = username }) > 0;
        }

        public async Task<bool> AddUser(User user)
        {
            if (_db.State != ConnectionState.Open)
                _db.Open();
                
            var tran = _db.BeginTransaction();
            try {
                await _db.ExecuteAsync(
                    @"INSERT INTO dbo.Users (firstname, lastname, username, email, status, passwordHash, passwordSalt)
                    VALUES(@firstname, @lastname, @username, @email, @status, @passwordHash, @passwordSalt)",
                    new {
                        @username = user.Username,
                        @firstname = user.FirstName,
                        @lastname = user.LastName,
                        @email = user.EmailAddress,
                        @passwordHash = user.GetPasswordHash(),
                        @passwordSalt = user.GetPasswordSalt(),
                        @status = user.UserStatus
                    },
                    tran);
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