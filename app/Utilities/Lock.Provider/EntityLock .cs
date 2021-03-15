namespace Lock.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class EntityLock : IDisposable
    {
        private static readonly object ListLock = new object();
        private static readonly List<EntityLock> Locks = new List<EntityLock>();

        private readonly Tuple<Guid, Type> name;
        private readonly object entityLock;

        public EntityLock(Guid entityId, Type entityType)
        {
            name = Tuple.Create(entityId, entityType);
            lock (ListLock)
            {
                var existing = Locks.Find(l => l.name.Equals(name));
                entityLock = existing == null ? new object() : existing.entityLock;
                Locks.Add(this);
            }

            Monitor.Enter(entityLock);
        }

        public void Dispose()
        {
            if (Monitor.IsEntered(entityLock))
            {
                Monitor.Exit(entityLock);
            }

            lock (ListLock)
            {
                Locks.Remove(this);
            }
        }
    }
}
