using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;

namespace BankingApp.Services.Helpful
{
    public class Configurator
    {
        public static string NgUrl => GetConfig("FRONTENDURL");

        public static string Issuer => GetConfig("ISSUER");

        public static string Audience => GetConfig("AUDIENCE");

        public static string Key => GetConfig("KEY");

        public static int LifeTime => Convert.ToInt32(GetConfig("LIFETIME"));

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));

        private static string GetConfig(string cofigName)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();

            return config.GetValue<string>(cofigName);
        }
    }
}