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
    /// <summary>
    /// Generic service providing helper methods for executing raw SQL
    /// queries using Dapper.
    /// </summary>
    public class DbService : IDbService
    {
        private readonly IDbConnection _db;
        private IConfiguration _configuration;
        /// <summary>
        /// Initializes a new instance of <see cref="DbService"/>.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
            _db = new NpgsqlConnection(_configuration.GetConnectionString("Default"));
        }

        /// <summary>
        /// Executes a SQL query and returns the first record.
        /// </summary>
        /// <typeparam name="T">Type of record.</typeparam>
        /// <param name="command">SQL command.</param>
        /// <param name="parms">Query parameters.</param>
        /// <returns>Single record of type <typeparamref name="T"/>.</returns>
        public async Task<T> GetAsync<T>(string command, object parms)
        {
            T result;

            result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

            return result;

        }

        /// <summary>
        /// Executes a SQL query and returns all matching records.
        /// </summary>
        /// <typeparam name="T">Type of records.</typeparam>
        /// <param name="command">SQL command.</param>
        /// <param name="parms">Query parameters.</param>
        /// <returns>List of records of type <typeparamref name="T"/>.</returns>
        public async Task<List<T>> GetAll<T>(string command, object parms)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command, parms)).ToList();

            return result;
        }

        /// <summary>
        /// Executes a SQL query that does not require parameters and returns all records.
        /// </summary>
        /// <typeparam name="T">Type of records.</typeparam>
        /// <param name="command">SQL command.</param>
        /// <returns>List of records of type <typeparamref name="T"/>.</returns>
        public async Task<List<T>> GetAll<T>(string command)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command)).ToList();

            return result;
        }

        /// <summary>
        /// Executes a SQL command that modifies data.
        /// </summary>
        /// <param name="command">SQL command.</param>
        /// <param name="parms">Command parameters.</param>
        /// <returns>Number of affected rows.</returns>
        public async Task<int> EditData(string command, object parms)
        {
            int result;

            result = await _db.ExecuteAsync(command, parms);

            return result;
        }

        /// <summary>
        /// Performs bulk insert into the database using PostgreSQL copy helper.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <param name="copyHelper">Copy helper instance.</param>
        /// <param name="entities">Entities to insert.</param>
        public void WriteToDatabase<T>(PostgreSQLCopyHelper<T> copyHelper, IEnumerable<T> entities)
        {
            var connection = new NpgsqlConnection(_configuration.GetConnectionString("Default"));
            connection.Open();
            copyHelper.SaveAll(connection, entities);
            connection.Close();
        }

    }}