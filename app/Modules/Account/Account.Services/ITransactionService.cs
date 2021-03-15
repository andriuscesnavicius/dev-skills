namespace Account.Services
{
    using System;
    using System.Threading.Tasks;
    using Account.Models;

    public interface ITransactionService
    {
        public Task CreateTransaction(Transaction transaction, Guid transactionId);

        public Task<Transaction> GetTransaction(Guid transactionId);

        public Task<AccountBalance> GetBalance(Guid accountId);

        public Task<MaxTransactionVolume> GetMaxTransactionVolume();
    }
}
