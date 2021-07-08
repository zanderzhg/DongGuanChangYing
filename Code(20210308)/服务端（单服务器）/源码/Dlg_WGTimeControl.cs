using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ADServer.DAL;
using WG3000_COMM.Core;
using ADServer.Model;

namespace ADServer
{
    public partial class Dlg_WGTimeControl : Form
    {
        wgMjController wgMjController1 = new wgMjController();  //此窗体使用的控制器
        wgMjControllerTimeSegList controlTimeSegList;
        public wgMjController control; //正在处理的控制器
        BLL.B_WG_Config bll_wg_config = new BLL.B_WG_Config();
        M_WG_Time wgTime = null;

        public Dlg_WGTimeControl(wgMjController control)
        {
            InitializeComponent();

            this.control = control;
            loadWGTime(control.ControllerSN.ToString());
        }

        private void loadWGTime(string sn)
        {
            wgTime = bll_wg_config.GetWgTimeBySN(sn);
            if (wgTime != null)
            {
                char[] dateArr = wgTime.Opendate.ToCharArray();

                chkMonday.Checked = dateArr[0] == '1' ? true : false;
                chkTuesday.Checked = dateArr[1] == '1' ? true : false;
                chkWednesday.Checked = dateArr[2] == '1' ? true : false;
                chkThursday.Checked = dateArr[3] == '1' ? true : false;
                chkFriday.Checked = dateArr[4] == '1' ? true : false;
                chkSaturday.Checked = dateArr[5] == '1' ? true : false;
                chkSunday.Checked = dateArr[6] == '1' ? true : false;

                dtpTimeZone1From.Value = wgTime.TimeZone1From;
                dtpTimeZone1To.Value = wgTime.TimeZone1To;
                dtpTimeZone2From.Value = wgTime.TimeZone2From;
                dtpTimeZone2To.Value = wgTime.TimeZone2To;
                dtpTimeZone3From.Value = wgTime.TimeZone3From;
                dtpTimeZone3To.Value = wgTime.TimeZone3To;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtpTimeZone1From.Value > dtpTimeZone1To.Value || dtpTimeZone2From.Value > dtpTimeZone2To.Value || dtpTimeZone3From.Value > dtpTimeZone3To.Value)
            {
                MessageBox.Show("开始时间不能晚于结束时间！");
                return;
            }

            if (controlTimeSegList == null)
            {
                controlTimeSegList = new wgMjControllerTimeSegList();
            }
            MjControlTimeSeg mjCI = new MjControlTimeSeg();
            mjCI.ymdStart = this.dateTimePicker7.Value;
            mjCI.ymdEnd = this.dateTimePicker6.Value;
            mjCI.SegIndex = (byte)(2);
            mjCI.TotalLimittedAccess = (byte)(this.numericUpDown10.Value);
            mjCI.LimittedMode = 0;
            mjCI.nextSeg = byte.Parse(this.comboBox58.Text.ToString());
            mjCI.weekdayControl = 0;
            mjCI.weekdayControl += (byte)(this.chkMonday.Checked ? (1 << 0) : 0);
            mjCI.weekdayControl += (byte)(this.chkTuesday.Checked ? (1 << 1) : 0);
            mjCI.weekdayControl += (byte)(this.chkWednesday.Checked ? (1 << 2) : 0);
            mjCI.weekdayControl += (byte)(this.chkThursday.Checked ? (1 << 3) : 0);
            mjCI.weekdayControl += (byte)(this.chkFriday.Checked ? (1 << 4) : 0);
            mjCI.weekdayControl += (byte)(this.chkSaturday.Checked ? (1 << 5) : 0);
            mjCI.weekdayControl += (byte)(this.chkSunday.Checked ? (1 << 6) : 0);
            mjCI.hmsStart1 = this.dtpTimeZone1From.Value;
            mjCI.hmsEnd1 = this.dtpTimeZone1To.Value;
            mjCI.hmsStart2 = this.dtpTimeZone2From.Value;
            mjCI.hmsEnd2 = this.dtpTimeZone2To.Value;
            mjCI.hmsStart3 = this.dtpTimeZone3From.Value;
            mjCI.hmsEnd3 = this.dtpTimeZone3To.Value;
            mjCI.LimittedAccess1 = (byte)(0);
            mjCI.LimittedAccess2 = (byte)(0);
            mjCI.LimittedAccess3 = (byte)(0);
            //this.listBox2.Items.Add(this.comboBox57.Text.ToString() + " , " + System.BitConverter.ToString(mjCI.ToBytes()));
            controlTimeSegList.AddItem(mjCI);

            if (this.controlTimeSegList != null)
            {
                if (control.UpdateControlTimeSegListIP(this.controlTimeSegList.ToByte()) > 0)//上传时段列表
                {
                    string openDate = "";
                    openDate += chkMonday.Checked == true ? "1" : "0";
                    openDate += chkTuesday.Checked == true ? "1" : "0";
                    openDate += chkWednesday.Checked == true ? "1" : "0";
                    openDate += chkThursday.Checked == true ? "1" : "0";
                    openDate += chkFriday.Checked == true ? "1" : "0";
                    openDate += chkSaturday.Checked == true ? "1" : "0";
                    openDate += chkSunday.Checked == true ? "1" : "0";

                    if (wgTime == null)
                    {
                        wgTime = new M_WG_Time();
                        wgTime.Sn = control.ControllerSN.ToString();
                        wgTime.Opendate = openDate;
                        wgTime.TimeZone1From = dtpTimeZone1From.Value;
                        wgTime.TimeZone1To = dtpTimeZone1To.Value;
                        wgTime.TimeZone2From = dtpTimeZone2From.Value;
                        wgTime.TimeZone2To = dtpTimeZone2To.Value;
                        wgTime.TimeZone3From = dtpTimeZone3From.Value;
                        wgTime.TimeZone3To = dtpTimeZone3To.Value;

                        bll_wg_config.Add(wgTime);
                    }
                    else
                    {
                        wgTime.Opendate = openDate;
                        wgTime.TimeZone1From = dtpTimeZone1From.Value;
                        wgTime.TimeZone1To = dtpTimeZone1To.Value;
                        wgTime.TimeZone2From = dtpTimeZone2From.Value;
                        wgTime.TimeZone2To = dtpTimeZone2To.Value;
                        wgTime.TimeZone3From = dtpTimeZone3From.Value;
                        wgTime.TimeZone3To = dtpTimeZone3To.Value;

                        bll_wg_config.Update(wgTime);
                    }

                    MessageBox.Show("设置成功！");
                }
                else
                {
                    MessageBox.Show("设置失败！");
                }
            }
            else
            {
                MessageBox.Show("设置失败！");
            }
        }

    }
}
