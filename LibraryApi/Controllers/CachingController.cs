using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class CachingController : ControllerBase
    {
        private readonly ILookupOnCallDevelopers _onCalldeveloperLookup;

        public CachingController(ILookupOnCallDevelopers onCalldeveloperLookup)
        {
            _onCalldeveloperLookup = onCalldeveloperLookup;
        }

        [HttpGet("/time")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 10)]
        public ActionResult GetTheTime()
        {
            return Ok(new { TimeAtServer = DateTime.Now });
        }

        [HttpGet("/oncalldeveloper")]
        public async Task<ActionResult> GetOnCallDeveloper()
        {
            OnCallDeveloperResponse response = await _onCalldeveloperLookup.GetOnCallDeveloperAsync();
            return Ok(response);
        }

        public class OnCallDeveloperResponse
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Until { get; set; }

        }
    }
}
