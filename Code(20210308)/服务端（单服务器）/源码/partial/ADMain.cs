using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using ADServer.Model;
using ADServer.BLL;
using ADServer.Utils;
using ADServer.Interface;
using ADServer.DAL;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Data;

namespace ADServer
{
    public partial class ADMain : Form
    {
        #region 数据接口
        bool openDataServer = false;
        HttpListener DataSrvPlatformHttpPostRequest = new HttpListener();

        string dataSrvIP = string.Empty;
        string dataSrvPort = string.Empty;
        string dataSrvAppId = string.Empty;

        public void InitDataSrvConfig()
        {
            dataSrvIP = txtDataIP.Text = SysFunc.GetParamValue("DataSrvIP").ToString();
            dataSrvPort = txtDataPort.Text = SysFunc.GetParamValue("DataSrvPort").ToString();
            dataSrvAppId = txtDataAppId.Text = SysFunc.GetParamValue("DataSrvAppId").ToString();
            if (!string.IsNullOrEmpty(dataSrvIP) && !string.IsNullOrEmpty(dataSrvPort))
            {
                openDataServer = true;
            }
            else
            {
                MessageBox.Show("请完善数据服务接口和端口信息！");
                return;
            }
        }

        /// <summary>
        /// 初始化接口
        /// </summary>
        private int OpenDataServicesPlatform()
        {
            #region 开启接口服务
            int pfPort = 0;

            if (dataSrvIP == "" || dataSrvPort == "")
            {
                MessageBox.Show("请完善数据服务接口和端口信息！");
                return 0;
            }

            if (IsIP(dataSrvIP) && int.TryParse(dataSrvPort, out pfPort))
            {
                if (DataSrvPlatformHttpPostRequest.IsListening)
                {
                    DataSrvPlatformHttpPostRequest.Stop();
                    DataSrvPlatformHttpPostRequest.Prefixes.Clear();
                }
                else
                {
                    DataSrvPlatformHttpPostRequest = new HttpListener();
                }
                DataSrvPlatformHttpPostRequest.Prefixes.Add("http://" + dataSrvIP + ":" + dataSrvPort + "/");
                DataSrvPlatformHttpPostRequest.Start();

                if (DataSrvPlatformHttpPostRequest.IsListening)
                {
                    try
                    {
                        DataSrvPlatformHttpPostRequest.BeginGetContext(new AsyncCallback(DataServicesPlatformGetContextCallBack), DataSrvPlatformHttpPostRequest);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("启动服务错误，详情信息：" + ex.Message);
                        return 0;
                    }
                }
            }
            else
            {
                MessageBox.Show("数据服务接口和端口非法！");
            }
            #endregion
            return pfPort;
        }

        /// <summary>
        /// 数据处理接口
        /// </summary>
        /// <param name="ar"></param>
        private void DataServicesPlatformGetContextCallBack(IAsyncResult ar)
        {
            if (!HttpListener.IsSupported)
            {
                throw new System.InvalidOperationException(
                    "使用 HttpListener 必须为 Windows XP SP2 或 Server 2003 以上系统！");
            }

            try
            {
                HttpListener sSocket = ar.AsyncState as HttpListener;
                //HttpListenerContext context = (HttpListenerContext)obj;
                HttpListenerContext context = sSocket.EndGetContext(ar);
                sSocket.BeginGetContext(new AsyncCallback(DataServicesPlatformGetContextCallBack), sSocket);

                // 取得请求对象
                HttpListenerRequest request = context.Request;
                // 构造回应内容
                string responseString = AnaRequestDataManagerServicesPlatformData(request);

                // 取得回应对象
                HttpListenerResponse response = context.Response;
                // 设置回应头部内容，长度，编码
                response.ContentLength64
                    = System.Text.Encoding.UTF8.GetByteCount(responseString);
                response.ContentType = "application/json; charset=UTF-8";
                // 输出回应内容
                System.IO.Stream output = response.OutputStream;
                System.IO.StreamWriter writer = new System.IO.StreamWriter(output);
                writer.Write(responseString);
                // 必须关闭输出流
                writer.Close();
            }
            catch { }
            finally
            {
                GC.Collect();
            }
        }



        /// <summary>
        /// 分析处理数据接口接口接收的数据包DateServicesPlatform
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string AnaRequestDataManagerServicesPlatformData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                Console.WriteLine("请求中无客户端发来的POST数据包");
                return "";
            }
            string postData = string.Empty;

