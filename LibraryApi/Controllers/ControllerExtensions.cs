using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult Maybe<T>(this ControllerBase controller, T obj)
        {
            if (obj == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new OkObjectResult(obj);
            }
        }
    }
}
