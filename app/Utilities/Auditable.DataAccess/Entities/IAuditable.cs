namespace Utility.DataAccess.Entity
{
    using System;

    public interface IAuditable
    {
        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }

        DateTime? DeletedAt { get; set; }
    }
}
