using BankingApp.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using BankingApp.Services.Interface;
using BankingApp.DataAccess.UowFactory;

namespace BankingApp.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IBankingUowFactory _bankingUow;

        public UserService(IBankingUowFactory uow) =>
            _bankingUow = uow;

        public User GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
                return null;

            using (var bankingUow = _bankingUow.Create())
            {
                return bankingUow.User.GetById(userId);
            }  
        }

        public List<User> GetUsersList(Guid currentUserId)
        {
            using (var bankingUow = _bankingUow.Create())
            {
                return bankingUow.User.Get(u => u.UserId != currentUserId).ToList();
            }
        }
    }
}