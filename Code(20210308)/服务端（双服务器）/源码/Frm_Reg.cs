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
                txt_sta.Text = "[�����ע��]";
            else
            {
                BLL.B_Base_Info.days = SysFunc.GetTestDays(false);
                txt_sta.Text = "[�ÿ͵Ǽǹ�������������ʣ" + B_Base_Info.days + "��]";
            }
            txtCpy.Text = SysFunc.GetRegistData();
            txt_sta.ForeColor = System.Drawing.Color.Red;

        }

        private void btAddDay_Click(object sender, EventArgs e)
        {
            if (this.btAddDay.Text == "������������")
            {
                this.Height += groupboxHeight;
                this.btAddDay.Text = "����";
            }
            else
            {
                this.Height -= this.groupboxHeight;
                this.btAddDay.Text = "������������";
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
                MessageBox.Show("�����뵥λ����");
                return;
            }
            if (txtRegCode.Text == SysFunc.getActiveNum(txtCpy.Text))
            {
                SysFunc.WTRegedit(txtCpy.Text);    //�����½ʱȡ������Ϣ

                MessageBox.Show("ע��ɹ�,���������Ч��");

                this.Close();
            }
            else
            {
                MessageBox.Show("ע�������");
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
                MessageBox.Show("�����뵥λ����");
                return;
            }
            if (txtDays.Text == "")
            {
                MessageBox.Show("������Ҫ���ӵ���������");
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
                MessageBox.Show("��λ���Ʋ���Ϊ��");
                return;
            }
            if (txtDays.Text == "")
            {
                MessageBox.Show("������������Ϊ��");
                return;
            }

            if (!SysFunc.isValidAddDaysKey(txtKey.Text))
            {
                MessageBox.Show("��ǰ��Կ��ʹ�ù�");
                return;
            }

            if (txtKey.Text != SysFunc.getKeyNum(txtCpy.Text, txtPcCode.Text, txtDays.Text))
            {
                MessageBox.Show("��Ʒ��Կ����");
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