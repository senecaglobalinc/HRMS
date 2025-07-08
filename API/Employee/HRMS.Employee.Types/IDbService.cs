using Npgsql;
using PostgreSQLCopyHelper;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
    public interface IDbService
    {
        Task<T> GetAsync<T>(string command, object parms);
        Task<List<T>> GetAll<T>(string command, object parms);
        Task<List<T>> GetAll<T>(string command);
        Task<int> EditData(string command, object parms);
        void WriteToDatabase<T>(PostgreSQLCopyHelper<T> copyHelper, IEnumerable<T> entities);
    }
}