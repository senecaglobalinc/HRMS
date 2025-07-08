using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Employee.Types
{
   public interface IAssociateLeaveService
    {
         Task<ServiceResponse<bool>> UploadLeaveData(IFormFile file);
        FileDetail GetTemplateFile();
    }
}
