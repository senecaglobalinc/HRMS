using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    /// <summary>
    /// Interface for client service
    /// </summary>
    public interface IClientService
    {
        //Create client abstract method 
        Task<dynamic> Create(Client clientIn);

        //Update client abstract method
        Task<dynamic> Update(Client clientIn);

        //Get Client details abstract method
        Task<List<Client>> GetAll(bool isActive = true);

        //Get Client details abstract method
        Task<List<Client>> GetByIds(int[] clientIds);

        //Get client details by id abstract method
        Task<Client> GetById(int clientId);

        //Get client details by Name abstract method
        Task<Client> GetByName(string clientName);
        Task<List<GenericType>> GetClientsForDropdown();
    }
}
