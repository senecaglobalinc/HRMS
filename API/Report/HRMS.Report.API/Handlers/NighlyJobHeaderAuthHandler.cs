using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Report.API.Handlers
{
    public class NighlyJobHeaderAuthHandler : AuthorizationHandler<NightJobHeaderAuthRequirement>
    {
        private readonly IHttpContextAccessor m_httpContextAccessor = null;

        public NighlyJobHeaderAuthHandler(IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NightJobHeaderAuthRequirement authRequirement)
        {
            HttpRequest httpRequest = m_httpContextAccessor.HttpContext.Request; // Access context here
            string nightjobHeaderName = authRequirement.NightjobHeader.ToLower().Trim();

            // Check for header to have specific key from night job. If present, authorizing request and propagating it to next service for validation
            if (httpRequest.Headers.ContainsKey(nightjobHeaderName))
            {
                context.Succeed(authRequirement);
            }

            return Task.FromResult(0);
        }
    }
}

namespace HRMS.Report.API.Handlers
{
    public class NightJobHeaderAuthRequirement : IAuthorizationRequirement
    {
        public string NightjobHeader { get; set; }

        public NightJobHeaderAuthRequirement(string nightjobheader)
        {
            NightjobHeader = nightjobheader;
        }
    }
}
