namespace Account.DataAccess.Entities
{
    using System;
    using Utility.DataAccess.Entity;

    public class Transaction : Auditable
    {
        public int Amount { get; set; }

        public Guid AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}