namespace Account.DataAccess
{
    using Account.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;

    public class AccountDbContext : AuditableDbContext
    {
        public AccountDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
