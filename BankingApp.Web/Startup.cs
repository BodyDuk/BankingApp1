using System;
using BankingApp.DataAccessLayer.UOW;
using DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace TestTask1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async System.Threading.Tasks.Task ConfigureAsync(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //using (BankContext db = new BankContext())
            //{
            //    User user1 = new User { Name = "Bohdan", Money = 0, UserID = Guid.NewGuid(), TimeStemp = DateTime.Now };

            //    db.User.Add(user1);

            //    db.SaveChanges();
            //}


            BankingUOW bankingUOW = new BankingUOW();

            User user =  await bankingUOW.User.Get(Guid.Parse("0475cfbc-45f7-43e4-b9de-32410bb6f28e"));

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
