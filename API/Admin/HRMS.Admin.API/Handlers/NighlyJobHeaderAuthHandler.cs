using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Handlers
{
    public class NighlyJobHeaderAuthHandler : AuthorizationHandler<NightlyjobAuthRequirement>
    {
        private const char splitOperator = ',';
        private readonly IHttpContextAccessor m_httpContextAccessor = null;
        private StringValues nightjobnames;

        public NighlyJobHeaderAuthHandler(/*IConfiguration configuration,*/ IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NightlyjobAuthRequirement authRequirement)
        {
            HttpRequest httpRequest = m_httpContextAccessor.HttpContext.Request; // Access context here
            string nightJobHeaderName = authRequirement.NightJobHeader.ToLower().Trim();
            string[] nightJobClients = authRequirement.NightJobClients?.Split(splitOperator);

            // Check for header to have specific key propagated from night job. If present, validate request with configured list of nightjob names
            if (httpRequest.Headers.ContainsKey(nightJobHeaderName))
            {
                _ = httpRequest.Headers.TryGetValue(nightJobHeaderName, out nightjobnames);
                if (nightJobClients != null && nightjobnames.Count() > 0 && nightJobClients.Any(x => x.ToLower().Trim() == nightjobnames[0]?.ToLower().Trim()))
                {
                    context.Succeed(authRequirement);
                }
            }

            return Task.FromResult(0);
        }
    }

    public class NightlyjobAuthRequirement : IAuthorizationRequirement
    {
        public string NightJobHeader { get; set; }
        public string NightJobClients { get; set; }

        public NightlyjobAuthRequirement(string nightJobHeader, string nightJobClients)
        {
            NightJobHeader = nightJobHeader;
            NightJobClients = nightJobClients;
        }
    }
}
