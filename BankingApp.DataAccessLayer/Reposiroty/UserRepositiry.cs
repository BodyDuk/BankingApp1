using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Reposiroty
{
    public class UserRepositiry : IRepositoryUser<User>
    {
        private BankContext db;

        public UserRepositiry(BankContext context)
        {
            this.db = context;
        }

        public void Create(User item)
        {
            db.User.Add(item);
        }

        public async Task<User> Get(Guid id)
        {
             return await db.User.FirstOrDefaultAsync(user => user.UserID == id);
        }

        public IEnumerable<User> GetAll()
        {
            return db.User;
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
