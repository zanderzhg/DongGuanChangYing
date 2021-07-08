using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ADServer.DAL;
using ADServer.BLL;
using System.Diagnostics;

namespace ADServer
{
    public partial class Frm_Reg : Form
    {
        private int groupboxHeight;
        private delegate void hw_eventHandler();

        public Frm_Reg(bool needShutDown)
        {
            InitializeComponent();
        }

        private void FrmReg_Load(object sender, EventArgs e)
        {
            groupboxHeight = groupBox1.Height;
            this.Height -= groupboxHeight;
            txtPcCode.Text = SysFunc.getUserNum();
            if (SysFunc.GetRegistData() != "")
                txt_sta.Text = "[软件已注册]";
            else
            {
                BLL.B_Base_Info.days = SysFunc.GetTestDays(false);
                txt_sta.Text = "[访客登记功能试用天数还剩" + B_Base_Info.days + "天]";
            }
            txtCpy.Text = SysFunc.GetRegistData();
            txt_sta.ForeColor = System.Drawing.Color.Red;

        }

        private void btAddDay_Click(object sender, EventArgs e)
        {
            if (this.btAddDay.Text == "增加试用天数")
            {
                this.Height += groupboxHeight;
                this.btAddDay.Text = "隐藏";
            }
            else
            {
                this.Height -= this.groupboxHeight;
                this.btAddDay.Text = "增加试用天数";
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btReg_Click(object sender, EventArgs e)
        {
            if (txtCpy.Text == "")
            {
                MessageBox.Show("请输入单位名称");
                return;
            }
            if (txtRegCode.Text == SysFunc.getActiveNum(txtCpy.Text))
            {
                SysFunc.WTRegedit(txtCpy.Text);    //软件登陆时取出此信息

                MessageBox.Show("注册成功,重启软件生效！");

                this.Close();
            }
            else
            {
                MessageBox.Show("注册码错误");
            }
        }

        private void btCopy_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(label2.Text + txtCpy.Text + "\n");
            sb.Append(label3.Text + txtPcCode.Text);
            Clipboard.SetDataObject(sb.ToString());

        }

        private void bt_Copy_Click(object sender, EventArgs e)
        {
            if (txtCpy.Text == "")
            {
                MessageBox.Show("请输入单位名称");
                return;
            }
            if (txtDays.Text == "")
            {
                MessageBox.Show("请输入要增加的试用天数");
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(label2.Text + txtCpy.Text + "\n");
            sb.Append(label3.Text + txtPcCode.Text + "\n");
            sb.Append(label6.Text + txtDays.Text);
            Clipboard.SetDataObject(sb.ToString());
        }

        private void bt_AddDays_Click(object sender, EventArgs e)
        {
            if (txtCpy.Text == "")
            {
                MessageBox.Show("单位名称不能为空");
                return;
            }
            if (txtDays.Text == "")
            {
                MessageBox.Show("增加天数不能为空");
                return;
            }

            if (!SysFunc.isValidAddDaysKey(txtKey.Text))
            {
                MessageBox.Show("当前密钥已使用过");
                return;
            }

            if (txtKey.Text != SysFunc.getKeyNum(txtCpy.Text, txtPcCode.Text, txtDays.Text))
            {
                MessageBox.Show("产品密钥错误");
                return;
            }
            SysFunc.AddTestDays(int.Parse(txtDays.Text), txtKey.Text);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}