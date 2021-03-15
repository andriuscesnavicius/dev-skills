namespace Account.API.Infrastructure
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Account.DataAccess;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

    public class AccountDbContextMigrations
    {
        public static void UpdateDbContext(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var accountDbContext = (AccountDbContext)scope.ServiceProvider.GetService(typeof(AccountDbContext));
            if (env.IsDevelopment())
            {
                accountDbContext.Database.EnsureDeleted();
            }

            accountDbContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                string accountsJson = File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/accounts.json");
                var accounts = JsonConvert.DeserializeObject<List<DataAccess.Entities.Account>>(accountsJson);
                accounts.ForEach(account => accountDbContext.Accounts.Add(account));
                accountDbContext.SaveChanges();
            }
        }
    }
}
