using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class CachingController : ControllerBase
    {
        private readonly ILookupOnCallDevelopers _onCalldeveloperLookup;
        private readonly IOptions<ProductInfoOptions> _productInfoOptions;
        private readonly ILogger<CachingController> _logger;

        public CachingController(ILookupOnCallDevelopers onCalldeveloperLookup, IOptions<ProductInfoOptions> productInfoOptions)
        {
            _onCalldeveloperLookup = onCalldeveloperLookup;
            _productInfoOptions = productInfoOptions;
        }

        [HttpGet("/productinfo")]
        public ActionResult GetSomething()
        {
            _logger.LogDebug("Reading the configuration");
            return Ok($"Allowing Backorders: {_productInfoOptions.Value.BackOrderAllowed} " +
                $"- Markup { _productInfoOptions.Value.Markup}");
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
