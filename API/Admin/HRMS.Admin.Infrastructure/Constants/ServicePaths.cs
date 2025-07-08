using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Admin.Infrastructure.Constants
{
    public static class ServicePaths
    {
        #region EmployeeEndPoint
        public static class EmployeeEndPoint
        {
            public const string GETALL = "Employee/GetAll?isActive=";
            public const string GETEMPLOYEESBYSEARCHSTRING = "Employee/GetEmployeeBySearchString/";
            public const string GETEMPLOYEEBYUSERID = "Employee/GetByUserId/";
            public const string GETEMPLOYEEBYIDS = "Employee/GetByIds?employeeIds=";
        }
        #endregion

    }
}
