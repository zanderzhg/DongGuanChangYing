using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ADServer.DAL;

namespace ADServer
{
    public partial class Frm_SMSFormat : Form
    {
        public Frm_SMSFormat()
        {
            InitializeComponent();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txbCheckIn.Text = "访客易提醒您，@访客姓名 刷卡登记进入拜访您。";
            txbLeave.Text = "访客易提醒您，@访客姓名 已刷卡签离。";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!txbCheckIn.Text.Contains("@访客姓名"))
            {
                MessageBox.Show("设置失败，缺失@访客姓名！");
                return;
            }
            if (!txbLeave.Text.Contains("@访客姓名"))
            {
                MessageBox.Show("设置失败，缺失@访客姓名！");
                return;
            }

            SysFunc.SetParamValue("SmsCheckInContent", txbCheckIn.Text);
            SysFunc.SetParamValue("SmsLeaveContent", txbLeave.Text);

            this.DialogResult = DialogResult.OK;
        }

        private void Frm_SMSFormat_Load(object sender, EventArgs e)
        {
            txbCheckIn.Text = SysFunc.GetParamValue("SmsCheckInContent").ToString();
            txbLeave.Text = SysFunc.GetParamValue("SmsLeaveContent").ToString();
        }
    }
}
