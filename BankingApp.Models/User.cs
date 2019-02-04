using System;
using System.Collections.Generic;

namespace Models
{
    public class User
    {
        public Guid UserID { get; set; }

        public string Name { get; set; }

        public Int64 Amount { get; set; }

        public List<Transaction> Transactions { get; set; }

        public DateTime TimeStemp { get; set; }
    }
}