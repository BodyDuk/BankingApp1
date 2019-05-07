using BankingApp.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Web.Controllers
{
    [Authorize]
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;

        public UserController(IUserService service, IUserIdentityService userIdentityService)
        {
            _userService = service;
            _userIdentityService = userIdentityService;
        }

        [HttpGet("userProfile")]
        public IActionResult GetUser()
        {
            return Ok(_userService.GetUser(_userIdentityService.GetUserId(User.Claims))) ;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(_userService.GetUsersList(_userIdentityService.GetUserId(User.Claims)));
        }
    }
}