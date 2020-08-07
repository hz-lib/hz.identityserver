using System;
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
            // 验证clientid
            if(!string.IsNullOrWhiteSpace(options.client_id)) {
                ViewData["orgName"] = "xxx机构";
            }
            ViewData["options"] = options;
            return View();
        }

        [HttpPost]
        public IActionResult Submit([FromBody]LoginModel model)
        {
            if(model.account == "admin" && model.passwd == "123456")
            {
                var code =  Guid.NewGuid().ToString("N");
                if ("token".Equals(model.response_type)) {
                    // 保存到缓存
                }
                return Ok(code);
            }
            else {
                return RedirectToAction("Error","Home");
            }
        }
    }
}