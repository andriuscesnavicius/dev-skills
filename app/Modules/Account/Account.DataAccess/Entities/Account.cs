namespace Account.DataAccess.Entities
{
    using System.Collections.Generic;
    using Utility.DataAccess.Entity;

    public class Account : Auditable
    {
        public int Balance { get; set; }

        public virtual IEnumerable<Transaction> Transactions { get; set; }
    }
}
