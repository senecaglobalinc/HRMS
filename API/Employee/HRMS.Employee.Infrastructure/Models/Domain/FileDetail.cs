using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Employee.Infrastructure.Models.Domain
{
    public class FileDetail
    {        
        public string FileName { get; set; }
        public byte[] FileContent { set; get; }
        public string Content { set; get; }
        public string FileType { set; get; }
    }
}
