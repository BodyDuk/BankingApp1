using BankingApp.Models;
using System;
using System.Collections.Generic;

namespace BankingApp.Services.Interface
{
    public interface IUserService
    {
        List<User> GetUsersList(Guid currentUserId);
        User GetUser(Guid UserId);
    }
}