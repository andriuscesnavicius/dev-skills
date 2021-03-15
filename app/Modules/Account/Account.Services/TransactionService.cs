namespace Account.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Account.DataAccess;
    using Account.Models;
    using Lock.DataAccess;
    using Microsoft.EntityFrameworkCore;

    public class TransactionService : ITransactionService
    {
        private readonly AccountDbContext accountDbContext;

        public TransactionService(AccountDbContext accountDbContext)
        {
            this.accountDbContext = accountDbContext;
        }

        public async Task CreateTransaction(Transaction transaction, Guid transactionId)
        {
            using var itemLock = new EntityLock(transaction.AccountId, typeof(DataAccess.Entities.Account));
            var transactionEntity = await accountDbContext.Transactions
                  .AsNoTracking()
                  .FirstOrDefaultAsync(x => x.Id == transactionId);
            if (transactionEntity != null)
            {
                return;
            }

            var account = await accountDbContext.Accounts
                .FirstOrDefaultAsync(x => x.Id == transaction.AccountId);
            if (account == null)
            {
                throw new ValidationException();
            }

            account.Balance += transaction.Amount.Value;
            accountDbContext.Transactions.Add(new DataAccess.Entities.Transaction()
            {
                Id = transactionId,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount.Value
            });

            await accountDbContext.SaveChangesAsync();
        }

        public async Task<AccountBalance> GetBalance(Guid accountId)
        {
            return await accountDbContext.Accounts
              .AsNoTracking()
              .Where(x => x.Id == accountId)
              .Select(x => new AccountBalance()
              {
                  Balance = x.Balance
              })
              .FirstOrDefaultAsync();
        }

        public async Task<MaxTransactionVolume> GetMaxTransactionVolume()
        {
            var accountTransactionCount = await accountDbContext.Transactions
                 .AsNoTracking()
                 .GroupBy(x => x.AccountId)
                 .Select(group => new
                 {
                     AccountId = group.Key,
                     Count = group.Count()
                 })
                 .ToListAsync();

            return accountTransactionCount
                .GroupBy(x => x.Count)
                .Select(group => new MaxTransactionVolume()
                {
                    Accounts = group.Select(x => x.AccountId).ToArray(),
                    MaxVolume = group.Key
                })
                .OrderByDescending(x => x.MaxVolume)
                .FirstOrDefault();
        }

        public async Task<Transaction> GetTransaction(Guid transactionId)
        {
            return await accountDbContext.Transactions
                 .AsNoTracking()
                 .Where(x => x.Id == transactionId)
                 .Select(x => new Transaction()
                 {
                     AccountId = x.AccountId,
                     Amount = x.Amount
                 })
                 .FirstOrDefaultAsync();
        }
    }
}
