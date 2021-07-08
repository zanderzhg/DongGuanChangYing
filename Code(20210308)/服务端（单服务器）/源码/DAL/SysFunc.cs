using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using ADServer.BLL;
using System.Diagnostics;
using System.Management;
using ADServer.BLL.Face;
using System.Web.Script.Serialization;

namespace ADServer.DAL
{
    public class SysFunc
    {
        #region 释放内存
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);  //释放内存

        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SysFunc.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion

        /// <summary>
        /// 获取guid
        /// </summary>
        /// <returns></returns>
        public static string GetGUID()
        {
            System.Guid guid = System.Guid.NewGuid(); //Guid 类型
            return Guid.NewGuid().ToString(); //直接返回字符串类型
        }

        /// <summary>
        /// 获取配置码，值保存在文本文件内
        /// </summary>
        /// <returns></returns>
        public static string getSettingsKey()
        {
            string guid = GetGUID().Substring(0, 20);
            string path = Application.StartupPath + "\\guidSettingsKey";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, guid);
                return guid;
            }
            else
            {
                using (StreamReader stream = File.OpenText(path))
                {
                    return stream.ReadToEnd();
                }
            }
        }

        #region 获取当前系统平台
        public static string platForm = GetSystemPlatForm();//获取电脑使用平台

        /// <summary>
        /// 获取当前系统平台
        /// </summary>
        /// <returns></returns>
        public static string GetSystemPlatForm()
        {
            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 5:
                            switch (os.Version.Minor)
                            {
                                case 0:
                                    return "Windows 200";
                                case 1:
                                    return "Windows XP";
                                case 2:
                                    return "Windows 2003 ";
                            }
                            break;
                        case 6:
                            switch (os.Version.Minor)
                            {
                                case 0:
                                    return "Windows Vista";
                                case 1:
                                    return "Windows 7";
                            }
                            break;
                    }
                    break;
            }
            return "";
        }
        #endregion

        #region 安装字体
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, // handle to destination window 
        uint Msg, // message 
        int wParam, // first message parameter 
        int lParam // second message parameter 
        );

        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        public static void installFont()
        {

            string WinFontDir = "C:\\windows\\fonts";
            string FontFileName = "C39HrP72DlTt.TTF";
            string FontName = "C39HrP72DlTt";
            int Ret;
            int Res;
            string FontPath;
            const int WM_FONTCHANGE = 0x001D;
            const int HWND_BROADCAST = 0xffff;
            FontPath = WinFontDir + "\\" + FontFileName;
            if (!File.Exists(FontPath))
            {
                File.Copy(System.Windows.Forms.Application.StartupPath + "\\C39HrP72DlTt.TTF", FontPath);
                Ret = AddFontResource(FontPath);

                Res = SendMessage(HWND_BROADCAST, WM_FONTCHANGE, 0, 0);
                Ret = WriteProfileString("fonts", FontName + "(TrueType)", FontFileName);
            }
        }
        #endregion

        #region 获取图片存储路径
        public static string GetSaveFile()
        {
            System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
            saveFile.Title = "save file";
            saveFile.OverwritePrompt = true;
            saveFile.CreatePrompt = true;
            saveFile.AddExtension = true;
            //saveFile.Filter = "file(*.txt)|*.txt|all file(*.*)|(*.*)";
            saveFile.Filter = "JPeg Image(*.jpg)|*.jpg|Bitmap Image(*.bmp)|*.bmp|Gif Image(*.gif)|*.gif|All Image(*.*)|*.*";
            ;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                return saveFile.FileName;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 图片打印方法导入
        [DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        #endregion

        #region 获取图片二进制流
        public static byte[] GetImage(string filePath)
        {
            if (filePath != null)
            {
                BinaryReader reader = null;
                FileStream myFileStream = new FileStream(filePath, FileMode.Open);
                reader = new BinaryReader(myFileStream);
                byte[] image = reader.ReadBytes((int)myFileStream.Length);//存储图片到数组中。
                reader.Close();
                myFileStream.Close();
                return image;
            }
            else
            {
                return new byte[1];
            }
        }
        #endregion

        #region 获取内存图片
        public static Image GetMsImage(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            int byteLength = (int)fs.Length;
            byte[] fileBytes = new byte[byteLength];
            fs.Read(fileBytes, 0, byteLength);
            fs.Close();

            MemoryStream ms = new MemoryStream(fileBytes);
            Image img = Image.FromStream(ms);
            return img;
        }
        #endregion

        #region 程序运行上级目录获取
        public static string GetImagePath()
        {
            string path = System.Windows.Forms.Application.StartupPath;
            int aa1 = path.LastIndexOf("\\");
            int aa2 = path.Substring(0, aa1).LastIndexOf("\\");
            string imgPath = path.Substring(0, aa2) + "\\image\\227.jpg";
            return imgPath;
        }

        public static string GetImgPath()
        {
            string path = System.Windows.Forms.Application.StartupPath;
            //int aa1 = path.LastIndexOf("\\");
            //int aa2 = path.Substring(0, aa1).LastIndexOf("\\");
            //string imgPath = path.Substring(0, aa2) + "\\img";
            //return imgPath;
            return path;
        }
        #endregion

        #region 加解密
        public static string encode(string str)
        {
            string htext = "";

            for (int i = 0; i < str.Length; i++)
            {
                htext = htext + (char)(str[i] + 10 - 1 * 2);
            }
            return htext;
        }


        public static string decode(string str)
        {
            string dtext = "";

            for (int i = 0; i < str.Length; i++)
            {
                dtext = dtext + (char)(str[i] - 10 + 1 * 2);
            }
            return dtext;
        }
        #endregion

        //字节数组转换十六进制

        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

        public static string ByteArrayToHexStringNoSpace(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }

        //十六进制转换字节数组
        public static byte[] HexStringToByteArray(string s)
        {

            s = s.Replace(" ", "");

            byte[] buffer = new byte[s.Length / 2];

            for (int i = 0; i < s.Length; i += 2)

                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);

            return buffer;

        }

        public static string ConvertTo1(int seq)   //针对感应式读卡器
        {
            string tempStr = Convert.ToString(seq, 2);   //转成二进制
            //string LongStr = tempStr.PadLeft(24, '0');            //不够24位则在前面补0
            string LongStr = tempStr;
            if (tempStr.Length > 24)
                LongStr = tempStr.Substring(tempStr.Length - 24, 24);   //只保留后面的24位
            string Str = LongStr.Replace('0', 'k').Replace('1', '0').Replace('k', '1');   //按位取反
            string reStr = Convert.ToUInt32(Str, 2).ToString();
            return reStr;
        }

        public static string ConvertToJl(string icCardno)
        {
            try
            {
                string ret = icCardno.Substring(6, 2) + icCardno.Substring(4, 2) + icCardno.Substring(2, 2) + icCardno.Substring(0, 2);
                return ret;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 命令中止导出Excel
        /// </summary>
        private static bool stopExport = false;

        public static bool StopExport
        {
            get { return stopExport; }
            set { stopExport = value; }
        }

        #region 写入Excel文档(需要新建文件)
        /// <summary>
        /// 写入Excel文档
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="ds2Excel">被导出的数据集</param>

        //public static int SaveFP2toExcel2(string Path, DataTable ds2Excel)
        //{
        //    try
        //    {
        //        //创建Excel应用程序

        //        Excel.Application xApp = new Excel.ApplicationClass();
        //        if (xApp == null)
        //        {
        //            MessageBox.Show("错误：Excel不能打开！");
        //            return 1;
        //        }
        //        object objOpt = Missing.Value;
        //        Excel.Workbook xBook = xApp.Workbooks.Add(true);//添加新工作簿
        //        Excel.Sheets xSheets = xBook.Sheets;
        //        Excel._Worksheet xSheet = null;
        //        //
        //        //创建空的sheet
        //        //
        //        xSheet = (Excel._Worksheet)(xBook.Sheets.Add(objOpt, objOpt, objOpt, objOpt));
        //        xSheet.Cells.NumberFormatLocal = "@";
        //        if (xSheet == null)
        //        {
        //            MessageBox.Show("错误：工作表为空！");
        //            return 1;
        //        }
        //        //写数据集表头
        //        for (int k = 0; k < ds2Excel.Columns.Count; k++)
        //            xSheet.Cells[1, k + 1] = ds2Excel.Columns[k].ColumnName.ToString().Trim();
        //        //写数据集数据
        //        for (int i = 0; i < ds2Excel.Rows.Count; i++)
        //        {
        //            if (StopExport)
        //            {
        //                break;
        //            }
        //            for (int j = 0; j < ds2Excel.Columns.Count; j++)
        //                xSheet.Cells[i + 2, j + 1] = ds2Excel.Rows[i][j].ToString();
        //        }
        //        //保存文件
        //        xBook.Saved = true;
        //        xBook.SaveCopyAs(Path);

        //        //显示文件
        //        xApp.Visible = true;
        //        //
        //        //释放资源
        //        //
        //        Marshal.ReleaseComObject(xSheet);
        //        xSheet = null;
        //        Marshal.ReleaseComObject(xSheets);
        //        xSheets = null;

        //        Marshal.ReleaseComObject(xBook);
        //        xBook = null;
        //        xApp.Quit();
        //        Marshal.ReleaseComObject(xApp);
        //        xApp = null;
        //        GC.Collect();//强行销毁
        //        MessageBox.Show("导出成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("写入Excel发生错误：" + ex.Message);
        //    }
        //    return 0;
        //}
        #endregion

        #region 写入Excel文档模板，只写表头，不写数据(需要新建文件)
        /// <summary>
        /// 写入Excel文档
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="ds2Excel">被导出的数据集</param>

        //public static void SaveFP2toHeadExcel2(string Path, DataSet ds2Excel)
        //{
        //    try
        //    {
        //        //创建Excel应用程序

        //        Excel.Application xApp = new Excel.ApplicationClass();
        //        if (xApp == null)
        //        {
        //            MessageBox.Show("错误：Excel不能打开！");
        //            return;
        //        }
        //        object objOpt = Missing.Value;
        //        Excel.Workbook xBook = xApp.Workbooks.Add(true);//添加新工作簿
        //        Excel.Sheets xSheets = xBook.Sheets;
        //        Excel._Worksheet xSheet = null;
        //        //
        //        //创建空的sheet
        //        //
        //        xSheet = (Excel._Worksheet)(xBook.Sheets.Add(objOpt, objOpt, objOpt, objOpt));
        //        xSheet.Cells.NumberFormatLocal = "@";
        //        if (xSheet == null)
        //        {
        //            MessageBox.Show("错误：工作表为空！");
        //            return;
        //        }
        //        //写数据集表头
        //        for (int k = 0; k < ds2Excel.Tables[0].Columns.Count; k++)
        //            xSheet.Cells[1, k + 1] = ds2Excel.Tables[0].Columns[k].ColumnName.ToString().Trim();
        //        //写数据集数据
        //        if (ds2Excel.Tables[0].Rows.Count > 2)
        //        {
        //            for (int i = 0; i < 2; i++)
        //            {
        //                for (int j = 0; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j + 1] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < ds2Excel.Tables[0].Columns.Count; i++)
        //            {
        //                for (int j = 0; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j + 1] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        //保存文件
        //        xBook.Saved = true;
        //        xBook.SaveCopyAs(Path);

        //        //显示文件
        //        xApp.Visible = true;
        //        //
        //        //释放资源
        //        //
        //        Marshal.ReleaseComObject(xSheet);
        //        xSheet = null;
        //        Marshal.ReleaseComObject(xSheets);
        //        xSheets = null;

        //        Marshal.ReleaseComObject(xBook);
        //        xBook = null;
        //        xApp.Quit();
        //        Marshal.ReleaseComObject(xApp);
        //        xApp = null;
        //        GC.Collect();//强行销毁
        //        MessageBox.Show("导出成功");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("写入Excel发生错误：" + ex.Message);
        //    }
        //}
        #endregion

        #region 写入Excel文档模板，只写表头(不要第一列)，不写数据(需要新建文件)
        /// <summary>
        /// 写入Excel文档,只写表头
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="ds2Excel">被导出的数据集</param>

        //public static void SaveFP2toExcelHead2(string Path, DataSet ds2Excel)
        //{
        //    try
        //    {
        //        //创建Excel应用程序

        //        Excel.Application xApp = new Excel.ApplicationClass();
        //        if (xApp == null)
        //        {
        //            MessageBox.Show("错误：Excel不能打开！");
        //            return;
        //        }
        //        object objOpt = Missing.Value;
        //        Excel.Workbook xBook = xApp.Workbooks.Add(true);//添加新工作簿
        //        Excel.Sheets xSheets = xBook.Sheets;
        //        Excel._Worksheet xSheet = null;
        //        //
        //        //创建空的sheet
        //        //
        //        xSheet = (Excel._Worksheet)(xBook.Sheets.Add(objOpt, objOpt, objOpt, objOpt));
        //        xSheet.Cells.NumberFormatLocal = "@";
        //        if (xSheet == null)
        //        {
        //            MessageBox.Show("错误：工作表为空！");
        //            return;
        //        }

        //        //写数据集表头
        //        for (int k = 1; k < ds2Excel.Tables[0].Columns.Count; k++)
        //            xSheet.Cells[1, k] = ds2Excel.Tables[0].Columns[k].ColumnName.ToString().Trim();

        //        //写数据集数据
        //        if (ds2Excel.Tables[0].Rows.Count < 2)   //数据小于三行的按实际行数导出
        //        {
        //            for (int i = 0; i < ds2Excel.Tables[0].Rows.Count; i++)
        //            {
        //                for (int j = 1; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        else    //数据大于等于三行的按2行导出 
        //        {
        //            for (int i = 0; i < 2; i++)
        //            {
        //                for (int j = 1; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        //保存文件
        //        xBook.Saved = true;
        //        xBook.SaveCopyAs(Path);

        //        //显示文件
        //        xApp.Visible = true;
        //        //
        //        //释放资源
        //        //
        //        Marshal.ReleaseComObject(xSheet);
        //        xSheet = null;
        //        Marshal.ReleaseComObject(xSheets);
        //        xSheets = null;

        //        Marshal.ReleaseComObject(xBook);
        //        xBook = null;
        //        xApp.Quit();
        //        Marshal.ReleaseComObject(xApp);
        //        xApp = null;
        //        GC.Collect();//强行销毁
        //        MessageBox.Show("导出成功");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("写入Excel发生错误：" + ex.Message);
        //    }
        //}
        #endregion

        #region 写入Excel文档(不需要新建文件)
        /// <summary>
        /// 写入Excel文档
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="ds2Excel">被导出的数据集</param>

        //public static int SaveFP2toExcel1(string Path, DataTable ds2Excel)
        //{
        //    try
        //    {
        //        //创建Excel应用程序
        //        Excel.Application xApp = new Excel.ApplicationClass();
        //        if (xApp == null)
        //        {
        //            MessageBox.Show("错误：Excel不能打开！");
        //            return 1;
        //        }
        //        //打开指定路径的Excel文件
        //        Excel._Workbook xBook = xApp.Workbooks.Open(Path, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //        Excel.Sheets xSheets = xBook.Worksheets;
        //        Excel._Worksheet xSheet = (Excel._Worksheet)xSheets.get_Item(1);
        //        xSheet.Cells.NumberFormatLocal = "@";
        //        if (xSheet == null)
        //        {
        //            MessageBox.Show("错误：工作表为空！");
        //            return 1;
        //        }
        //        //写数据集表头
        //        for (int k = 0; k < ds2Excel.Columns.Count; k++)
        //            xSheet.Cells[1, k + 1] = ds2Excel.Columns[k].ColumnName.ToString().Trim();
        //        //写数据集数据
        //        for (int i = 0; i < ds2Excel.Rows.Count; i++)
        //        {
        //            if (StopExport)
        //            {
        //                break;
        //            }
        //            for (int j = 0; j < ds2Excel.Columns.Count; j++)
        //                xSheet.Cells[i + 2, j + 1] = ds2Excel.Rows[i][j].ToString();
        //        }
        //        //保存文件
        //        xBook.Save();
        //        //显示文件
        //        xApp.Visible = true;
        //        //
        //        //释放资源
        //        //
        //        Marshal.ReleaseComObject(xSheet);
        //        xSheet = null;
        //        Marshal.ReleaseComObject(xSheets);
        //        xSheets = null;

        //        Marshal.ReleaseComObject(xBook);
        //        xBook = null;
        //        xApp.Quit();
        //        Marshal.ReleaseComObject(xApp);
        //        xApp = null;
        //        GC.Collect();//强行销毁
        //        MessageBox.Show("导出成功");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("写入Excel发生错误：" + ex.Message);
        //    }
        //    return 0;
        //}

        #endregion

        #region 写入Excel文档,只写表头，不写数据(不需要新建文件)
        /// <summary>
        /// 写入Excel文档
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="ds2Excel">被导出的数据集</param>

        //public static void SaveFP2toHeadExcel1(string Path, DataSet ds2Excel)
        //{
        //    try
        //    {
        //        //创建Excel应用程序
        //        Excel.Application xApp = new Excel.ApplicationClass();
        //        if (xApp == null)
        //        {
        //            MessageBox.Show("错误：Excel不能打开！");
        //            return;
        //        }
        //        //打开指定路径的Excel文件
        //        Excel._Workbook xBook = xApp.Workbooks.Open(Path, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //        Excel.Sheets xSheets = xBook.Worksheets;
        //        Excel._Worksheet xSheet = (Excel._Worksheet)xSheets.get_Item(1);
        //        xSheet.Cells.NumberFormatLocal = "@";
        //        if (xSheet == null)
        //        {
        //            MessageBox.Show("错误：工作表为空！");
        //            return;
        //        }
        //        //写数据集表头
        //        for (int k = 0; k < ds2Excel.Tables[0].Columns.Count; k++)
        //            xSheet.Cells[1, k + 1] = ds2Excel.Tables[0].Columns[k].ColumnName.ToString().Trim();
        //        //写数据集数据

        //        if (ds2Excel.Tables[0].Rows.Count < 2)   //数据小于三行的按实际行数导出
        //        {
        //            for (int i = 0; i < ds2Excel.Tables[0].Rows.Count; i++)
        //            {
        //                for (int j = 0; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j + 1] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        else    //数据大于等于三行的按2行导出 
        //        {
        //            for (int i = 0; i < 2; i++)
        //            {
        //                for (int j = 0; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j + 1] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }

        //        //保存文件
        //        xBook.Save();
        //        //显示文件
        //        xApp.Visible = true;
        //        //
        //        //释放资源
        //        //
        //        Marshal.ReleaseComObject(xSheet);
        //        xSheet = null;
        //        Marshal.ReleaseComObject(xSheets);
        //        xSheets = null;

        //        Marshal.ReleaseComObject(xBook);
        //        xBook = null;
        //        xApp.Quit();
        //        Marshal.ReleaseComObject(xApp);
        //        xApp = null;
        //        GC.Collect();//强行销毁
        //        MessageBox.Show("导出成功");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("写入Excel发生错误：" + ex.Message);
        //    }
        //}

        #endregion

        #region 写入Excel文档,只写表头(不要第一列)，不写数据(不需要新建文件)
        /// <summary>
        /// 写入Excel文档模板，不写数据
        /// </summary>
        /// <param name="Path">文件名称</param>
        /// <param name="ds2Excel">被导出的数据集</param>

        //public static void SaveFP2toExcelHead1(string Path, DataSet ds2Excel)
        //{
        //    try
        //    {
        //        //创建Excel应用程序
        //        Excel.Application xApp = new Excel.ApplicationClass();
        //        if (xApp == null)
        //        {
        //            MessageBox.Show("错误：Excel不能打开！");
        //            return;
        //        }
        //        //打开指定路径的Excel文件
        //        Excel._Workbook xBook = xApp.Workbooks.Open(Path, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        //        Excel.Sheets xSheets = xBook.Worksheets;
        //        Excel._Worksheet xSheet = (Excel._Worksheet)xSheets.get_Item(1);
        //        xSheet.Cells.NumberFormatLocal = "@";
        //        if (xSheet == null)
        //        {
        //            MessageBox.Show("错误：工作表为空！");
        //            return;
        //        }
        //        //写数据集表头
        //        for (int k = 1; k < ds2Excel.Tables[0].Columns.Count; k++)
        //            xSheet.Cells[1, k] = ds2Excel.Tables[0].Columns[k].ColumnName.ToString().Trim();
        //        //写数据集数据
        //        if (ds2Excel.Tables[0].Rows.Count < 2)   //数据小于三行的按实际行数导出
        //        {
        //            for (int i = 0; i < ds2Excel.Tables[0].Rows.Count; i++)
        //            {
        //                for (int j = 1; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        else    //数据大于等于三行的按2行导出 
        //        {
        //            for (int i = 0; i < 2; i++)
        //            {
        //                for (int j = 1; j < ds2Excel.Tables[0].Columns.Count; j++)
        //                    xSheet.Cells[i + 2, j] = ds2Excel.Tables[0].Rows[i][j].ToString();
        //            }
        //        }
        //        //保存文件
        //        xBook.Save();
        //        //显示文件
        //        xApp.Visible = true;
        //        //
        //        //释放资源
        //        //
        //        Marshal.ReleaseComObject(xSheet);
        //        xSheet = null;
        //        Marshal.ReleaseComObject(xSheets);
        //        xSheets = null;

        //        Marshal.ReleaseComObject(xBook);
        //        xBook = null;
        //        xApp.Quit();
        //        Marshal.ReleaseComObject(xApp);
        //        xApp = null;
        //        GC.Collect();//强行销毁
        //        MessageBox.Show("导出成功"); MessageBox.Show("导出成功");

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("写入Excel发生错误：" + ex.Message);
        //    }
        //}

        #endregion

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDangerSqlString(string str)
        {
            return Regex.IsMatch(str, @"[;|,|\/|%|@|\*|!|\']");
        }

        /// <summary>
        /// 获取证件照片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] GetCertImg(string filename)
        {
            byte[] imageData = null;
            string filePath = Application.StartupPath + "\\CertPhoto\\" + filename;
            if (File.Exists(filePath))
            {
                FileInfo f = new FileInfo(filePath);
                FileStream fs = f.OpenRead();
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, (int)fs.Length);
                fs.Close();
                imageData = bt;
            }
            else
            {
                imageData = new byte[1];
            }

            return imageData;
        }

        /// <summary>
        /// 获取现场拍照照片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] GetCatchPhotoImg(string filename)
        {
            byte[] imageData = null;
            string filePath = Application.StartupPath + "\\CatchPhoto\\" + filename;
            if (File.Exists(filePath))
            {
                FileInfo f = new FileInfo(filePath);
                FileStream fs = f.OpenRead();
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, (int)fs.Length);
                fs.Close();
                imageData = bt;
            }
            else
            {
                imageData = new byte[1];
            }

            return imageData;
        }

        /// <summary>
        /// 获取现场拍照照片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] GetQrCodeImg(string filename)
        {
            byte[] imageData = null;
            string filePath = Application.StartupPath + "\\QrCode\\" + filename;
            if (File.Exists(filePath))
            {
                FileInfo f = new FileInfo(filePath);
                FileStream fs = f.OpenRead();
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, (int)fs.Length);
                fs.Close();
                imageData = bt;
            }
            else
            {
                imageData = new byte[1];
            }

            return imageData;
        }

        /// <summary>
        /// 获取功能激活的配置文件
        /// </summary>
        /// <returns></returns>
        public static bool GetFunctionConfig()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Application.StartupPath + "\\FunctionConfig.xml");

                XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
                XmlNode node = xmldocSelect.SelectSingleNode("Function").FirstChild;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取系统参数设置值
        /// </summary>
        /// <param name="param">参数名</param>
        /// <returns></returns>
        public static object GetParamValue(string param, string xmlFile = "")
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (xmlFile != "")
                {
                    xmlDoc.Load(xmlFile);
                }
                else
                {
                    xmlDoc.Load(Application.StartupPath + "\\Config.xml");
                }

                XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
                XmlNode node = xmldocSelect.SelectSingleNode(param).FirstChild;
                string dataType = node.Attributes["type"].Value;
                string dataValue = node.InnerText;
                object value = null;
                switch (dataType)
                {
                    case "string":
                        value = dataValue;
                        break;
                    case "bool":
                        value = bool.Parse(dataValue);
                        break;
                    case "int":
                        value = int.Parse(dataValue);
                        break;
                    default:
                        break;
                }
                return value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置系统参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="value">参数值</param>
        public static void SetParamValue(string paramName, object value, string xmlFile = "")
        {
            XmlDocument xmlDoc = new XmlDocument();
            if (xmlFile != "")
            {
                xmlDoc.Load(xmlFile);
            }
            else
            {
                xmlDoc.Load(Application.StartupPath + "\\Config.xml");
            }

            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNode node = xmldocSelect.SelectSingleNode(paramName).FirstChild;
            node.InnerText = value.ToString();
            if (xmlFile != "")
            {
                xmlDoc.Save(xmlFile);
            }
            else
            {
                xmlDoc.Save(Application.StartupPath + "\\Config.xml");
            }

        }

        /// <summary>
        /// 初始化配置文件 
        /// </summary>
        public static void InitConfig()
        {
            if (!File.Exists(Application.StartupPath + "\\Config.xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                CreateRootElement();
            }
        }

        public static void CreateRootElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmldecl);
            XmlNode elementConfigs = xmlDoc.CreateElement("Configs");
            xmlDoc.AppendChild(elementConfigs);

            XmlNode elementRegis = xmlDoc.CreateElement("RegistrationField");
            elementConfigs.AppendChild(elementRegis);

            XmlElement elementTelVoice = xmlDoc.CreateElement("TelVoice");
            elementConfigs.AppendChild(elementTelVoice);

            XmlElement elementFastMode = xmlDoc.CreateElement("FastMode");
            elementConfigs.AppendChild(elementFastMode);

            XmlElement elementFastModeGraPhoto = xmlDoc.CreateElement("FastModeGraPhoto");
            elementConfigs.AppendChild(elementFastModeGraPhoto);

            XmlElement elementWGGrantDoors = xmlDoc.CreateElement("WGGrantDoors");
            elementConfigs.AppendChild(elementWGGrantDoors);

            XmlElement elementBookVoice = xmlDoc.CreateElement("BookVoice");
            elementConfigs.AppendChild(elementBookVoice);

            XmlElement elementVipVoice = xmlDoc.CreateElement("VipVoice");
            elementConfigs.AppendChild(elementVipVoice);

            XmlElement elementBlackVoice = xmlDoc.CreateElement("BlackVoice");
            elementConfigs.AppendChild(elementBlackVoice);

            XmlElement elementPrintType = xmlDoc.CreateElement("PrintType");
            elementConfigs.AppendChild(elementPrintType);

            XmlElement elementIcPrint = xmlDoc.CreateElement("IcPrint");
            elementConfigs.AppendChild(elementIcPrint);

            XmlElement elementFKServiceUrl = xmlDoc.CreateElement("FKServiceUrl");
            elementConfigs.AppendChild(elementFKServiceUrl);

            XmlElement elementOpenFKService = xmlDoc.CreateElement("OpenFKService");
            elementConfigs.AppendChild(elementOpenFKService);

            XmlElement elementUserName = xmlDoc.CreateElement("UserName");
            elementConfigs.AppendChild(elementUserName);

            XmlElement elementUserPWD = xmlDoc.CreateElement("UserPWD");
            elementConfigs.AppendChild(elementUserPWD);

            XmlElement elementIsCheck = xmlDoc.CreateElement("IsCheck");
            elementConfigs.AppendChild(elementIsCheck);

            XmlElement elementLeaveType = xmlDoc.CreateElement("LeaveType");
            elementConfigs.AppendChild(elementLeaveType);

            XmlElement elementPort = xmlDoc.CreateElement("Port");
            elementConfigs.AppendChild(elementPort);

            XmlElement elementShowLeave = xmlDoc.CreateElement("ShowLeave");
            elementConfigs.AppendChild(elementShowLeave);

            XmlElement elementHideType = xmlDoc.CreateElement("HideType");
            elementConfigs.AppendChild(elementHideType);

            XmlElement elementShowLastVisit = xmlDoc.CreateElement("ShowLastVisit");
            elementConfigs.AppendChild(elementShowLastVisit);

            XmlElement elementMachineKind = xmlDoc.CreateElement("MachineKind");
            elementConfigs.AppendChild(elementMachineKind);

            XmlElement elementSerialPort = xmlDoc.CreateElement("SerialPort");
            elementConfigs.AppendChild(elementSerialPort);

            XmlElement elementIpPort = xmlDoc.CreateElement("IpPort");
            elementConfigs.AppendChild(elementIpPort);

            XmlElement elementIP = xmlDoc.CreateElement("IP");
            elementConfigs.AppendChild(elementIP);

            XmlElement elementScanMachineKind = xmlDoc.CreateElement("ScanMachineKind");
            elementConfigs.AppendChild(elementScanMachineKind);

            XmlElement elementPhoneKind = xmlDoc.CreateElement("PhoneKind");
            elementConfigs.AppendChild(elementPhoneKind);

            XmlElement elementPhonePort = xmlDoc.CreateElement("PhonePort");
            elementConfigs.AppendChild(elementPhonePort);

            XmlElement elementCallSet = xmlDoc.CreateElement("CallSet");
            elementConfigs.AppendChild(elementCallSet);

            XmlElement elementGateSentry = xmlDoc.CreateElement("GateSentry");
            elementConfigs.AppendChild(elementGateSentry);

            XmlElement elementRedPort = xmlDoc.CreateElement("RedPort");
            elementConfigs.AppendChild(elementRedPort);

            XmlElement elementOnlyID = xmlDoc.CreateElement("onlyID");
            elementConfigs.AppendChild(elementOnlyID);

            XmlElement elementIsOpen = xmlDoc.CreateElement("IsOpen");
            elementConfigs.AppendChild(elementIsOpen);

            XmlElement elementIsService = xmlDoc.CreateElement("IsService");
            elementConfigs.AppendChild(elementIsService);

            XmlElement elementServicePath = xmlDoc.CreateElement("ServicePath");
            elementConfigs.AppendChild(elementServicePath);

            XmlElement elementFiled1 = xmlDoc.CreateElement("Filed1");
            elementConfigs.AppendChild(elementFiled1);

            XmlElement elementFiled2 = xmlDoc.CreateElement("Filed2");
            elementConfigs.AppendChild(elementFiled2);

            XmlElement elementIsShowLeave = xmlDoc.CreateElement("IsShowLeave");
            elementConfigs.AppendChild(elementIsShowLeave);

            XmlElement elementAutoRunSS = xmlDoc.CreateElement("AutoRunSS");
            elementConfigs.AppendChild(elementAutoRunSS);

            XmlElement elementIcCardDay = xmlDoc.CreateElement("IcCardDay");
            elementConfigs.AppendChild(elementIcCardDay);

            XmlElement elementEquipment = xmlDoc.CreateElement("Equipment");
            elementConfigs.AppendChild(elementEquipment);

            XmlElement elementICCardReader = xmlDoc.CreateElement("ICCardReader");
            elementConfigs.AppendChild(elementICCardReader);

            XmlElement elementDbType = xmlDoc.CreateElement("DbType");
            elementConfigs.AppendChild(elementDbType);

            XmlElement elementDbServername = xmlDoc.CreateElement("DbServername");
            elementConfigs.AppendChild(elementDbServername);

            XmlElement elementDbName = xmlDoc.CreateElement("DbName");
            elementConfigs.AppendChild(elementDbName);

            XmlElement elementDbUser = xmlDoc.CreateElement("DbUser");
            elementConfigs.AppendChild(elementDbUser);

            XmlElement elementDbPwd = xmlDoc.CreateElement("DbPwd");
            elementConfigs.AppendChild(elementDbPwd);

            XmlElement elementDbTypeSJP = xmlDoc.CreateElement("DbTypeSJP");
            elementConfigs.AppendChild(elementDbTypeSJP);

            XmlElement elementDbServernameSJP = xmlDoc.CreateElement("DbServernameSJP");
            elementConfigs.AppendChild(elementDbServernameSJP);

            XmlElement elementDbNameSJP = xmlDoc.CreateElement("DbNameSJP");
            elementConfigs.AppendChild(elementDbNameSJP);

            XmlElement elementDbUserSJP = xmlDoc.CreateElement("DbUserSJP");
            elementConfigs.AppendChild(elementDbUserSJP);

            XmlElement elementDbPwdSJP = xmlDoc.CreateElement("DbPwdSJP");
            elementConfigs.AppendChild(elementDbPwdSJP);

            XmlElement elementSysPwd = xmlDoc.CreateElement("SysPwd");
            elementConfigs.AppendChild(elementSysPwd);

            XmlElement elementCRUserId = xmlDoc.CreateElement("CRUserId");
            elementConfigs.AppendChild(elementCRUserId);

            XmlElement elementScanTypes = xmlDoc.CreateElement("ScanTypes");
            elementConfigs.AppendChild(elementScanTypes);

            XmlElement elementAutoDeleteTelRec = xmlDoc.CreateElement("AutoDeleteTelRec");
            elementConfigs.AppendChild(elementAutoDeleteTelRec);

            XmlElement elementDeleteTelRecDays = xmlDoc.CreateElement("DeleteTelRecDays");
            elementConfigs.AppendChild(elementDeleteTelRecDays);

            XmlElement elementTelRecMaxMinutes = xmlDoc.CreateElement("TelRecMaxMinutes");
            elementConfigs.AppendChild(elementTelRecMaxMinutes);

            XmlElement elementOpenPF = xmlDoc.CreateElement("OpenPF");
            elementConfigs.AppendChild(elementOpenPF);

            XmlElement elementPFUploadADRecord = xmlDoc.CreateElement("PFUploadADRecord");
            elementConfigs.AppendChild(elementPFUploadADRecord);

            XmlElement elementPFUrl = xmlDoc.CreateElement("PFUrl");
            elementConfigs.AppendChild(elementPFUrl);

            XmlElement elementPFUserName = xmlDoc.CreateElement("PFUserName");
            elementConfigs.AppendChild(elementPFUserName);

            XmlElement elementPFUserPwd = xmlDoc.CreateElement("PFUserPwd");
            elementConfigs.AppendChild(elementPFUserPwd);

            XmlElement elementPFToken = xmlDoc.CreateElement("PFToken");
            elementConfigs.AppendChild(elementPFToken);

            XmlElement elementWGGrantDoorsWeixin = xmlDoc.CreateElement("WGGrantDoorsWeixin");
            elementConfigs.AppendChild(elementWGGrantDoorsWeixin);

            XmlElement elementWeixinAccount = xmlDoc.CreateElement("WeixinAccount");
            elementConfigs.AppendChild(elementWeixinAccount);

            XmlElement elementWeixinDownloadEmpLastTime = xmlDoc.CreateElement("WeixinDownloadEmpLastTime");
            elementConfigs.AppendChild(elementWeixinDownloadEmpLastTime);

            XmlElement elementWeixinDownloadAuto = xmlDoc.CreateElement("WeixinDownloadAuto");
            elementConfigs.AppendChild(elementWeixinDownloadAuto);

            XmlElement elementWeixinDownloadTime = xmlDoc.CreateElement("WeixinDownloadTime");
            elementConfigs.AppendChild(elementWeixinDownloadTime);

            XmlElement elementSJPDownloadEmpLastTime = xmlDoc.CreateElement("SJPDownloadEmpLastTime");
            elementConfigs.AppendChild(elementSJPDownloadEmpLastTime);

            XmlElement elementSJPDownloadAuto = xmlDoc.CreateElement("SJPDownloadAuto");
            elementConfigs.AppendChild(elementSJPDownloadAuto);

            XmlElement elementSJPDownloadTime = xmlDoc.CreateElement("SJPDownloadTime");
            elementConfigs.AppendChild(elementSJPDownloadTime);

            XmlElement elementSJPCompanyName = xmlDoc.CreateElement("SJPCompanyName");
            elementConfigs.AppendChild(elementSJPCompanyName);

            XmlElement elementSmsCheckInContent = xmlDoc.CreateElement("SmsCheckInContent");
            elementConfigs.AppendChild(elementSmsCheckInContent);

            XmlElement elementSmsLeaveContent = xmlDoc.CreateElement("SmsLeaveContent");
            elementConfigs.AppendChild(elementSmsLeaveContent);

            XmlElement elementAccessControlType = xmlDoc.CreateElement("AccessControlType");
            elementConfigs.AppendChild(elementAccessControlType);

            XmlElement elementAutoDeleteOverdueCard = xmlDoc.CreateElement("AutoDeleteOverdueCard");
            elementConfigs.AppendChild(elementAutoDeleteOverdueCard);

            XmlElement elementDbTypePf = xmlDoc.CreateElement("DbTypePf");
            elementConfigs.AppendChild(elementDbTypePf);

            XmlElement elementDbServernamePf = xmlDoc.CreateElement("DbServernamePf");
            elementConfigs.AppendChild(elementDbServernamePf);

            XmlElement elementDbNamePf = xmlDoc.CreateElement("DbNamePf");
            elementConfigs.AppendChild(elementDbNamePf);

            XmlElement elementDbUserPf = xmlDoc.CreateElement("DbUserPf");
            elementConfigs.AppendChild(elementDbUserPf);

            XmlElement elementDbPwdPf = xmlDoc.CreateElement("DbPwdPf");
            elementConfigs.AppendChild(elementDbPwdPf);

            XmlElement elementPfServerIp = xmlDoc.CreateElement("PfServerIp");
            elementConfigs.AppendChild(elementPfServerIp);

            XmlElement elementPfServerPort = xmlDoc.CreateElement("PfServerPort");
            elementConfigs.AppendChild(elementPfServerPort);

            XmlElement elementBaiduAK = xmlDoc.CreateElement("BaiduAK");
            elementConfigs.AppendChild(elementBaiduAK);

            XmlElement elementOpenFace = xmlDoc.CreateElement("OpenFaceService");
            elementConfigs.AppendChild(elementOpenFace);

            XmlElement elementFaceIP = xmlDoc.CreateElement("FaceIP");
            elementConfigs.AppendChild(elementFaceIP);

            XmlElement elementFacePort = xmlDoc.CreateElement("FacePort");
            elementConfigs.AppendChild(elementFacePort);

            XmlElement elementFaceUser = xmlDoc.CreateElement("FaceUser");
            elementConfigs.AppendChild(elementFaceUser);

            XmlElement elementFacePwd = xmlDoc.CreateElement("FacePwd");
            elementConfigs.AppendChild(elementFacePwd);

            XmlElement elementFacePic = xmlDoc.CreateElement("FacePic");
            elementConfigs.AppendChild(elementFacePic);

            XmlElement elementFaceServerIP = xmlDoc.CreateElement("FaceServerIP");
            elementConfigs.AppendChild(elementFaceServerIP);

            XmlElement elementFaceServerPort = xmlDoc.CreateElement("FaceServerPort");
            elementConfigs.AppendChild(elementFaceServerPort);

            XmlElement elementFaceServerInterface = xmlDoc.CreateElement("FaceServerInterface");
            elementConfigs.AppendChild(elementFaceServerInterface);

            XmlElement elementFaceServerType = xmlDoc.CreateElement("FaceServerType");
            elementConfigs.AppendChild(elementFaceServerType);

            XmlElement elementWeiXinServerType = xmlDoc.CreateElement("WeiXinServerType");
            elementConfigs.AppendChild(elementWeiXinServerType);

            XmlElement elementCTIDUrl = xmlDoc.CreateElement("CTIDUrl");
            elementConfigs.AppendChild(elementCTIDUrl);
            XmlElement elementCTIDClientId = xmlDoc.CreateElement("CTIDClientId");
            elementConfigs.AppendChild(elementCTIDClientId);
            XmlElement elementCTIDClientSecret = xmlDoc.CreateElement("CTIDClientSecret");
            elementConfigs.AppendChild(elementCTIDClientSecret);
            XmlElement elementBaiduApiKey = xmlDoc.CreateElement("BaiduApiKey");
            elementConfigs.AppendChild(elementBaiduApiKey);
            XmlElement elementBaiduSecretKey = xmlDoc.CreateElement("BaiduSecretKey");
            elementConfigs.AppendChild(elementBaiduSecretKey);

            //XmlElement elementIsOpenJSService = xmlDoc.CreateElement("IsOpenCPService");
            //elementConfigs.AppendChild(elementIsOpenJSService);

            XmlElement elementCPSBType = xmlDoc.CreateElement("CPSBType");
            elementConfigs.AppendChild(elementCPSBType);

            XmlElement elementIsOpenWxJSService = xmlDoc.CreateElement("IsOpenWxJSService");
            elementConfigs.AppendChild(elementIsOpenWxJSService);

            XmlElement elementJSUrl = xmlDoc.CreateElement("JSUrl");
            elementConfigs.AppendChild(elementJSUrl);

            XmlElement elementJSAccount = xmlDoc.CreateElement("JSAccount");
            elementConfigs.AppendChild(elementJSAccount);

            XmlElement elementJSPwd = xmlDoc.CreateElement("JSPwd");
            elementConfigs.AppendChild(elementJSPwd);

            XmlElement elementJSVersion = xmlDoc.CreateElement("JSVersion");
            elementConfigs.AppendChild(elementJSVersion);

            XmlElement elementJSPersonId = xmlDoc.CreateElement("JSPersonId");
            elementConfigs.AppendChild(elementJSPersonId);

            XmlElement elementCPSBSrvIP = xmlDoc.CreateElement("CPSBSrvIP");
            elementConfigs.AppendChild(elementCPSBSrvIP);

            XmlElement elementCPSBSrvPort = xmlDoc.CreateElement("CPSBSrvPort");
            elementConfigs.AppendChild(elementCPSBSrvPort);

            XmlElement elementTDZMonitorIP = xmlDoc.CreateElement("TDZMonitorIP");
            elementConfigs.AppendChild(elementTDZMonitorIP);

            XmlElement elementTDZMonitorPort = xmlDoc.CreateElement("TDZMonitorPort");
            elementConfigs.AppendChild(elementTDZMonitorPort);

            XmlElement elementTDZIsEnableIDCardMode = xmlDoc.CreateElement("TDZIsEnableIDCardMode");
            elementConfigs.AppendChild(elementTDZIsEnableIDCardMode);

            XmlElement elementTDZOpenDoorDuration = xmlDoc.CreateElement("TDZOpenDoorDuration");
            elementConfigs.AppendChild(elementTDZOpenDoorDuration);

            XmlElement elementNotify = xmlDoc.CreateElement("Notify");
            elementConfigs.AppendChild(elementNotify);

            XmlElement elementOrgId = xmlDoc.CreateElement("OrgId");
            elementConfigs.AppendChild(elementOrgId);

            XmlElement elementOpenWXSaaS = xmlDoc.CreateElement("OpenWXSaaS");
            elementConfigs.AppendChild(elementOpenWXSaaS);

            #region 第三方车牌识别接入访客系统
            XmlElement elementOPIP = xmlDoc.CreateElement("OPIP");
            elementConfigs.AppendChild(elementOPIP);

            XmlElement elementOPPort = xmlDoc.CreateElement("OPPort");
            elementConfigs.AppendChild(elementOPPort);

            XmlElement elementOPSecret = xmlDoc.CreateElement("OPSecret");
            elementConfigs.AppendChild(elementOPSecret);

            XmlElement elementOPIPServer = xmlDoc.CreateElement("OPIPServer");
            elementConfigs.AppendChild(elementOPIPServer);

            XmlElement elementOPPortServer = xmlDoc.CreateElement("OPPortServer");
            elementConfigs.AppendChild(elementOPPortServer);

            XmlElement elementTDZOpenSvrIP = xmlDoc.CreateElement("TDZOpenSvrIP");
            elementConfigs.AppendChild(elementTDZOpenSvrIP);

            XmlElement elementTDZOpenSvrPort = xmlDoc.CreateElement("TDZOpenSvrPort");
            elementConfigs.AppendChild(elementTDZOpenSvrPort);

            XmlElement elementIsEnableTDZOpenSvr = xmlDoc.CreateElement("IsEnableTDZOpenSvr");
            elementConfigs.AppendChild(elementIsEnableTDZOpenSvr);

            XmlElement elementTDZOpenSvrSecret = xmlDoc.CreateElement("TDZOpenSvrSecret");
            elementConfigs.AppendChild(elementTDZOpenSvrSecret);
            #endregion

            XmlElement elementPoliceType = xmlDoc.CreateElement("PoliceType");
            elementConfigs.AppendChild(elementPoliceType);


            XmlElement AreaTag = xmlDoc.CreateElement("AreaTag");
            elementConfigs.AppendChild(AreaTag);

            #region 运维平台配置信息
            XmlElement elementOpenGA = xmlDoc.CreateElement("OpenGA");
            elementConfigs.AppendChild(elementOpenGA);
            XmlElement elementGAAreaName = xmlDoc.CreateElement("GAAreaName");
            elementConfigs.AppendChild(elementGAAreaName);
            XmlElement elementGAUnitName = xmlDoc.CreateElement("GAUnitName");
            elementConfigs.AppendChild(elementGAUnitName);
            XmlElement elementGAUnitAddress = xmlDoc.CreateElement("GAUnitAddress");
            elementConfigs.AppendChild(elementGAUnitAddress);
            XmlElement elementGAUploadIP = xmlDoc.CreateElement("GAUploadIP");
            elementConfigs.AppendChild(elementGAUploadIP);
            XmlElement elementGAServiceNo = xmlDoc.CreateElement("GAServiceNo");
            elementConfigs.AppendChild(elementGAServiceNo);
            XmlElement elementGAOrgKey = xmlDoc.CreateElement("GAOrgKey");
            elementConfigs.AppendChild(elementGAOrgKey);
            XmlElement elementGAUploadName = xmlDoc.CreateElement("GAUploadName");
            elementConfigs.AppendChild(elementGAUploadName);
            XmlElement elementGAUploadPWD = xmlDoc.CreateElement("GAUploadPWD");
            elementConfigs.AppendChild(elementGAUploadPWD);

            XmlElement elementGAUploadVisitor_Src = xmlDoc.CreateElement("GAUploadVisitor_Src");
            elementConfigs.AppendChild(elementGAUploadVisitor_Src);
            XmlElement elementGARSAPublicKey = xmlDoc.CreateElement("GARSAPublicKey");
            elementConfigs.AppendChild(elementGARSAPublicKey);
            #endregion

            #region 数据接口服务
            XmlElement elementDataSrvIP = xmlDoc.CreateElement("DataSrvIP");
            elementConfigs.AppendChild(elementDataSrvIP);
            XmlElement elementDataSrvPort = xmlDoc.CreateElement("DataSrvPort");
            elementConfigs.AppendChild(elementDataSrvPort);
            XmlElement elementDataSrvAppId = xmlDoc.CreateElement("DataSrvAppId");
            elementConfigs.AppendChild(elementDataSrvAppId);

            #endregion
            xmlDoc.Save(Application.StartupPath + "\\Config.xml"); //保存以上字段

            addFieldElement("RegistrationField", "name", "required", "true");
            addFieldElement("RegistrationField", "sex", "required", "false");
            addFieldElement("RegistrationField", "certtype", "required", "false");
            addFieldElement("RegistrationField", "certnum", "required", "true");
            addFieldElement("RegistrationField", "tel", "required", "false");
            addFieldElement("RegistrationField", "company", "required", "false");
            addFieldElement("RegistrationField", "address", "required", "false");
            addFieldElement("RegistrationField", "belongs", "required", "false");
            addFieldElement("RegistrationField", "reason", "required", "false");
            addFieldElement("RegistrationField", "carmsg", "required", "false");

            addFieldElement("TelVoice", "content", "format", "访客易提醒您，尊敬的@被访人姓名，您有客人@访客姓名 来访");

            addParamElement("FastMode", "bool", "false");
            addParamElement("FastModeGraPhoto", "bool", "false");
            addParamElement("WGGrantDoors", "string", "");
            addParamElement("BookVoice", "bool", "false");
            addParamElement("VipVoice", "bool", "false");
            addParamElement("BlackVoice", "bool", "false");
            addParamElement("PrintType", "int", "0");
            addParamElement("IcPrint", "string", "0");

            //addParamElement("FKServiceUsername", "string", "tecsunPf");
            //addParamElement("FKServicePwd", "string", "tecsunPf");
            //addParamElement("FKServiceUrl", "string", "http://tecsun.tengenglish.cn");
            //addParamElement("OpenFKService", "bool", "false");

            addParamElement("UserName", "string", "");
            addParamElement("UserPWD", "string", "");
            addParamElement("IsCheck", "string", "0");

            addParamElement("LeaveType", "string", "0");
            addParamElement("Port", "int", "1001");
            addParamElement("ShowLeave", "string", "0");
            addParamElement("HideType", "string", "1");
            addParamElement("ShowLastVisit", "string", "1");
            addParamElement("MachineKind", "string", "ss");

            addParamElement("SerialPort", "int", "9");
            addParamElement("IpPort", "string", "59889");
            addParamElement("IP", "string", "127.0.0.1");
            addParamElement("ScanMachineKind", "string", "533U");
            addParamElement("PhoneKind", "string", "");

            addParamElement("PhonePort", "string", "COM4");
            addParamElement("CallSet", "string", "");
            addParamElement("GateSentry", "string", "");
            addParamElement("RedPort", "int", "2");
            addParamElement("onlyID", "string", "007");
            addParamElement("IsOpen", "string", "0");
            addParamElement("IsService", "string", "0");
            addParamElement("ServicePath", "string", "");
            addParamElement("Filed1", "string", "");
            addParamElement("Filed2", "string", "");
            addParamElement("IsShowLeave", "string", "0");
            addParamElement("AutoRunSS", "string", "0");
            addParamElement("IcCardDay", "string", "");
            addParamElement("Equipment", "string", "");
            addParamElement("ICCardReader", "string", "");
            addParamElement("DbType", "int", "-1");
            addParamElement("DbServername", "string", "");
            addParamElement("DbName", "string", "");
            addParamElement("DbUser", "string", "");
            addParamElement("DbPwd", "string", "");
            addParamElement("DbTypeSJP", "int", "-1");
            addParamElement("DbServernameSJP", "string", "");
            addParamElement("DbNameSJP", "string", "");
            addParamElement("DbUserSJP", "string", "");
            addParamElement("DbPwdSJP", "string", "");
            addParamElement("DbTypePf", "int", "-1");
            addParamElement("DbServernamePf", "string", "");
            addParamElement("DbNamePf", "string", "FK_Platform");
            addParamElement("DbUserPf", "string", "");
            addParamElement("DbPwdPf", "string", "");
            addParamElement("SysPwd", "string", "system");
            addParamElement("CRUserId", "string", "66915733240627193845");
            addParamElement("ScanTypes", "string", "身份证;驾驶证;护照");
            addParamElement("AutoDeleteTelRec", "bool", "True");
            addParamElement("DeleteTelRecDays", "int", "365");
            addParamElement("TelRecMaxMinutes", "int", "10");

            addParamElement("OpenPF", "bool", "false");
            addParamElement("PFUploadADRecord", "bool", "false");
            addParamElement("PFUrl", "string", "");
            addParamElement("PFUserName", "string", "");
            addParamElement("PFUserPwd", "string", "");

            addParamElement("WGGrantDoorsWeixin", "string", "");
            addParamElement("WeixinAccount", "string", "");
            addParamElement("WeixinDownloadEmpLastTime", "string", "最近一次更新时间:无");
            addParamElement("WeixinDownloadAuto", "bool", "false");
            addParamElement("WeixinDownloadTime", "string", "");

            addParamElement("SJPDownloadEmpLastTime", "string", "最近一次更新时间:无");
            addParamElement("SJPDownloadAuto", "bool", "false");
            addParamElement("SJPDownloadTime", "string", "");
            addParamElement("SJPCompanyName", "string", "一卡通");

            addParamElement("PFToken", "string", "tecsunPf");
            addParamElement("FKServiceUrl", "string", "http://tecsun.tengenglish.cn");
            addParamElement("OpenFKService", "bool", "false");

            addParamElement("SmsCheckInContent", "string", "访客易提醒您，@访客姓名 刷卡登记进入拜访您。");
            addParamElement("SmsLeaveContent", "string", "访客易提醒您，@访客姓名 已刷卡签离。");

            addParamElement("AccessControlType", "int", "0"); //0：没有门禁，1:微耕门禁，2：盛炬门禁

            addParamElement("AutoDeleteOverdueCard", "bool", "True");

            addParamElement("PfServerIp", "string", GetLocalIp());
            addParamElement("PfServerPort", "string", "8000");

            addParamElement("BaiduAK", "string", "Pke8GjtsNZUlkicXzG6GwhKS9eTuXurf");

            addParamElement("OpenFaceService", "bool", "true");

            addParamElement("FaceIP", "string", "127.0.0.0");
            addParamElement("FacePort", "string", "6800");
            addParamElement("FaceUser", "string", "admin");
            addParamElement("FacePwd", "string", "admin");
            addParamElement("FacePic", "string", "");

            addParamElement("FaceServerIP", "string", GetLocalIp());
            addParamElement("FaceServerPort", "string", "61000");
            addParamElement("FaceServerInterface", "string", "http://" + GetLocalIp() + ":8060/PlatformService");
            addParamElement("FaceServerType", "int", "0"); //0:没有启动人脸算法服务，1：单机版人脸1：N算法，2：服务版人脸1：N算法
            addParamElement("WeiXinServerType", "int", "0");

            addParamElement("CTIDUrl", "string", "");
            addParamElement("CTIDClientId", "string", "");
            addParamElement("CTIDClientSecret", "string", "");
            addParamElement("BaiduApiKey", "string", "");
            addParamElement("BaiduSecretKey", "string", "");

            //addParamElement("IsOpenCPService", "bool", "false");
            addParamElement("CPSBSrvIP", "string", GetLocalIp());
            addParamElement("CPSBSrvPort", "string", "51000");

            addParamElement("CPSBType", "int", "0");
            addParamElement("IsOpenWxJSService", "bool", "false");

            addParamElement("JSUrl", "string", "");
            addParamElement("JSAccount", "string", "");
            addParamElement("JSPwd", "string", "");

            addParamElement("JSVersion", "string", "1.0");
            addParamElement("JSPersonId", "string", "");

            addParamElement("TDZMonitorIP", "string", GetLocalIp());
            addParamElement("TDZMonitorPort", "string", "18887");
            addParamElement("TDZIsEnableIDCardMode", "bool", "false");
            addParamElement("TDZOpenDoorDuration", "string", "0000"); //
            addParamElement("Notify", "string", "0");

            addParamElement("OPIP", "string", GetLocalIp());
            addParamElement("OPPort", "string", "8098");
            addParamElement("OPSecret", "string", "tecsun");
            addParamElement("OPIPServer", "string", ""); //
            addParamElement("OPPortServer", "string", "");

            addParamElement("TDZOpenSvrIP", "string", GetLocalIp());
            addParamElement("TDZOpenSvrPort", "string", "18890");
            addParamElement("IsEnableTDZOpenSvr", "bool", "false");
            addParamElement("TDZOpenSvrSecret", "string", "tecsun");

            addParamElement("OrgId", "string", ""); //
            addParamElement("OpenWXSaaS", "string", "0");
            addParamElement("PoliceType", "int", "0");

            addParamElement("AreaTag", "string", "生态园");

            #region 数据接口服务

            addParamElement("DataSrvIP", "string", GetLocalIp()); //
            addParamElement("DataSrvPort", "string", "9098");
            addParamElement("DataSrvAppId", "string", "tecsun");

            #endregion

            #region 运维平台数据接口服务
            addParamElement("OpenGA", "bool", "false");
            addParamElement("GAAreaName", "string", "湖南");
            addParamElement("GAUnitName", "string", ""); //
            addParamElement("GAUnitAddress", "string", "");

            addParamElement("GAUploadIP", "string", "http://121.46.29.161:82"); //
            addParamElement("GAServiceNo", "string", "");
            addParamElement("GAOrgKey", "string", ""); //
            addParamElement("GAUploadName", "string", "");
            addParamElement("GAUploadPWD", "string", ""); //

            addParamElement("GAUploadVisitor_Src", "string", ""); //
            addParamElement("GARSAPublicKey", "string", ""); //
            
            #endregion
        }

        public static void CreateRootPoliceGZ(string xmlFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmldecl);
            XmlNode elementConfigs = xmlDoc.CreateElement("Configs");
            xmlDoc.AppendChild(elementConfigs);

            XmlNode elementUploadVisit = xmlDoc.CreateElement("UploadVisit");
            elementConfigs.AppendChild(elementUploadVisit);

            XmlNode elementPoliceUrl = xmlDoc.CreateElement("PoliceUrl");
            elementConfigs.AppendChild(elementPoliceUrl);

            XmlNode elementUnitName = xmlDoc.CreateElement("UnitName");
            elementConfigs.AppendChild(elementUnitName);

            XmlElement elementUnitAddress = xmlDoc.CreateElement("UnitAddress");
            elementConfigs.AppendChild(elementUnitAddress);

            XmlElement elementDealerCode = xmlDoc.CreateElement("DealerCode");
            elementConfigs.AppendChild(elementDealerCode);

            XmlElement elementDealerName = xmlDoc.CreateElement("DealerName");
            elementConfigs.AppendChild(elementDealerName);

            XmlElement elementKscode = xmlDoc.CreateElement("Kscode");
            elementConfigs.AppendChild(elementKscode);

            XmlElement elementKsArea = xmlDoc.CreateElement("KsArea");
            elementConfigs.AppendChild(elementKsArea);

            XmlElement elementLegalName = xmlDoc.CreateElement("LegalName");
            elementConfigs.AppendChild(elementLegalName);

            XmlElement elementContact = xmlDoc.CreateElement("Contact");
            elementConfigs.AppendChild(elementContact);

            XmlElement elementUinitID = xmlDoc.CreateElement("UinitID");
            elementConfigs.AppendChild(elementUinitID);
            

            xmlDoc.Save(xmlFile); //保存以上字段


            addParamElement("UploadVisit", "int", "1", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("PoliceUrl", "string", "http://gznbdc.gzjd.gov.cn", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("UnitName", "string", "", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("UnitAddress", "string", "", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("DealerCode", "string", "NB_GZDSZMXXKJYXGS_001", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("DealerName", "string", "广州德生智盟信息科技有限公司", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("Kscode", "string", "", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("KsArea", "string", "", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("LegalName", "string", "", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("Contact", "string", "", Application.StartupPath + "\\policeGZ.xml");
            addParamElement("UinitID", "string", "", Application.StartupPath + "\\policeGZ.xml");
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            string name = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] ipadrlist = System.Net.Dns.GetHostAddresses(name);
            foreach (System.Net.IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ipa.ToString();
            }
            return "";
        }

        private static void addFieldElement(string rootElement, string fieldName, string nodeName, string innertext, string xmlFile = "")
        {
            //装载Xml文件  
            XmlDocument xmlDoc = new XmlDocument();
            if (xmlFile != "")
            {
                xmlDoc.Load(xmlFile);
            }
            else
            {
                xmlDoc.Load(Application.StartupPath + "\\Config.xml");
            }


            //获取根节点  
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNode xmlElement = xmldocSelect.SelectSingleNode(rootElement);

            XmlElement elementFiled = xmlDoc.CreateElement("Field");
            elementFiled.SetAttribute("name", fieldName);

            XmlElement elementRequired = xmlDoc.CreateElement(nodeName);
            elementRequired.InnerText = innertext;
            elementFiled.AppendChild(elementRequired);

            xmlElement.AppendChild(elementFiled);
            if (xmlFile != "")
            {
                xmlDoc.Save(xmlFile);
            }
            else
            {
                xmlDoc.Save(Application.StartupPath + "\\Config.xml");
            }

        }

        private static void addParamElement(string rootElement, string datatype, string innertext, string xmlFile = "")
        {
            //装载Xml文件  
            XmlDocument xmlDoc = new XmlDocument();
            if (xmlFile != "")
            {
                xmlDoc.Load(xmlFile);
            }
            else
            {
                xmlDoc.Load(Application.StartupPath + "\\Config.xml");
            }

            //获取根节点  
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNode xmlElement = xmldocSelect.SelectSingleNode(rootElement);

            XmlElement elementFiled = xmlDoc.CreateElement("Data");
            elementFiled.SetAttribute("type", datatype);
            elementFiled.InnerText = innertext;

            xmlElement.AppendChild(elementFiled);
            if (xmlFile != "")
            {
                xmlDoc.Save(xmlFile);
            }
            else
            {
                xmlDoc.Save(Application.StartupPath + "\\Config.xml");
            }

        }

        /// <summary>
        /// 浏览文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void ExplorePath(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        private static void addFuctionFieldElement(string rootElement, string fieldName, string nodeName, string innertext)
        {
            //装载Xml文件  
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\FunctionConfig.xml");

            //获取根节点  
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNode xmlElement = xmldocSelect.SelectSingleNode(rootElement);

            XmlElement elementFiled = xmlDoc.CreateElement("Field");
            elementFiled.SetAttribute("name", fieldName);

            XmlElement elementRequired = xmlDoc.CreateElement(nodeName);
            elementRequired.InnerText = innertext;
            elementFiled.AppendChild(elementRequired);

            xmlElement.AppendChild(elementFiled);
            xmlDoc.Save(Application.StartupPath + "\\FunctionConfig.xml");
        }

        /// <summary>
        /// 初始化配置文件 
        /// </summary>
        public static void InitFunctionConfig()
        {
            if (!File.Exists(Application.StartupPath + "\\FunctionConfig.xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                CreateFunctionRootElement();
            }
        }

        public static void CreateFunctionRootElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmldecl);
            XmlNode elementConfigs = xmlDoc.CreateElement("Configs");
            xmlDoc.AppendChild(elementConfigs);

            XmlNode elementRegis = xmlDoc.CreateElement("Function");
            elementConfigs.AppendChild(elementRegis);

            xmlDoc.Save(Application.StartupPath + "\\FunctionConfig.xml"); //保存以上字段

            addFuctionFieldElement("Function", "ActiveLocal", "required", "false");
            addFuctionFieldElement("Function", "ActivePf", "required", "false");
            addFuctionFieldElement("Function", "ActinveSJP", "required", "false");

            addFuctionFieldElement("Function", "PfInterface", "required", "false");
            addFuctionFieldElement("Function", "AD", "required", "false");
            addFuctionFieldElement("Function", "WeiXin", "required", "false");
            addFuctionFieldElement("Function", "SMS", "required", "false");
            addFuctionFieldElement("Function", "PFUpload", "required", "false");
            addFuctionFieldElement("Function", "SJP", "required", "false");
            addFuctionFieldElement("Function", "FaceBarrier", "required", "false");
            addFuctionFieldElement("Function", "CPSB", "required", "false");
            addFuctionFieldElement("Function", "ADAPI", "required", "false");
            addFuctionFieldElement("Function", "PoliceUpload", "required", "false");
            addFuctionFieldElement("Function", "FKYDataService", "required", "false");
        }

        /// <summary>
        /// 修改功能字段配置
        /// </summary>
        public static void SetFunctionState(string function, string state)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\FunctionConfig.xml");

            //获取根节点  
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNodeList nodeList = xmldocSelect.SelectSingleNode("Function").ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                XmlElement fieldElement = (XmlElement)childNode;
                string fieldName = fieldElement.GetAttribute("name");
                if (fieldName == function)
                {
                    XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                    grandsonNodes[0].InnerText = state;
                }
            }

            xmlDoc.Save(Application.StartupPath + "\\FunctionConfig.xml");
        }

        /// <summary>
        /// 获取功能的启用状态
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public static string GetFunctionState(string function)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\FunctionConfig.xml");

            //获取根节点  
            XmlNode xmldocSelect = xmlDoc.SelectSingleNode("Configs");
            XmlNodeList nodeList = xmldocSelect.SelectSingleNode("Function").ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                XmlElement fieldElement = (XmlElement)childNode;
                string fieldName = fieldElement.GetAttribute("name");
                if (fieldName == function)
                {
                    XmlNodeList grandsonNodes = fieldElement.ChildNodes;
                    return grandsonNodes[0].InnerText;
                }
            }

            return "false";
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 是否本机有注册过试用期
        /// </summary>
        /// <returns></returns>
        public static bool IsRegeditExit()
        {
            string snFilePath = "C:\\TSLicense\\trial";
            if (File.Exists(snFilePath))
            {
                using (StreamReader stream = File.OpenText(snFilePath))
                {
                    string trial = stream.ReadToEnd();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        //试用密钥产生
        public static string getKeyNum(string cpyName, string pcCode, string days)
        {
            string UserNum = pcCode + cpyName + days;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(Encoding.ASCII.GetBytes(UserNum));
            string temp = BitConverter.ToString(bs).Replace("-", "").ToLower();
            StringBuilder sbTemp = new StringBuilder();
            string str1, str2, str3, str4;
            str1 = temp.Substring(2, 4).ToUpper();
            str2 = temp.Substring(6, 4).ToUpper();
            str3 = temp.Substring(11, 4).ToUpper();
            str4 = temp.Substring(8, 4).ToUpper();
            return str2 + "-" + str1 + "-" + str3 + "-" + str4;
        }

        //生成用户机器码
        public static string getUserNum()
        {
            //1、取出机器唯一码
            string input = GetCpuID() + GetDisk() + GetMacAddressByDos();//网卡可变不唯一
            //2、如果取唯一码出错，直接退出
            if (input.Length < 5)
            {
                MessageBox.Show("程序出错，即将退出");
                Process.GetCurrentProcess().Kill();
            }
            //3、取出机器唯一码的摘要并转换成字符串
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            string temp = BitConverter.ToString(bs).Replace("-", "").ToLower();

            string str1, str2, str3, str4;
            str1 = temp.Substring(0, 4).ToUpper();
            str2 = temp.Substring(4, 4).ToUpper();
            str3 = temp.Substring(8, 4).ToUpper();
            str4 = temp.Substring(12, 4).ToUpper();
            return str1 + "-" + str2 + "-" + str3 + "-" + str4;
        }

        //取CPU编号
        private static String GetCpuID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                return strCpuID;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        private static string GetDisk()
        {
            ManagementClass mc1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = mc1.GetInstances();
            string macihingId = null;
            foreach (ManagementObject mo in moc1)
            {
                macihingId = (string)mo.Properties["Model"].Value;
                break;
            }
            return macihingId;
        }

        /// <summary>  
        /// 通过DOS命令获得MAC地址  
        /// </summary>  
        /// <returns></returns>  
        public static string GetMacAddressByDos()
        {
            string macAddress = "";
            Process p = null;
            StreamReader reader = null;
            try
            {
                ProcessStartInfo start = new ProcessStartInfo("cmd.exe");

                start.FileName = "ipconfig";
                start.Arguments = "/all";

                start.CreateNoWindow = true;

                start.RedirectStandardOutput = true;

                start.RedirectStandardInput = true;

                start.UseShellExecute = false;

                p = Process.Start(start);

                reader = p.StandardOutput;

                string line = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    if (line.ToLower().IndexOf("physical address") > 0 || line.ToLower().IndexOf("物理地址") > 0)
                    {
                        int index = line.IndexOf(":");
                        index += 2;
                        macAddress = line.Substring(index);
                        macAddress = macAddress.Replace('-', ':');
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch
            {

            }
            finally
            {
                if (p != null)
                {
                    p.WaitForExit();
                    p.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return macAddress;
        }

        /// <summary>
        /// 获取注册软件的公司名
        /// </summary>
        /// <returns></returns>
        public static string GetRegistData()
        {
            string regPath = "C:\\TecsunMachine\\RegCom";
            if (File.Exists(regPath))
            {
                using (StreamReader stream = File.OpenText(regPath))
                {
                    string regCompany = stream.ReadToEnd();
                    return regCompany;
                }
            }
            else
            {
                return "";
            }
        }

        //生成注册激活码  
        public static string getActiveNum(string cpyName)
        {
            string UserNum = getUserNum() + cpyName;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(Encoding.ASCII.GetBytes(UserNum));
            string temp = BitConverter.ToString(bs).Replace("-", "").ToLower();
            StringBuilder sbTemp = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                int tIn = temp[i] % 10;
                string tCh = tIn.ToString();
                sbTemp.Append(tCh);
            }
            return sbTemp.ToString().Substring(3, 5);
        }

        /// <summary>
        /// 注册软件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tovalue"></param>
        public static void WTRegedit(string company)
        {
            string regPath = "C:\\TecsunMachine\\RegCom";
            string regPathDir = "C:\\TecsunMachine";
            if (File.Exists(regPath))
            {
                File.Delete(regPath);
            }

            if (!Directory.Exists(regPathDir))
            {
                Directory.CreateDirectory(regPathDir);
                File.SetAttributes(regPathDir, FileAttributes.ReadOnly);//设置成只读文件夹
                File.SetAttributes(regPathDir, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件夹
            }
            File.Create(regPath).Close();
            File.WriteAllText(regPath, company);

            File.SetAttributes(regPath, FileAttributes.ReadOnly);//设置成只读文件夹
            File.SetAttributes(regPath, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件
        }

        /// <summary>
        /// 增加使用天数
        /// </summary>
        /// <param name="day"></param>
        /// <param name="key"></param>
        public static void AddTestDays(int day, string key)
        {
            string snFilePath = "C:\\TSLicense\\trial";
            if (File.Exists(snFilePath))
            {
                StreamReader stream = File.OpenText(snFilePath);

                string trial = stream.ReadToEnd();
                string newTrial = trial + "_" + key;
                stream.Close();
                File.Delete(snFilePath);
                File.Create(snFilePath).Close();
                File.WriteAllText(snFilePath, newTrial); //保存使用过的增加试用天数码
                File.SetAttributes(snFilePath, FileAttributes.ReadOnly);//设置成只读文件夹
                File.SetAttributes(snFilePath, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件
            }

            string userTimeFilePath = "C:\\TecsunMachine\\userTime";
            string userTimeFileDir = "C:\\TecsunMachine";

            if (File.Exists(userTimeFilePath))
            {
                string newTrial;
                using (StreamReader stream = File.OpenText(userTimeFilePath))
                {
                    string trial = stream.ReadToEnd();
                    string time = desMethod.DecryptDES(trial, desMethod.strKeys);

                    if (DateTime.Now.Date < Convert.ToDateTime(time))
                    {
                        newTrial = desMethod.EncryptDES(Convert.ToDateTime(time).AddDays(day).ToString("yyyy-MM-dd"), desMethod.strKeys);
                    }
                    else
                    {
                        newTrial = desMethod.EncryptDES(DateTime.Now.Date.AddDays(day).ToString("yyyy-MM-dd"), desMethod.strKeys);
                    }
                }

                File.Delete(userTimeFilePath);
                File.WriteAllText(userTimeFilePath, newTrial);
                File.SetAttributes(userTimeFilePath, FileAttributes.ReadOnly);//设置成只读文件夹
                File.SetAttributes(userTimeFilePath, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件
            }
            else
            {
                if (!Directory.Exists(userTimeFileDir))
                {
                    Directory.CreateDirectory(userTimeFileDir);
                    File.SetAttributes(userTimeFileDir, FileAttributes.ReadOnly);//设置成只读文件夹
                    File.SetAttributes(userTimeFileDir, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件夹
                }
                File.Create(userTimeFilePath).Close();
                string newTrial = desMethod.EncryptDES(DateTime.Now.Date.AddDays(day).ToString("yyyy-MM-dd"), desMethod.strKeys);
                File.WriteAllText(userTimeFilePath, newTrial);
                File.SetAttributes(userTimeFilePath, FileAttributes.ReadOnly);//设置成只读文件夹
                File.SetAttributes(userTimeFilePath, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件
            }
            MessageBox.Show("增加成功");
        }

        #region 软件试用天数设置

        /// <summary>
        /// 获取试用剩下天数
        /// </summary>
        /// <returns></returns>
        public static int GetTestDays(bool overdueAndClose = true)
        {
            string userTimeFilePath = "C:\\TecsunMachine\\userTime";
            if (File.Exists(userTimeFilePath))
            {
                StreamReader stream = File.OpenText(userTimeFilePath);
                string trial = stream.ReadToEnd();
                string time = desMethod.DecryptDES(trial, desMethod.strKeys);
                stream.Close();

                if (DateTime.Now.Date < Convert.ToDateTime(time))
                {
                    TimeSpan ts = Convert.ToDateTime(time) - DateTime.Now.Date;
                    BLL.B_Base_Info.days = ts.Days;
                    return ts.Days;
                }
                else
                {
                    if (overdueAndClose)
                    {
                        MessageBox.Show("试用期完，请注册后使用");
                        Frm_Reg frm = new Frm_Reg(true);
                        frm.ShowDialog();
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
            else
            {
                if (overdueAndClose)
                {
                    MessageBox.Show("试用期完，请注册后使用");
                    Frm_Reg frm = new Frm_Reg(true);
                    frm.ShowDialog();
                    Process.GetCurrentProcess().Kill();
                }
            }

            return 0;
        }

        /// <summary>
        /// 软件试用天数设置
        /// </summary>
        /// <param name="days">试用天数</param>
        public static void AppTest(int days)
        {
            string userTimeFilePath = "C:\\TecsunMachine\\userTime";
            string userTimeFileDir = "C:\\TecsunMachine";
            string trialPath = "C:\\TSLicense\\trial";
            string trialPathDir = "C:\\TSLicense";

            if (File.Exists(userTimeFilePath))
            {
                using (StreamReader stream = File.OpenText(userTimeFilePath))
                {
                    string trial = stream.ReadToEnd();
                    string time = desMethod.DecryptDES(trial, desMethod.strKeys);

                    if (DateTime.Now.Date < Convert.ToDateTime(time))
                    {
                        TimeSpan ts = Convert.ToDateTime(time) - DateTime.Now.Date;
                        BLL.B_Base_Info.days = ts.Days;

                        if (ts.Days < 7)
                        {
                            string strShow = "请注意：软件的试用天数剩下" + ts.Days + "天！";
                            MessageBox.Show(strShow);
                        }
                    }
                    else
                    {
                        MessageBox.Show("试用期完，请注册后使用");
                        Frm_Reg frm = new Frm_Reg(true);
                        frm.ShowDialog();
                        Process.GetCurrentProcess().Kill();
                    }
                }
            }
            else
            {
                if (!Directory.Exists(userTimeFileDir))
                {
                    Directory.CreateDirectory(userTimeFileDir);
                    File.SetAttributes(userTimeFileDir, FileAttributes.ReadOnly);//设置成只读文件夹
                    File.SetAttributes(userTimeFileDir, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件夹
                }
                File.Create(userTimeFilePath).Close();

                string trial = desMethod.EncryptDES(DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"), desMethod.strKeys);

                File.WriteAllText(userTimeFilePath, trial);
                File.SetAttributes(userTimeFilePath, FileAttributes.ReadOnly);//设置成只读文件夹
                File.SetAttributes(userTimeFilePath, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件

                if (!Directory.Exists(trialPathDir))
                {
                    Directory.CreateDirectory(trialPathDir);
                    File.SetAttributes(trialPathDir, FileAttributes.ReadOnly);//设置成只读文件夹
                    File.SetAttributes(trialPathDir, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件夹
                }
                File.Create(trialPath).Close();
                File.SetAttributes(trialPath, FileAttributes.ReadOnly);//设置成只读文件夹
                File.SetAttributes(trialPath, FileAttributes.Hidden | FileAttributes.System);//设置添加系统和隐藏文件

                BLL.B_Base_Info.days = days;
                MessageBox.Show("感谢您第1次使访客易服务端软件,可试用" + days + "天。");
            }
        }

        /// <summary>
        /// 识别加试用天数的密钥是否已经使用过
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool isValidAddDaysKey(string key)
        {
            string trialPath = "C:\\TSLicense\\trial";
            string trialPathDir = "C:\\TSLicense";

            if (File.Exists(trialPath))
            {
                using (StreamReader stream = File.OpenText(trialPath))
                {
                    string trial = stream.ReadToEnd();
                    if (trial.Contains(key))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        #endregion

        #region 人脸闸机库函数

        //服务器变量
        public static bool isFaceServiceConnected = false;//是否已经连接比对服务器
        //数据库变量
        public static bool isFaceLogin = false;//是否已经登录照片库        
        public static UInt32 usToken = 0;//用户ID，登录后由动态库负责返回
        public static string curFaceDbName = string.Empty; //当前选中的照片库名
        public static List<FgiDbStatus> faceDbStatusList = new List<FgiDbStatus>();//人脸库库信息结构体列表

        public static UInt32 UsToken = 0;//用户ID，登录后由动态库负责返回

        //将数组装换为相应的结构体，用于将Intptr转换为结构体
        public static object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        #endregion

        #region ucs2转码
        // 将一个gbk编码的String转换为Ucs2编码的String
        public static string StrGbkToStrUcs2(string strGbk)
        {
            byte[] bytAry = Encoding.BigEndianUnicode.GetBytes(strGbk);

            string strHex = "";
            for (int i = 0; i < bytAry.Length; i++)
                strHex += bytAry[i].ToString("x").PadLeft(2, '0');
            return strHex;
        }

        // 将一个Ucs2编码的String转换为byte数组
        public static byte[] StrUcs2ToBytAry(string strUcs2)
        {
            int len = strUcs2.Length / 2;
            byte[] bytAry = new byte[len];
            for (int i = 0, j = 0; i < strUcs2.Length; i += 2, j++)
            {
                string strByt = strUcs2.Substring(i, 2);
                bytAry[j] = (byte)StrXToDec(strByt, 16);
            }
            return bytAry;
        }

        // 将一个Ucs2编码的String转换为Gbk编码的String
        public static string StrUcs2ToStrGbk(string strUcs2)
        {
            int len = strUcs2.Length / 2;
            byte[] bytAry = StrUcs2ToBytAry(strUcs2);
            return Encoding.BigEndianUnicode.GetString(bytAry, 0, len);
        }

        // 将其他进制的String转换到十进制数字
        public static int StrXToDec(string Num, int n)
        {
            char[] nums = Num.ToCharArray();
            int d = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                string number = nums[i].ToString();
                if (n == 16)
                {
                    switch (number.ToUpper())
                    {
                        case "A":
                            number = "10";
                            break;
                        case "B":
                            number = "11";
                            break;
                        case "C":
                            number = "12";
                            break;
                        case "D":
                            number = "13";
                            break;
                        case "E":
                            number = "14";
                            break;
                        case "F":
                            number = "15";
                            break;
                    }
                }
                Double power = Math.Pow(Convert.ToDouble(n),
                    Convert.ToDouble(nums.Length - (i + 1)));
                d = d + Convert.ToInt32(number) * Convert.ToInt32(power);
            }
            return d;
        }
        #endregion

        /// <summary>
        /// 对象转json文本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(object obj)
        {
            var json = string.Empty;
            StringBuilder sd = new StringBuilder();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Serialize(obj, sd);
            }
            catch
            {
                return "";
            }
            return sd.ToString();
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="unencrypted"></param>
        /// <returns></returns>
        public static string CardNoEncryptByDES(string unencrypted)
        {
            string code = desMethod.EncryptDES(unencrypted, desMethod.strKeys);
            byte[] arrayByte = Encoding.ASCII.GetBytes(code);
            return ByteArrayToHexStringNoSpace(arrayByte);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public static string CardNoDecryptByDES(string encrypted)
        {
            byte[] arrayByte = HexStringToByteArray(encrypted);
            string code = Encoding.ASCII.GetString(arrayByte);
            return desMethod.DecryptDES(code, desMethod.strKeys);
        }

        /// <summary>
        /// IP正则
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 提取匹配到的日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Match MacDate(string str)
        {
            return Regex.Match(str, @"((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            double lTime = double.Parse(timeStamp);
            DateTime targetDt = dtStart.AddMilliseconds(lTime);
            return targetDt;
        }

        public static string DateTimeToStamp(DateTime dateTime)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

    }
}

