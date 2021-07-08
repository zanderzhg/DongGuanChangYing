using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADServer.DAL;
using ADServer.Model;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace ADServer.BLL
{
    public partial class B_WG_Record
    {
        private readonly D_WG_Record dal = new D_WG_Record();

        public B_WG_Record()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(M_WG_Record_Info model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return dal.DeleteList(idlist);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_WG_Record_Info> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_WG_Record_Info> GetModelList(string strWhere, int pageSize, int pageIndex, out int pageCount)
        {
            DataSet ds = dal.GetList(strWhere, pageSize, pageIndex, out pageCount);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_WG_Record_Info> DataTableToList(DataTable dt)
        {
            List<M_WG_Record_Info> modelList = new List<M_WG_Record_Info>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_WG_Record_Info model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new M_WG_Record_Info();

                    if (dt.Rows[n]["Id"] != null && dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    if (dt.Rows[n]["CardId"] != null && dt.Rows[n]["CardId"].ToString() != "")
                    {
                        model.CardSNR = dt.Rows[n]["CardId"].ToString();
                    }
                    if (dt.Rows[n]["RecordTime"] != null && dt.Rows[n]["RecordTime"].ToString() != "")
                    {
                        model.RecordTime = DateTime.Parse(dt.Rows[n]["RecordTime"].ToString());
                    }
                    if (dt.Rows[n]["DoorName"] != null && dt.Rows[n]["DoorName"].ToString() != "")
                    {
                        model.DoorName = dt.Rows[n]["DoorName"].ToString();
                    }
                    if (dt.Rows[n]["Event"] != null && dt.Rows[n]["Event"].ToString() != "")
                    {
                        model.REvent = dt.Rows[n]["Event"].ToString();
                    }
                    if (dt.Rows[n]["VisitorName"] != null && dt.Rows[n]["VisitorName"].ToString() != "")
                    {
                        model.VisitorName = dt.Rows[n]["VisitorName"].ToString();
                    }
                    if (dt.Rows[n]["PersonType"] != null && dt.Rows[n]["PersonType"].ToString() != "")
                    {
                        model.PersonType = int.Parse(dt.Rows[n]["PersonType"].ToString());
                    }
                    if (dt.Rows[n]["EmpName"] != null && dt.Rows[n]["EmpName"].ToString() != "")
                    {
                        model.EmpName = dt.Rows[n]["EmpName"].ToString();
                    }
                    if (dt.Rows[n]["Uploadpf"] != null && dt.Rows[n]["Uploadpf"].ToString() != "")
                    {
                        model.Uploadpf = int.Parse(dt.Rows[n]["Uploadpf"].ToString());
                    }
                    if (dt.Rows[n]["controllerSN"] != null && dt.Rows[n]["controllerSN"].ToString() != "")
                    {
                        model.ControllerSN = dt.Rows[n]["controllerSN"].ToString();
                    }
                    if (dt.Rows[n]["controllerIP"] != null && dt.Rows[n]["controllerIP"].ToString() != "")
                    {
                        model.ControllerIP = dt.Rows[n]["controllerIP"].ToString();
                    }
                    if (dt.Rows[n]["doorIndex"] != null && dt.Rows[n]["doorIndex"].ToString() != "")
                    {
                        model.DoorIndex = int.Parse(dt.Rows[n]["doorIndex"].ToString());
                    }
                    if (dt.Rows[n]["isEntryEvent"] != null && dt.Rows[n]["isEntryEvent"].ToString() != "")
                    {
                        model.IsEntryEvent = int.Parse(dt.Rows[n]["isEntryEvent"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 上传门禁刷卡记录到访客易平台
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="PFToken"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool PostAccessDoorRecord(string ipPort, string username, string pfToken, M_WG_Record_Info record, ref string error)
        {
            try
            {
                string url = "http://" + ipPort + "/tecsunapi/Visitor/PostAccessDoorRecord";

                string token = HttpWebResponseUtility.GetToken((string)SysFunc.GetParamValue("PFToken"));

                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("token", token);
                parameters.Add("strUsername", username);
                parameters.Add("strCardId", record.CardSNR);
                parameters.Add("dtRecordTime", record.RecordTime.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("strDoorName", record.DoorName);
                parameters.Add("strEvent", record.REvent);
                parameters.Add("strVisitorName", record.VisitorName);
                parameters.Add("strEmpName", record.EmpName);
                parameters.Add("iPersonType", record.PersonType.ToString());

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = reader.ReadToEnd().ToString();
                    Dictionary<string, object> jsonToken = serializer.DeserializeObject(responseText) as Dictionary<string, object>;
                    string codeToken = jsonToken["code"].ToString();
                    if (codeToken == "200")
                    {
                        return true;
                    }
                    else
                    {
                        string message = jsonToken["message"].ToString();
                        error = "上传门禁记录失败：" + message;
                        if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                        }
                        string nowTime = DateTime.Now.ToString("yyyyMMdd");
                        string file = Application.StartupPath + "\\Logs\\上传门禁记录_" + nowTime + ".txt";
                        if (!File.Exists(file))
                        {
                            FileStream fs = new FileStream(file, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);
                            sw.Write(error);
                            sw.Close();
                            fs.Close();
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = "上传门禁记录失败:" + ex.Message;

                if (!Directory.Exists(Application.StartupPath + "\\Logs"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Logs");
                }
                string nowTime = DateTime.Now.ToString("yyyyMMdd");
                string file = Application.StartupPath + "\\Logs\\上传门禁记录_" + nowTime + ".txt";
                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write("error");
                    sw.Close();
                    fs.Close();
                }

                return false;
            }

            //    //将用户名与密码存入SoapHeader;
            //    WebServiceHelper.PfSoapHeader header = new WebServiceHelper.PfSoapHeader("PfSoapHeader");
            //    header.AddProperty("UserName", pfServiceUsername);
            //    header.AddProperty("PassWord", pfServicePwd);
            //    object[] args = new object[8];
            //    args[0] = username;
            //    args[1] = record.CardSNR;
            //    args[2] = record.RecordTime.ToString("yyyy-MM-dd HH:mm:ss");
            //    args[3] = record.DoorName;
            //    args[4] = record.REvent;
            //    args[5] = record.VisitorName;
            //    args[6] = record.EmpName;
            //    args[7] = record.PersonType;

            //    object r = WebServiceHelper.InvokeWebService(
            //         url,
            //        "PostAccessDoorRecord",
            //        header,
            //        args);

            //    if (r == null)
            //    {
            //        MessageBox.Show("上传门禁记录失败,请检查配置！");
            //    }
            //    else
            //    {
            //        object[] values = r as object[];

            //        Type Tp1 = r.GetType();
            //        FieldInfo[] fields = Tp1.GetFields();
            //        FieldInfo filed = fields.GetValue(0) as FieldInfo;
            //        string code = (string)filed.GetValue(r);
            //        FieldInfo filed2 = fields.GetValue(1) as FieldInfo;
            //        string retDesc = (string)filed2.GetValue(r);

            //        if (code != "200")
            //        {
            //            MessageBox.Show("上传门禁记录失败，错误信息：" + retDesc);
            //        }
            //        else
            //        {

            //            FieldInfo Value1 = fields.GetValue(2) as FieldInfo;
            //            string userid = (string)Value1.GetValue(r);



            //            return true;
            //        }
            //    }

            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);

            //    return false;
            //}
        }

        /// <summary>
        /// 更改上传标识的状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="uploadpf"></param>
        /// <returns></returns>
        public void UpdateStatus(int recordId, int uploadpf)
        {
            dal.UpdateStatus(recordId, uploadpf);
        }

    }
}
