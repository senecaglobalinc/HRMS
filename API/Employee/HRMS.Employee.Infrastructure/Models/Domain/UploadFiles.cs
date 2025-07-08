using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class UploadFiles
    {
        public int EmployeeId { get; set; }
        public string FileName {get; set;}
        public List<IFormFile> UploadedFiles { get; set; }
    }
}
