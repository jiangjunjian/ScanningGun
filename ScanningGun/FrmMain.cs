using ScanningGun.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanningGun
{
    public partial class FrmMain : Form
    {
        InventoryContext db = new InventoryContext();
        BardCodeHooK BarCode = new BardCodeHooK();
        public FrmMain()
        {
            InitializeComponent();
            BarCode.BarCodeEvent += new BardCodeHooK.BardCodeDeletegate(BarCode_BarCodeEvent);
        }

        private delegate void ShowInfoDelegate(BardCodeHooK.BarCodes barCode);
        private void ShowInfo(BardCodeHooK.BarCodes barCode)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });
            }
            else
            {

                tbtiaoma.Text = barCode.IsValid ? barCode.BarCode : "";
                if (!string.IsNullOrEmpty(tbtiaoma.Text))
                {
                    var goodinfo = db.Goods.First(c => c.barCode == tbtiaoma.Text);

                    if (goodinfo != null)
                    {
                        tbname.Text = goodinfo.Name;
                        tbtiaoma.Text = goodinfo.barCode;
                        tbkucun.Text = goodinfo.quanty.ToString();
                        foreach (Control c in groupBox1.Controls)//遍历groupBox1中的所有空间
                        {
                            if (c is RadioButton)//判断该控件是不是RadioButton
                            {
                                RadioButton r = (RadioButton)c;//做转化
                                if (r.Tag.ToString()==goodinfo.cateogory.ToString())
                                {
                                    r.Checked = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        var newgood = BuildGood();
                        db.Goods.Add(newgood);
                    }
                }


                ///// 触发扫描事件
                //1.判断是否有录入
                //2. 显示名称库存
                // 3.填写库存 + -

                textBox1.Text = barCode.KeyName;
                textBox2.Text = barCode.VirtKey.ToString();
                textBox3.Text = barCode.ScanCode.ToString();
                textBox4.Text = barCode.Ascll.ToString();
                textBox5.Text = barCode.Chr.ToString();
                textBox6.Text = barCode.IsValid ? barCode.BarCode : "";//是否为扫描枪输入，如果为true则是 否则为键盘输入

                textBox7.Text += barCode.KeyName;
                //MessageBox.Show(barCode.IsValid.ToString());
            }
        }

        //C#中判断扫描枪输入与键盘输入 

        //Private DateTime _dt = DateTime.Now;  //定义一个成员函数用于保存每次的时间点
        //private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    DateTime tempDt = DateTime.Now;          //保存按键按下时刻的时间点
        //    TimeSpan ts = tempDt .Subtract(_dt);     //获取时间间隔
        //    if (ts.Milliseconds > 50)                           //判断时间间隔，如果时间间隔大于50毫秒，则将TextBox清空
        //        textBox1.Text = "";
        //    dt = tempDt ;
        //}



        void BarCode_BarCodeEvent(BardCodeHooK.BarCodes barCode)
        {
            ShowInfo(barCode);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            BarCode.Start();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            BarCode.Stop();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text.Length > 0)
            {
                MessageBox.Show("条码长度：" + textBox6.Text.Length + "\n条码内容：" + textBox6.Text, "系统提示");
            }
        }

        private void tbkucun_KeyDown(object sender, KeyEventArgs e)
        {
            int kucun;
            int.TryParse(tbkucun.Text, out kucun);
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Add)
            {
                kucun = kucun + 1;
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Subtract) {
                kucun = kucun - 1;
            }

            tbkucun.Text = kucun.ToString();
        }

        private void btsava_Click(object sender, EventArgs e)
        {
            GoodsEntity good;

            using (var db = new InventoryContext())
            {
                good = db.Goods.FirstOrDefault(c => c.barCode == tbtiaoma.Text);
            }

            if (good != null && good.Id > 0)
            {
                using (var dbCtx = new InventoryContext())
                {
                    dbCtx.Entry(good).State = EntityState.Modified;
                    dbCtx.SaveChanges();
                }
            }
            else
            {
                using (var db = new InventoryContext())
                {
                    good= BuildGood();
                    db.Goods.Add(good);
                    db.SaveChanges();
                }
            }

        }

        private GoodsEntity BuildGood()
        {
            var newgood = new GoodsEntity
            {
                barCode = tbtiaoma.Text,
                Name = tbname.Text,
                quanty = int.Parse(tbkucun.Text)
            };

            foreach (Control c in groupBox1.Controls)//遍历groupBox1中的所有空间
            {
                if (c is RadioButton)//判断该控件是不是RadioButton
                {
                    RadioButton r = (RadioButton)c;//做转化
                    bool rbState = r.Checked;//得到Checked状态
                    if (r.Checked)
                    {
                        newgood.cateogory = int.Parse(r.Tag.ToString());
                    }
                }
            }

            return newgood;
        }
    }
}
