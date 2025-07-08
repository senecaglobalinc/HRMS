using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IDepartmentService
    {
        Task<dynamic> Create(Department department);
        Task<dynamic> Update(Department department);
        Task<IEnumerable<Object>> GetAll(bool isActive = true);
        Task<ServiceListResponse<DepartmentDetails>> GetUserDepartmentDetails();
        Task<ServiceListResponse<DepartmentDetails>> GetUserDepartmentDetailsByEmployeeID(int employeeId);
        Task<ServiceListResponse<MasterDetails>> GetMasterTablesData();
        Task<Department> GetByDepartmentCode(string departmentCode);
        Task<List<Department>> GetByDepartmentCodes(string departmentCodes);
        Task<Department> GetById(int departmentId);
        Task<DepartmentDL> GetDepartmentDLByDeptId(int deptId);
        Task<List<DepartmentWithDLAddress>> GetAllDepartmentDLs();
        Task<List<Department>> GetAllDepartments(bool isActive=true);
    }
}
