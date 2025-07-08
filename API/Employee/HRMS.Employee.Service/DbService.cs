using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PostgreSQLCopyHelper;
using HRMS.Employee.Types;

namespace HRMS.Employee.Service
{
    public class DbService : IDbService
    {
        private readonly IDbConnection _db;
        private IConfiguration _configuration;
        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new NpgsqlConnection(_configuration.GetConnectionString("Default"));
        }

        public async Task<T> GetAsync<T>(string command, object parms)
        {
            T result;

            result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

            return result;

        }

        public async Task<List<T>> GetAll<T>(string command, object parms)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command, parms)).ToList();

            return result;
        }

        public async Task<List<T>> GetAll<T>(string command)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command)).ToList();

            return result;
        }

        public async Task<int> EditData(string command, object parms)
        {
            int result;

            result = await _db.ExecuteAsync(command, parms);

            return result;
        }

        public void WriteToDatabase<T>(PostgreSQLCopyHelper<T> copyHelper, IEnumerable<T> entities)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("Default"));
            connection.Open();
            copyHelper.SaveAll(connection, entities);
            connection.Close();
        }

    }
}