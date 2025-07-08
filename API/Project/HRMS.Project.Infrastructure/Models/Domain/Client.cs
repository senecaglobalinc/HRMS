namespace HRMS.Project.Infrastructure.Models.Domain
{
    public class Client
    {
        public int ClientId { get; set; }

        public string ClientCode { get; set; }

        public string ClientName { get; set; }

        public string ClientRegisterName { get; set; }

        public string ClientNameHash { get; set; }
    }
}
