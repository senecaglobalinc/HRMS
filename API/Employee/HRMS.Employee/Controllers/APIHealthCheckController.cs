using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRMS.Employee.API.Controllers
{
    [Controller]
    [Route("employee/[controller]")]
    [ApiController]
    /// <summary>
    /// Controller used for basic API health checks.
    /// </summary>
    public class HealthCheckController : ControllerBase
    {

        [HttpGet("check")]
        /// <summary>
        /// Simple endpoint to verify the API is responding.
        /// </summary>
        /// <returns>HTTP 200 when the service is healthy.</returns>
        public ActionResult Check()
        {
            return Ok();
        }
    }
}
