using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.Model;
using ADServer.Interface;
using ADServer.BLL;

namespace ADServer.Common
{
    public class DindDealWithInfo
    {
        B_Company_Info bll_company = new B_Company_Info();
        B_Department_Info bll_deptment = new B_Department_Info();
        /// <summary>
        /// 保存公司、部门、员工
        /// </summary>
        /// <returns></returns>
        public bool DealDepartment() 
        {
            DingTalkClient dingClient = new DingTalkClient();
            List<M_DingDepartment> data = dingClient.GetDingDeptGroup();
            try 
            {
                if (data.Count > 0)
                {
                    M_Company_Info company = new M_Company_Info(data[0]);
                    if (!bll_company.Exists_wx(company.CompanyName)) //公司是否存在
                        bll_company.Add_wx(company);//
                    M_Company_Info curCompany = bll_company.GetModel(company.CompanyName);
                    data.ForEach(item =>
                    {
                        if (!bll_deptment.Exists_wx(item.name, company.CompanyName)) //判断所在部门是否存在
                        {
                            M_Department_Info temp = new M_Department_Info(item, curCompany.CompanyId);
                            bll_deptment.Add_wx(temp);
                        }
                        List<M_DingEmp> empData = dingClient.GetPageEmploy(item.id, 0);
                        empData.ForEach(emp => 
                        {

                        });

                    });
                }
            }
            catch (Exception ex) 
            {
                return false;
            }
            return true;
        }

        //public bool DealEmp()
        //{
        //    DingTalkClient dingClient = new DingTalkClient();
        //    List<M_DingEmp> data = dingClient.GetDingEmploy();

        //}
    }
}
