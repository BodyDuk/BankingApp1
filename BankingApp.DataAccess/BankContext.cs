using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BankingApp.Models;
using System.IO;

namespace BankingApp.DataAccess
{
    public class BankContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

        public BankContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(tr => {

                tr.HasOne(sent => sent.SenderUser)
                .WithMany(us => us.SentTransactions)
                .HasForeignKey(fk => fk.SenderId)
                .OnDelete(DeleteBehavior.SetNull);

                tr.HasOne(rec => rec.RecipientUser)
               .WithMany(us => us.ReceivedTransactions)
               .HasForeignKey(fk => fk.RecipientId)
               .OnDelete(DeleteBehavior.SetNull); ;
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}