            #region 简单方法

            using (Stream inputStream = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(inputStream))
                {
                    postData = reader.ReadToEnd();
                }
            }
            #endregion

            string logContent = request.RawUrl + "\r\n" + postData;
            FKY_WCFLibrary.WriteLog.Log4Local(logContent, true);

            string result = "";
            bool isValidRawUrl = false;

            try
            {
                switch (request.RawUrl)
                {
                    case "/tecsunapi/data/get/token":
                        {
                            if (openDataServer)
                            {
                                #region token获取
                                isValidRawUrl = true;
                                string _appId = string.Empty;

                                try
                                {
                                    #region token获取处理
                                    JObject json = JObject.Parse(postData);
                                    _appId = json.Value<string>("appId");//访客姓名
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取token请求");
                                    //LogNet.WriteLog("服务端", "token获取请求[" + _appId + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    if (!string.IsNullOrEmpty(_appId) && _appId == dataSrvAppId)
                                    {
                                        TokenInfo M = new TokenInfo()
                                        {
                                            appId = _appId,
                                        };

                                        string orgStr = AuthTokenHelper.GenToken(M);
                                        string access_Token = orgStr;//SysFunc.CardNoEncryptByDES(orgStr); 

                                        result = ApiTools.MsgFormat(ApiTools.ResponseCode.成功, "数据请求成功", access_Token);
                                    }
                                    else
                                    {
                                        result = VisitorInterface.ErrorPost(201, "获取token失败,不存在此appId", "");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "获取token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取token异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    #region 员工
                    case "/tecsunapi/data/update/employee":
                        {
                            if (openDataServer)
                            {
                                #region 员工管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                JArray empList = new JArray();
                                try
                                {
                                    #region 获取Employee处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    empList = json.Value<JArray>("datas");
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取员工数据同步请求");
                                    //LogNet.WriteLog("服务端", "获取Employee处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证
                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.ReceiveEmploy_API(_token, empList);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("updateEmployee异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;

                    case "/tecsunapi/data/get/employee":
                        {
                            if (openDataServer)
                            {
                                #region 员工查询管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                try
                                {
                                    #region 获取getEmployee处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取员工数据查询请求");
                                    //LogNet.WriteLog("服务端", "获取getEmployee处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证

                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.授权失败, "token不能为空", "");
                                    }

                                    #endregion

                                    result = VisitorInterface.GetEmployInfo_API(_token, json);

                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取getEmployee异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    #endregion

                    #region 预约
                    case "/tecsunapi/data/update/visitor/receiveAppointment":
                        {
                            if (openDataServer)
                            {
                                #region 预约管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                int flag = 0;
                                JArray lstVisitPersonVO = new JArray();

                                try
                                {
                                    #region 获取booking处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    flag = json.Value<int>("flag");//1增2改3删
                                    lstVisitPersonVO = json.Value<JArray>("datas"); //访问人员列表
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客预约同步请求");
                                    //LogNet.WriteLog("服务端", "获取booking处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证
                                    if (flag == 0)
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "flag有误", "");
                                    }
                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.ReceiveAppointment_API(_token, lstVisitPersonVO, flag);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取booking异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/data/get/visitor/bookinginfo":
                        {
                            if (openDataServer)
                            {
                                #region 预约查询管理
                                isValidRawUrl = true;
                                string _token = string.Empty;

                                try
                                {
                                    #region 获取getbooking处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客预约数据查询请求");
                                    //LogNet.WriteLog("服务端", "获取getbookinginfo处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证

                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion


                                    result = VisitorInterface.GetBookingInfo_API(_token, json);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取getbookinginfo异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    #endregion

                    #region 白名单
                    case "/tecsunapi/data/get/visitor/white":
                        {
                            if (openDataServer)
                            {
                                #region 白名单查询管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                JArray ja = new JArray();
                                try
                                {
                                    #region 获取getwhite处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客白名单查询请求");
                                    //LogNet.WriteLog("服务端", "获取getwhite处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证
                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion
                                    result = VisitorInterface.GetWhiteInfo_API(_token, json);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取getwhite异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/data/update/visitor/white":
                        {
                            if (openDataServer)
                            {
                                #region 白名单管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                JArray ja = new JArray();
                                try
                                {
                                    #region 获取white处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    ja = json.Value<JArray>("datas");
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客白名单数据同步请求");
                                    //LogNet.WriteLog("服务端", "获取white处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证

                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.ReceiveWhite_API(_token, ja);

                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取updateWhite异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    #endregion

                    #region 黑名单
                    case "/tecsunapi/data/update/visitor/black":
                        {
                            if (openDataServer)
                            {
                                #region 黑名单管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                JArray ja = new JArray();
                                try
                                {
                                    #region 获取black处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    ja = json.Value<JArray>("datas");
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客黑名单数据同步请求");
                                    //LogNet.WriteLog("服务端", "获取black处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证
                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.ReceiveBlack_API(_token, ja);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取updateblack异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/data/get/visitor/black":
                        {
                            if (openDataServer)
                            {
                                #region 黑名单查询管理
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                try
                                {
                                    #region 获取getblack处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客黑名单数据查询请求");
                                    //LogNet.WriteLog("服务端", "获取getblack处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                                    #endregion

                                    #region 参数验证
                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.GetBlackInfo_API(_token, json);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取getblack异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    #endregion

                    #region 访客
                    case "/tecsunapi/data/get/visitor/info":
                        {
                            if (openDataServer)
                            {
                                #region 普通访客查询
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                try
                                {
                                    #region 获取getvisitor处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客数据查询请求");
                                    //LogNet.WriteLog("服务端", "获取getvisitor处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证
                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.GetVisitorInfo_API(_token, json);

                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取getvisitor异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    case "/tecsunapi/data/get/visitor/photo":
                        {
                            if (openDataServer)
                            {
                                #region 普通访客查询
                                isValidRawUrl = true;
                                string _token = string.Empty;
                                string visitno = string.Empty;
                                try
                                {
                                    #region 获取getvisitor处理请求
                                    JObject json = JObject.Parse(postData);
                                    _token = json.Value<string>("token");//token
                                    visitno = json.Value<string>("visitno");//visitno
                                    addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客图像数据查询请求");
                                    //LogNet.WriteLog("服务端", "获取getVisitorPhoto处理请求[" + visitno + "]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                    #endregion

                                    #region 参数验证

                                    if (string.IsNullOrEmpty(_token))
                                    {
                                        return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                    }
                                    #endregion

                                    result = VisitorInterface.GetVisitorPhotoInfo_API(_token, visitno);
                                }
                                catch (Exception ex)
                                {
                                    result = VisitorInterface.ErrorPost(400, "token异常", "");
                                    FKY_WCFLibrary.WriteLog.Log4Local("获取getVisitorPhoto异常\r\n[" + ex.ToString() + "]", true);
                                }
                                #endregion
                            }
                            else
                            {
                                isValidRawUrl = false;
                                LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                            }
                        }
                        break;
                    #endregion

                    #region 门禁
                    case "/tecsunapi/data/get/acInfo":
                        if (openDataServer)
                        {
                            #region 门禁查询管理
                            isValidRawUrl = true;
                            string _token = string.Empty;
                            try
                            {
                                #region 获取getEmployee处理请求
                                JObject json = JObject.Parse(postData);
                                _token = json.Value<string>("token");//token

                                addRuningLog(DateTime.Now + " " + request.RemoteEndPoint.Address.ToString() + " - 获取访客门禁数据查询请求");
                                //LogNet.WriteLog("服务端", "获取getEmployee处理请求", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志

                                #endregion

                                #region 参数验证

                                if (string.IsNullOrEmpty(_token))
                                {
                                    return ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "token不能为空", "");
                                }

                                #endregion

                                result = VisitorInterface.GetAcInfo_API(_token, json);

                            }
                            catch (Exception ex)
                            {
                                result = VisitorInterface.ErrorPost(400, "token异常", "");
                                FKY_WCFLibrary.WriteLog.Log4Local("获取getEmployee异常\r\n[" + ex.ToString() + "]", true);
                            }
                            #endregion
                        }
                        else
                        {
                            isValidRawUrl = false;
                            LogNet.WriteLog("服务端", "未开启数据开放接口服务,无效的请求url", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//写入日志
                        }
                        break;
                    #endregion
                }
            }
            catch
            {
                result = VisitorInterface.InvalidPostData("");
            }

            if (!isValidRawUrl)
                result = ApiTools.MsgFormat(ApiTools.ResponseCode.操作失败, "无效的请求url", "");

            return result;
        }

        #endregion

        #region 运维平台公安上传
        FKY_GA_Common fky_GA = null;
        Thread th_fkyga = null;
        private void btnSaveGA_Click(object sender, EventArgs e)
        {
            #region 访客数据上传云平台
            if (ckbOpenGA.Checked)
            {
                if (string.IsNullOrEmpty(txtGAAreaName.Text.Trim()))
                {
                    MessageBox.Show("上传区域名称不能为空！");
                    txtGAAreaName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtGAUnitName.Text.Trim()))
                {
                    MessageBox.Show("用户单位名称不能为空！");
                    txtGAUnitName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtGAUnitAddress.Text.Trim()))
                {
                    MessageBox.Show("用户单位地址不能为空！");
                    txtGAUnitAddress.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txt_GAUploadIP.Text.Trim()))
                {
                    MessageBox.Show("数据上传接口不能为空！请输入接口地址。");
                    txt_GAUploadIP.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtGAOrgKey.Text.Trim()))
                {
                    MessageBox.Show("OrgKey不能为空！请输入接口地址。");
                    txtGAOrgKey.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txt_GAUploadName.Text.Trim()))
                {
                    MessageBox.Show("上传账号不能为空！请输入接口地址。");
                    txt_GAUploadName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txt_GAUploadPWD.Text.Trim()))
                {
                    MessageBox.Show("密码接口不能为空！请输入接口地址。");
                    txt_GAUploadPWD.Focus();
                    return;
                }
            }

            SysFunc.SetParamValue("OpenGA", ckbOpenGA.Checked);
            SysFunc.SetParamValue("GAAreaName", txtGAAreaName.Text.Trim());
            SysFunc.SetParamValue("GAUnitName", txtGAUnitName.Text.Trim());
            SysFunc.SetParamValue("GAUnitAddress", txtGAUnitAddress.Text.Trim());
            SysFunc.SetParamValue("GAUploadIP", txt_GAUploadIP.Text.Trim());
            SysFunc.SetParamValue("GAServiceNo", txtGAServiceNo.Text.Trim());
            SysFunc.SetParamValue("GAOrgKey", txtGAOrgKey.Text.Trim());
            SysFunc.SetParamValue("GAUploadName", txt_GAUploadName.Text.Trim());
            SysFunc.SetParamValue("GAUploadPWD", desMethod.EncryptDES(txt_GAUploadPWD.Text.Trim(), desMethod.strKeys));

            if (fky_GA == null)
            {
                fky_GA = new FKY_GA_Common(
                    txt_GAUploadIP.Text.Trim(),
                    txtGAUnitName.Text.Trim(),
                    txtGAServiceNo.Text.Trim(),
                    txtGAAreaName.Text.Trim(),
                    txtGAOrgKey.Text.Trim(),
                    txt_GAUploadName.Text.Trim(),
                    txt_GAUploadPWD.Text.Trim());
            }
            else
            {
                Motify_GA();
            }

            MessageBox.Show("保存成功！");

            #endregion
        }

        private void btnGATest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGAAreaName.Text.Trim()))
            {
                MessageBox.Show("上传区域名称不能为空！");
                txtGAAreaName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtGAUnitName.Text.Trim()))
            {
                MessageBox.Show("用户单位名称不能为空！");
                txtGAUnitName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtGAUnitAddress.Text.Trim()))
            {
                MessageBox.Show("用户单位地址不能为空！");
                txtGAUnitAddress.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txt_GAUploadIP.Text.Trim()))
            {
                MessageBox.Show("数据上传接口不能为空！请输入接口地址。");
                txt_GAUploadIP.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtGAOrgKey.Text.Trim()))
            {
                MessageBox.Show("OrgKey不能为空！请输入接口地址。");
                txtGAOrgKey.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txt_GAUploadName.Text.Trim()))
            {
                MessageBox.Show("上传账号不能为空！请输入接口地址。");
                txt_GAUploadName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txt_GAUploadPWD.Text.Trim()))
            {
                MessageBox.Show("密码接口不能为空！请输入接口地址。");
                txt_GAUploadPWD.Focus();
                return;
            }
            if (fky_GA == null)
            {
                fky_GA = new FKY_GA_Common(
                    txt_GAUploadIP.Text.Trim(),
                    txtGAUnitName.Text.Trim(),
                    txtGAServiceNo.Text.Trim(),
                    txtGAAreaName.Text.Trim(),
                    txtGAOrgKey.Text.Trim(),
                    txt_GAUploadName.Text.Trim(),
                    txt_GAUploadPWD.Text.Trim());
            }
            else
            {
                Motify_GA();
            }
            btnGATest.Enabled = false;
            try
            {
                if (fky_GA.Access_Token())
                {
                    MessageBox.Show(this, "测试成功");
                }
                else
                {
                    MessageBox.Show(this, "测试失败,请检查账号密码等配置信息是否正确！");
                }
            }
            catch (Exception ex)
            {
                FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                MessageBox.Show(this, "网络异常");
            }
            finally
            {
                btnGATest.Enabled = true;
            }
        }
        private void Motify_GA()
        {
            if (fky_GA != null)
            {
                fky_GA.Motify_Community(txtGAUnitName.Text.Trim());
                fky_GA.Motify_SerialNo(txtGAServiceNo.Text.Trim());
                fky_GA.Motify_Area(txtGAAreaName.Text.Trim());
                fky_GA.Motify_OrgKey(txtGAOrgKey.Text.Trim());
                fky_GA.Motify_UploadName(txt_GAUploadName.Text.Trim());
                fky_GA.Motify_UploadPwd(txt_GAUploadPWD.Text.Trim());
            }
        }
        private void UploadFKY_GA_Info()
        {
            while (true)
            {
                U_GA_Info();
                GC.Collect();
                Thread.Sleep(10000);
            }
        }

        private void U_GA_Info()
        {

            if (ckbOpenGA.Checked)
            {
                DataSet dsUploadList = bll_police_gz.GetUploadList();
                foreach (DataRow row in dsUploadList.Tables[0].Rows)
                {
                    try
                    {
                        string visitorID = row["VisitNo"].ToString();
                        string name = row["field2"].ToString();
                        string visitingReasons = row["reasonname"].ToString();
                        string certNo = row["CertNumber"].ToString();
                        string tel = row["VisitorTel"].ToString();
                        string VisitorName = row["VisitorName"].ToString();
                        DateTime intime = DateTime.Parse(row["intime"].ToString());
                        string visitingTime = intime.ToString("yyyy-MM-dd HH:mm:ss");

                        M_VisitList_Info model = new M_VisitList_Info();
                        model.VisitorName = GetNullStr(VisitorName);
                        model.CertNumber = GetNullStr(certNo);
                        model.Field2 = GetNullStr(name);
                        model.VisitNo = GetNullStr(visitorID);
                        model.ReasonName = GetNullStr(visitingReasons);
                        model.VisitorTel = GetNullStr(tel);
                        model.InTime = intime;

                        bool ret = fky_GA.UploadVisitor2GA(model);
                        if (ret)
                        {
                            bll_visitList.UpdatePoliceFlag(visitorID, 1);
                            addRuningLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 访客记录【" + row["VisitorName"].ToString() + "】上传公安成功！");
                        }
                    }
                    catch (Exception ex)
                    {
                        FKY_WCFLibrary.WriteLog.Log4Local(ex.ToString(), true);
                    }
                }
            }
        }
        private void CloseUpload_GA_Thread()
        {
            if (th_fkyga != null)
            {
                try
                {
                    th_fkyga.Abort();
                }
                catch { }
                th_fkyga = null;
            }
        }
        private string GetNullStr(string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str;
        }


        #endregion

        private void txtDataIP_TextChanged(object sender, EventArgs e)
        {
            dataSrvAppId = txtDataIP.Text;
        }
        private void txtDataPort_TextChanged(object sender, EventArgs e)
        {
            dataSrvPort = txtDataPort.Text;
        }
        private void txtDataAppId_TextChanged(object sender, EventArgs e)
        {
            dataSrvAppId = txtDataAppId.Text;
        }
        private void btnDataSrvSave_Click(object sender, EventArgs e)
        {
            SysFunc.SetParamValue("DataSrvIP", txtDataIP.Text);
            SysFunc.SetParamValue("DataSrvPort", txtDataPort.Text);
            SysFunc.SetParamValue("DataSrvAppId", txtDataAppId.Text);
            MessageBox.Show("保存成功");
        }

        private void CloseDataServicesPlatform()
        {
            if (DataSrvPlatformHttpPostRequest.IsListening)
            {
                DataSrvPlatformHttpPostRequest.Stop();
                DataSrvPlatformHttpPostRequest.Prefixes.Clear();
            }
        }

        private void CloseCPSBServicesPlatform()
        {
            if (CPSBHttpPostRequest.IsListening)
            {
                CPSBHttpPostRequest.Stop();
                CPSBHttpPostRequest.Prefixes.Clear();
            }
        }

    }
}
