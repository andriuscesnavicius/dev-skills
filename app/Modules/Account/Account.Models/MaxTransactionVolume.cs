namespace Account.Models
{
    using System;

    public class MaxTransactionVolume
    {
        public int MaxVolume { get; set; }

        public Guid[] Accounts { get; set; }
    }
}
