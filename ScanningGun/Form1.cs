using ScanningGun.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanningGun
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            using (InventoryContext db = new InventoryContext())
            {
                db.Goods.Add(new entity.GoodsEntity { barCode = "sdfsafsafsdfas" });
                db.SaveChanges();

                var query = from b in db.Goods
                            orderby b.Name
                            select b;

                List<GoodsEntity> goods = query.ToList();
            }

             
            InitializeComponent();
        }

    }
}
