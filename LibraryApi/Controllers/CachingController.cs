using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class CachingController : ControllerBase
    {
        [HttpGet("/time")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public ActionResult GetTheTime()
        {
            return Ok(new { TimeAtServer = DateTime.Now });
        }
    }
}
