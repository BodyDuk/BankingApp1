using BankingApp.Models;
using BankingApp.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BankingApp.Services.Interface
{
    public interface IUserIdentityService 
    {
        User IdentityUser(string username, string password);
        OperationDetails RegisterUser(string username, string password);
        string GetIdentity(User user);
        Guid GetUserId(IEnumerable<Claim> claims);
    }
}
