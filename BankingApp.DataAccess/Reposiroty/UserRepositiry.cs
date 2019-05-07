using BankingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.DataAccess.Reposiroty
{
    public interface IUserRepositiry : IRepository<User>
    {
        List<User> GetAll();
    }

    public class UserRepositiry : RepositoryBase<User>, IUserRepositiry
    {
        public UserRepositiry(BankContext context) : base(context) { }

        public List<User> GetAll() => db.User.Select(s => s).ToList();
    }
}