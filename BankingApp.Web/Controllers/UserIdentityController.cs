using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankingApp.ModelsDTO;
using BankingApp.Services.Interface;

namespace BankingApp.Web.Controllers
{
    [AllowAnonymous]
    [Route("UserIdentity")]
    public class UserIdentityController : Controller
    {
        private readonly IUserIdentityService _userIdentityService;

        public UserIdentityController(IUserIdentityService service)
        {
            _userIdentityService = service;
        }

        [HttpPost("authenticate")]
        public IActionResult Login([FromBody] Identity identity)
        {
            if (identity != null)
            {
                var user = _userIdentityService.IdentityUser(identity.Name, identity.Password);

                if (user == null)
                    return BadRequest(new { message = "User name or password is incorrect." });

                var token = _userIdentityService.GetIdentity(user);

                if (token != null)
                    return Ok(new { token });
            }

            return BadRequest(new { message = "User name or password is incorrect." });
        }

        [HttpPost("registerUser")]
        public IActionResult RegisterUser([FromBody]Identity identity)
        {
            if (identity == null)
                return BadRequest(OperationDetails.Error("Registration error"));

            var result = _userIdentityService.RegisterUser(identity.Name, identity.Password);
            return result.Succeeded == true ? (IActionResult)Ok(result) : BadRequest(result);
        }
    }
}