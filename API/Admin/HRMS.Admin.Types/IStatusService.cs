using HRMS.Admin.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRMS.Admin.Infrastructure.Models.Domain;

namespace HRMS.Admin.Types
{
    public interface IStatusService
    {
        //Get Status  abstract method
        Task<List<Status>> GetAll(bool? isActive = true);

        //Get StatusByCode  abstract method

        Task<Status> GetStatusById(int statusId);

        //Get StatusByCode  abstract method
        Task<Status> GetStatusByCode(string statusCode);

        //Get GetStatusId by category and statusCode abstract method
        Task<Status> GetByCategoryAndStatusCode(string category, string statusCode);

        Task<List<Status>> GetByCategory(string category);

        Task<Status> GetByCategoryIdAndStatusCode(int categoryId, string statusCode);
        
        //Gets All project statuses
        Task<List<Status>> GetProjectStatuses();

        Task<List<StatusMaster>> GetStatusMasterDetails(bool? isActive);
    }
}
