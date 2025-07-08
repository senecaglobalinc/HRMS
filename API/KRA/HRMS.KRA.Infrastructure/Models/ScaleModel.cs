using System.Collections.Generic;

namespace HRMS.KRA.Infrastructure.Models
{
    public class ScaleModel
    {
        public int ScaleID { get; set; }
        public int MinimumScale { get; set; }
        public int MaximumScale { get; set; }
        public string ScaleTitle { get; set; }
        public List<ScaleDetailsModel> ScaleDetails { get; set; }
    }

    public class ScaleDetailsModel
    {
        public int ScaleDetailId { get; set; }
        public int ScaleID { get; set; }
        public int ScaleValue { get; set; }
        public string ScaleDescription { get; set; }
    }
}
