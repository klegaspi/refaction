using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.Model.Migrations;
//using Xero.Model.Migrations;

namespace Xero.Model
{
    public class DatabaseContext : DbContext
    {
        #region Member Variables

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductOption> ProductOption { get; set; }

        #endregion

        #region Constructor

        public DatabaseContext() : base("DatabaseContext")
        {
            Database.SetInitializer<DatabaseContext>(null);
            Database.SetInitializer<DatabaseContext>(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>()); 
        }

        #endregion
    }
}
