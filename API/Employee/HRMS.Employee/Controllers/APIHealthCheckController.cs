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
    public class HealthCheckController : ControllerBase
    {
       
        [HttpGet("check")]
        public ActionResult Check()
        {
            return Ok();
        }
    }
}
