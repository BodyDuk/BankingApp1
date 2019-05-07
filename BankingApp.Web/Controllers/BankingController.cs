using BankingApp.ModelsDTO;
using BankingApp.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Web.Controllers
{
    [Authorize]
    [Route("Banking")]
    public class BankingController : Controller
    {
        private readonly IBankingService _bankingService;
        private readonly IUserIdentityService _userIdentityService;

        public BankingController(IBankingService service, IUserIdentityService identityService )
        {
            _bankingService = service;
            _userIdentityService = identityService;
        }

        [HttpPost]
        [Route("transfer")]
        public IActionResult Transfer([FromBody]BankOperation bankOperation)
        {
            if (bankOperation == null || bankOperation.Amount < _bankingService.GetBankOperationMinAmoun())
                return BadRequest(OperationDetails.Error("Transfer error"));

            bankOperation.SenderId = _userIdentityService.GetUserId(User.Claims);
            var result = _bankingService.Transfer(bankOperation);
            return (result.Succeeded == true ? (IActionResult)Ok(result) : BadRequest(result));
        }

        [HttpPost]
        [Route("deposit")]
        public IActionResult Deposit([FromBody]double amount)
        {
            if (amount < _bankingService.GetBankOperationMinAmoun())
                return BadRequest(OperationDetails.Error("Deposit error"));

            var result = _bankingService.Deposit(new BankOperation(_userIdentityService.GetUserId(User.Claims), amount));
            return (result.Succeeded == true ? (IActionResult)Ok(result) : BadRequest(result));
        }

        [HttpPost]
        [Route("withdraw")]
        public IActionResult Withdraw([FromBody]double amount)
        {
            if (amount < _bankingService.GetBankOperationMinAmoun())
                return BadRequest(OperationDetails.Error("Withdraw error"));

            var result = _bankingService.Withdraw(new BankOperation(_userIdentityService.GetUserId(User.Claims), amount));
            return (result.Succeeded == true ? (IActionResult)Ok(result) : BadRequest(result));
        }
    }
}