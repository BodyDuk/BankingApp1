using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Models
{
    public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public double Amount { get; set; }

        public virtual ICollection<Transaction> ReceivedTransactions { get; set; }
        public virtual ICollection<Transaction> SentTransactions { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public User(string name, string password)
        {
            Amount = 0;
            Name = name;
            Password = password;
        }
    }
}