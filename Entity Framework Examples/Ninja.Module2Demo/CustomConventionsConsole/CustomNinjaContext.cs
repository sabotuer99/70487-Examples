using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomConventionsConsole
{
    class CustomNinjaContext : NinjaContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Add(new EquipmentRenamingConvention());
            modelBuilder.Conventions.AddBefore<PluralizingTableNameConvention>(new PluralZTableNameConvention());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    class EquipmentRenamingConvention : IStoreModelConvention<EdmProperty>
    {
        public void Apply(EdmProperty property, DbModel model)
        {
            if (property.Name == "ServedInOniwaban")
            {
                property.Name = "IsBadass";
            }
        }
    }

    class PluralZTableNameConvention : PluralizingTableNameConvention
    {
        public override void Apply(EntityType item, DbModel model)
        {
            var entitySet = model.StoreModel.Container.GetEntitySetByName(item.Name, true);

            entitySet.Table = entitySet.Table + "z";
        }
    }
}
