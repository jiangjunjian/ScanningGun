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
            ////SQLiteConnection.CreateFile("InventoryDB.db");
            InventoryContext inc = new InventoryContext();
            inc.Goods.Add(new entity.GoodsEntity { barCode="11111"});
            inc.Goods
            InitializeComponent();
        }
    }
}
