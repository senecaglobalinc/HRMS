using HRMS.Admin.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRMS.Admin.API.Controllers
{
    [Controller]
    [Route("admin/[controller]")]
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
