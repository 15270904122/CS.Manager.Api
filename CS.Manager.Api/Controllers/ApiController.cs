using CS.Manager.Api.Authorization;
using CS.Manager.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Manager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiExplorerSettings, Authorize]
    public class ApiController : ControllerBase
    {

    }
}
