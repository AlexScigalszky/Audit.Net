using Audit.Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Audit.Net
{
    public class Auditor
    {
        private readonly DbContext context;
        private readonly Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker;
        private readonly IUser user;
        private readonly DateTime now;
        private EntityEntry[] added;
        private EntityEntry[] modified;
        private EntityEntry[] deleted;

        public Auditor(DbContext context, IUser user, Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker changeTracker)
        {
            this.context = context;
            this.changeTracker = changeTracker;
            now = DateTime.UtcNow;
            this.user = user;
        }


        public void Prepare()
        {
            if (user == null)
            {
                return;
            }
            added = changeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is AuditModel)
                .ToArray();

            modified = changeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is AuditModel)
                .ToArray();

            deleted = changeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is AuditModel)
                .ToArray();
        }

        public void Complete()
        {
            if (user == null)
            {
                return;
            }
            AuditEntities(now, user, added, AuditConstants.ADD);
            AuditEntities(now, user, modified, AuditConstants.EDIT);
            AuditEntities(now, user, deleted, AuditConstants.REMOVE);
        }

        private void AuditEntities(DateTime now, IUser user, EntityEntry[] entries, string operation)
        {
            foreach (var item in entries)
            {
                var entity = item.Entity as AuditModel;
                if (entity.CanAudit)
                {
                    context.Set<Models.Audit>().Add(new Models.Audit()
                    {
                        DateTime = now,
                        EntityId = entity.Identifier,
                        UserId = user.Id,
                        Operation = operation,
                        EntityType = entity.EntityType,
                        Data = entity.AuditJson
                    });
                }
            }
        }
    }
}
