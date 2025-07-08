using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.KRA.Infrastructure.Models
{
    public class StatusModel
    {
        public int StatusId { get; set; }
        public string StatusText { get; set; }
        public string StatusDescription { get; set; }
    }
    public static class StatusConstants
    {
        public const int Draft = 1;
        public const int FinishedDrafting = 2;
        public const int SentToHOD = 3;
        public const int ApprovedbyHOD = 4;
        public const int EditedByHOD = 5;
        public const int FinishedEditByHOD = 6;
        public const int SentToHR = 7;
        public const int EditByHR = 8;
        public const int FinishedEditByHR = 9;
        public const int SendToCEO = 10;
        public const int ApprovedByCEO = 11;
        public const int RejectedByCEO = 12;
    }
}
