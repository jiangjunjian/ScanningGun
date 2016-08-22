using ScanningGun.entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanningGun
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(string databaseName = "InventoryDB")
            : base(databaseName)
        {
        }

        public DbSet<GoodsEntity> Goods { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder(this.Database.Connection.ConnectionString);
            string path = AppDomain.CurrentDomain.BaseDirectory + connstr.DataSource;
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (System.IO.File.Exists(fi.FullName) == false)
            {
                if (System.IO.Directory.Exists(fi.DirectoryName) == false)
                {
                    System.IO.Directory.CreateDirectory(fi.DirectoryName);
                }
                SQLiteConnection.CreateFile(fi.FullName);

                connstr.DataSource = path;
                //connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                using (SQLiteConnection conn = new SQLiteConnection(connstr.ConnectionString))
                {
                    string sql = @" CREATE TABLE Goods (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name varchar (50),
                    Time timestamp,
                    barCode  varchar (50),
                    quanty int,
                    cateogory int);";
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            //添加创建代码
            //modelBuilder.Configurations.Add(new Blog());
        }

    }
}
