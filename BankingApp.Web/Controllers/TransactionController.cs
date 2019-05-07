using BankingApp.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Web.Controllers
{
    [Authorize]
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserIdentityService _userIdentityService;

        public TransactionController(ITransactionService transactionService, IUserIdentityService identityService)
        {
            _transactionService = transactionService;
            _userIdentityService = identityService;
        }

        [HttpGet("userTransactions")]
        public IActionResult GetTransactions() =>
            Ok(_transactionService.GetByUser(_userIdentityService.GetUserId(User.Claims)));
    }
}