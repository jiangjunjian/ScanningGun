using ScanningGun.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanningGun
{
    public partial class ListForm : Form
    {
        // 1、定义几个所需的公有成员：
  
         int pageSize = 10;     //每页显示行数
         int nMax = 0;         //总记录数
         int pageCount = 0;    //页数＝总记录数/每页显示行数
         int pageCurrent = 1;   //当前页号
         int nCurrent = 0;      //当前记录行
    



        public ListForm()
        {
            InitializeComponent();
            ///this.dgvInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;  //自动调动datagridview的行高度
            ////dgvInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells;//自动调动datagrid死的宽度
            ///this.dgvInfo.DefaultCellStyle.WrapMode = DataGridViewTriState.True;//设置datagridview字段显示部的内容;
        }

        public DataTable Listtodt(List<GoodsEntity> goods)
        {
            DataTable dttable = new DataTable();
            dttable.Columns.Add("barCode");
            dttable.Columns.Add("Name");
            dttable.Columns.Add("category");
            dttable.Columns.Add("quanty");
            dttable.Rows.Clear();
            foreach (var good in goods)
            {
                DataRow dr = dttable.NewRow();
                dr["barCode"] = good.barCode;
                dr["Name"] = good.Name;

                switch (good.cateogory)
                {
                    case (int)CateogoryEnum.电容:
                        dr["category"] = "电容";
                        break;

                    case (int)CateogoryEnum.电阻:
                        dr["category"] = "电阻";
                        break;
                    default:
                        dr["category"] = "无";
                        break;
                }

               
                dr["quanty"] = good.quanty;
                dttable.Rows.Add(dr);
            }

            return dttable;

        }

        public void binddata()
        {
            DataTable dttable = new DataTable();
            int total = 0;
            using (var db = new InventoryContext())
            {
                var alldata = db.Goods.OrderByDescending(c => c.Id);
                total = alldata.Count();

                dttable = this.Listtodt(alldata.Skip(pageSize * (pageCurrent-1)).Take(pageSize).ToList());
            }

            pageCount = (total / pageSize);    //计算出总页数
            if ((total % pageSize) > 0) pageCount++;

            if (pageCurrent == 1)
            {
                ///bdnInfo.MoveFirstItem.Enabled = false;
               /// bdnInfo.MovePreviousItem.Enabled = false;
            }
            else
            {
                ///bdnInfo.MoveFirstItem.Enabled = true;
                ///bdnInfo.MovePreviousItem.Enabled = true;
            }

            bdsInfo.DataSource = dttable;          
            ///bdnInfo.BindingSource = bdsInfo;
            dgvInfo.DataSource = bdsInfo;
            dgvInfo.Height = this.Height * 80/100;
            ///dgvInfo.RowCount = dttable.Rows.Count;
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "关闭")
            {
                this.Close();
            }
            if (e.ClickedItem.Text == "上一页")
            {
                pageCurrent--;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }

                binddata();
            }
            if (e.ClickedItem.Text == "下一页")
            {
                pageCurrent++;
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }

                binddata();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            binddata();
        }

        private void dgvInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
