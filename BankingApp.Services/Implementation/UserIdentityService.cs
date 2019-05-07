using BankingApp.Models;
using BankingApp.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BankingApp.Services.Helpful;
using BankingApp.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using BankingApp.DataAccess.UowFactory;

namespace BankingApp.Services.Implementation
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly string claimsTupeName = "UserId";
        private readonly IBankingUowFactory _bankingUow;

        public UserIdentityService(IBankingUowFactory uow) =>
            _bankingUow = uow;

        public Guid GetUserId(IEnumerable<Claim> claims) =>
         Guid.Parse(claims.First(c => c.Type == claimsTupeName).Value);

        public User IdentityUser(string username, string password)
        {
            using (var bankingUow = _bankingUow.Create())
            {
                return bankingUow.User.Get(
                    user => user.Name == username 
                    && user.Password == password).FirstOrDefault();
            }
        }

        public OperationDetails RegisterUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return OperationDetails.Error("You must enter your name");

            if (string.IsNullOrEmpty(password))
                return OperationDetails.Error("You must enter a password");

            using (var bankingUow = _bankingUow.Create())
            {
                var users = bankingUow.User.Get(us => us.Name == username);
                var user = users.FirstOrDefault();

                if (user != null)
                    return OperationDetails.Error("The name is already being used");

                bankingUow.User.Create(new User(username, password));
                bankingUow.Save();
            }

            return OperationDetails.Success(0);
        }

        public string GetIdentity(User user)
        {
            if (user == null)
                return null;

            var claims = new List<Claim>
                    {
                        new Claim(claimsTupeName, user.UserId.ToString())
                    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: Configurator.Issuer,
                    audience: Configurator.Audience,
                    notBefore: now,
                    claims: claimsIdentity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(Configurator.LifeTime)),
                    signingCredentials: new SigningCredentials(
                        Configurator.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}