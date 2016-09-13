using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using NinjaDomain.Classes;
using NinjaDomain.Classes.Interfaces;
using System;
using System.Data.Entity.Core.Objects;

namespace NinjaDomain.DataModel
{
    public class NinjaContext : DbContext
    {
        public DbSet<Ninja> Ninjas { get; set;}
        public DbSet<Clan> Clans { get; set; }
        public DbSet<NinjaEquipment> Equipment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Types().
                Configure(c => c.Ignore("IsDirty"));

            modelBuilder
                .Entity<Ninja>()
                .MapToStoredProcedures(s =>
                    s.Insert(i => i.HasName("InsertNinja")
                     .Parameter(n => n.Name, "Name")
                     .Parameter(n => n.ServedInOniwaban, "ServedInOniwaban")
                     .Parameter(n => n.ClanId, "ClanId")
                     .Parameter(n => n.DateOfBirth, "DateOfBirth")
                     .Parameter(n => n.DateModified, "DateModified")
                     .Parameter(n => n.DateCreated, "DateCreated")
                ));

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var history in this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IModificationHistory && 
                              (e.State == EntityState.Added || e.State == EntityState.Modified))
                        .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if(history.DateCreated == DateTime.MinValue){
                    history.DateCreated = DateTime.Now;
                }
            }

            int result = base.SaveChanges();

            foreach (var history in this.ChangeTracker.Entries()
                        .Where(e => e.Entity is IModificationHistory)
                        .Select(e => e.Entity as IModificationHistory))
            {

                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.IsDirty = false;
                }
            }

            return result;
        }

        public override System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            foreach (var history in this.ChangeTracker.Entries()
            .Where(e => e.Entity is IModificationHistory &&
                  (e.State == EntityState.Added || e.State == EntityState.Modified))
            .Select(e => e.Entity as IModificationHistory))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.DateCreated = DateTime.Now;
                }
            }

            var result = base.SaveChangesAsync();

            foreach (var history in this.ChangeTracker.Entries()
            .Where(e => e.Entity is IModificationHistory)
            .Select(e => e.Entity as IModificationHistory))
            {

                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.IsDirty = false;
                }
            }

            return result;
        }
    }
}
