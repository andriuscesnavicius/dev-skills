namespace Utility.DataAccess.Entity
{
    using System;

    public abstract class Auditable : IAuditable
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
