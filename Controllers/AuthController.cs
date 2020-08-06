using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hz.IdentityServer.Models;

namespace Hz.IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        public IActionResult Authorize([FromHeader]AuthorizeOptions options)
        {
            ViewData["options"] = options;
            return View();
        }
    }
}