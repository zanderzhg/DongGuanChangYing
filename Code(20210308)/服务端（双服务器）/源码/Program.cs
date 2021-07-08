using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ADServer.DAL;
using System.Diagnostics;
using System.IO;

namespace ADServer
{
    static class Program
    {
        public static System.Threading.Mutex Run;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            bool noRun = false;
            Run = new System.Threading.Mutex(true, Process.GetCurrentProcess().ProcessName, out noRun);

            if (noRun)
            {
                if (!SysFunc.GetFunctionConfig())
                {
                    InitForm frmInit = new InitForm();
                    if (frmInit.ShowDialog() == DialogResult.OK)
                    {
                        settingAndRunMain();
                    }
                }
                else
                {
                    settingAndRunMain();
                }

            }
            else
            {
                MessageBox.Show("程序已经运行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        private static void settingAndRunMain()
        {
            bool setDatabase = false;
            if (SysFunc.GetFunctionState("ActiveLocal") == "true")
            {
                if (SysFunc.GetParamValue("DbType") != null && ((int)SysFunc.GetParamValue("DbType") == 0 || (int)SysFunc.GetParamValue("DbType") == 1))
                {

                }
                else
                {
                    setDatabase = true;
                }
            }

            if (SysFunc.GetFunctionState("ActivePf") == "true")
            {
                if (SysFunc.GetParamValue("DbTypePf") != null && (int)SysFunc.GetParamValue("DbTypePf") == 1)
                {

                }
                else
                {
                    setDatabase = true;
                }
            }

            if (SysFunc.GetFunctionState("ActinveSJP") == "true")
            {
                if (SysFunc.GetParamValue("DbTypeSJP") != null && (int)SysFunc.GetParamValue("DbTypeSJP") == 1)
                {

                }
                else
                {
                    setDatabase = true;
                }
            }

            if (setDatabase)
            {
                if (new Frm_InitDatabase().ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new ADMain());
                }
            }
            else
            {
                Application.Run(new ADMain());
            }
        }

        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        static bool glExitApp = false;

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\Logs"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Logs");
            }
            string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
            if (!File.Exists(file))
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write("CurrentDomain_UnhandledException");
                sw.Write("IsTerminating : " + e.IsTerminating.ToString());
                sw.Write("IsTerminating : " + e.ExceptionObject.ToString());
                sw.Close();
                fs.Close();
            }

            if (e.ExceptionObject.ToString().Contains("无法打开登录") ||
                     e.ExceptionObject.ToString().Contains("A transport-level error has occurred when receiving results from the server")
                     || e.ExceptionObject.ToString().Contains("provider: Named Pipes Provider, error: 40")
                     || e.ExceptionObject.ToString().Contains("在与 SQL Server 建立连接时出现与网络相关的或特定于实例的错误")
                     || e.ExceptionObject.ToString().Contains("在从服务器接收结果时发生传输级错误")
                     || e.ExceptionObject.ToString().Contains("在操作完成之前超时时间已过或服务器未响应"))
            {
                MessageBox.Show(e.ExceptionObject.ToString());
            }
            else
            {
                MessageBox.Show("系统资源占用过高或发生错误，正在启动缓存清理…");
                Program.Run.Close();
                Application.Restart();
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (!Directory.Exists(Application.StartupPath + "\\Logs"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Logs");
            }
            string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string file = Application.StartupPath + "\\Logs\\" + nowTime + ".txt";
            if (!File.Exists(file))
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write("ThreadException");
                sw.Write("Exception : " + e.Exception.ToString());
                sw.Close();
                fs.Close();
            }

            //if (e.Exception.GetType().Name == "SqlException")
            if (e.Exception.Source == ".Net SqlClient Data Provider")
            {
                MessageBox.Show(e.Exception.Message);
                return;
            }
            else if (e.Exception.Source == "Npgsql")
            {
                if (e.Exception.Message.Contains("FATAL: 3D000"))
                {
                    MessageBox.Show("数据库不存在");
                }
                else if (e.Exception.Message.Contains("Failed to establish a connection to"))
                {
                    MessageBox.Show("数据库IP和端口号访问失败");
                }
                else if (e.Exception.Message.Contains("password authentication failed for user"))
                {
                    MessageBox.Show("数据库用户密码错误");
                }
                else
                {
                    MessageBox.Show(e.Exception.Message.ToString());
                }

            }
            else if (e.Exception.Message.Contains("无法打开登录")
                 || e.Exception.Message.Contains("A transport-level error has occurred when receiving results from the server")
                 || e.Exception.Message.Contains("provider: Named Pipes Provider, error: 40")
                 || e.Exception.Message.Contains("在与 SQL Server 建立连接时出现与网络相关的或特定于实例的错误")
                 || e.Exception.Message.Contains("在从服务器接收结果时发生传输级错误")
                 || e.Exception.Message.Contains("在操作完成之前超时时间已过或服务器未响应"))
            {
                MessageBox.Show("访问数据库失败！");
            }
            else
            {
                MessageBox.Show("系统资源占用过高或发生错误，正在启动缓存清理…");
                Program.Run.Close();
                Application.Restart();
            }
        }

    }
}
