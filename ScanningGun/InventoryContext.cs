using ScanningGun.entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanningGun
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(string databaseName = "InventoryDB") : base(databaseName)
        {

        }

        public DbSet<GoodsEntity> Goods { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }

    }
}
