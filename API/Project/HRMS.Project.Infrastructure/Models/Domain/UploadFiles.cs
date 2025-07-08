using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.AspNetCore.Http;

namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class UploadFiles
    {
        public int ProjectId { get; set; }
        public string FileType { get; set; }
        public string ClientFeedbackFile { get; set; }
        public string DeliveryPerformanceFile { get; set; }
        public IFormFile UploadedFiles { get; set; }
    }
}
