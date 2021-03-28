using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookcaseApi.helpers
{
    public class CustomFilterToAction : IActionFilter
    {
        private readonly ILogger<CustomFilterToAction> _logger;

        public CustomFilterToAction(ILogger<CustomFilterToAction> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogError("OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogError("OnActionExecuting");
        }
    }
}
