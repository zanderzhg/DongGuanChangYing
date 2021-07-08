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
using ADServer.BLL;
using ADServer.Model;

namespace ADServer
{
    public partial class Dlg_Passageway : Form
    {
        B_WG_Config bll_wgConfig = new B_WG_Config();
        public Dlg_Passageway()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Dlg_Passageway_Load(object sender, EventArgs e)
        {
            loadPassageway();
        }

        private void loadPassageway()
        {
            listViewPassageway.Items.Clear();

            List<M_PassageWay> passagewayList = bll_wgConfig.GetPassagewayList("");
            foreach (M_PassageWay passageway in passagewayList)
            {
                int sn = listViewPassageway.Items.Count + 1;
                ListViewItem lvItem = new ListViewItem(sn.ToString());
                lvItem.SubItems.Add(passageway.Name);
                lvItem.Tag = passageway.Id;
                listViewPassageway.Items.Add(lvItem);
            }
        }

        private void listViewWG_Click(object sender, EventArgs e)
        {
            if (listViewPassageway.SelectedItems.Count > 0 || listViewPassageway.Items.Count == 1)
            {
                txbName.Text = listViewPassageway.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewPassageway.SelectedItems.Count > 0 || listViewPassageway.Items.Count == 1)
            {
                bll_wgConfig.DeletePassageway((int)listViewPassageway.SelectedItems[0].Tag);
            }

            loadPassageway();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listViewPassageway.Items.Count == 10)
            {
                MessageBox.Show("通道数目不能超过10个！");
                return;
            }

            if (txbName.Text == "")
            {
                MessageBox.Show("通道名称不能为空！");
                return;
            }

            if (SysFunc.IsDangerSqlString(txbName.Text))
            {
                MessageBox.Show("通道名称不能包含非法字符 ;,/%@*!'！");
                return;
            }

            bool isExist = bll_wgConfig.ExistPassageway(txbName.Text, -1);
            if (isExist)
            {
                MessageBox.Show("已存在同名的通道名称！");
                return;
            }

            M_PassageWay pw = new M_PassageWay();
            pw.Name = txbName.Text;
            pw.AcType = 1;

            bll_wgConfig.AddPassageway(pw);

            loadPassageway();
            txbName.Text = "";
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (listViewPassageway.SelectedItems.Count > 0 || listViewPassageway.Items.Count == 1)
            {
                if (txbName.Text == "")
                {
                    MessageBox.Show("通道名称不能为空！");
                    return;
                }

                if (SysFunc.IsDangerSqlString(txbName.Text))
                {
                    MessageBox.Show("通道名称不能包含非法字符 ;,/%@*!'！");
                    return;
                }

                int id = (int)listViewPassageway.SelectedItems[0].Tag;

                bool isExist = bll_wgConfig.ExistPassageway(txbName.Text, id);
                if (isExist)
                {
                    MessageBox.Show("已存在同名的通道名称！");
                    return;
                }

                M_PassageWay pw = new M_PassageWay();
                pw.Id = id;
                pw.Name = txbName.Text;

                bll_wgConfig.UpdatePassageway(pw);
                loadPassageway();
            }
        }

    }
}
