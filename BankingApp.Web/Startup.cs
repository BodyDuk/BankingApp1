using BankingApp.Services.Implementation;
using BankingApp.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using BankingApp.Services.Helpful;
using BankingApp.DataAccess.Uow;
using BankingApp.DataAccess.Reposiroty;
using BankingApp.DataAccess.UowFactory;

namespace BankingApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepositiry, UserRepositiry>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IBankingUow, BankingUow>();
            services.AddScoped<IBankingUowFactory, BankingUowFactory>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<IBankingService, BankingService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configurator.Issuer,
                            ValidateAudience = true,
                            ValidAudience = Configurator.Audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = Configurator.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddMvc();
            services.AddCors(options => options.AddPolicy("AngularCors", builder => builder
                  .WithOrigins(Configurator.NgUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("AngularCors");
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=User}/{action=Get}/{id?}");
            });
        }
    }
}