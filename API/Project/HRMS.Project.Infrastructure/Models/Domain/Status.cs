namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Status
    {
        public int Id { get; set; }

        public int StatusId { get; set; }

        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public int? CategoryMasterId { get; set; }
    }
}
