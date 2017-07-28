using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using WPFClient.Common;
using WPFClient.Model;

namespace WPFClient.Util
{

    public class AskUtil
    {
        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(AskUtil));
        private static Teacher _teacher;

        public static Teacher Teacher
        {
            get
            {
                return _teacher;
            }

            set
            {
                _teacher = value;
            }
        }

        private static Login _login;

        public static Login Login
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
            }
        }


        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        public static string GetParams(object[] obj)
        {
            string str = string.Empty;
            str = string.Join("&", obj);//数组转成字符串

            return str;
        }

        public static void ShowAlert(string content, string header = "信息提示", int time =2000)
        {
            var alert = new RadDesktopAlert();
            alert.Header = header;
            alert.Content = content;
            alert.ShowDuration = time;
            //RadDesktopAlertManager manager = new RadDesktopAlertManager();
            RadDesktopAlertManager manager = new RadDesktopAlertManager(AlertScreenPosition.TopCenter, new System.Windows.Point(0, 300), 50);
            manager.ShowAlert(alert);
        }

        /// <summary>
        /// 获取科目标识  M
        /// </summary>
        /// <returns></returns>
        public static string GetSubject()
        {
            return _teacher.AskInfo.Code.ToCharArray()[1].ToString();
        }
        /// <summary>
        /// 年级标识  
        /// </summary>
        /// <returns></returns>
        public static string GetGrade()
        {
            return _teacher.AskInfo.Code.ToCharArray()[3].ToString() + _teacher.AskInfo.Code.ToCharArray()[4].ToString(); 
        }


        /// <summary>
        /// debug模式 IsDebug(this)
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public static bool IsDebug(UIElement u)
        {
            //TODO Debug模式设为false
            //bool isDebug = !(bool)DesignerProperties.IsInDesignModeProperty
            //            .GetMetadata(typeof(DependencyObject)).DefaultValue;
            return false;
        }

        public static BitmapImage GetImgSource(string path)
        {
            //@"/Resources/Image/main/close_sound.png"
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            return bi;
        }

        /// <summary>
        /// 显示键盘
        /// </summary>
        public static void ShowTip()
        {
            Process.Start("C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe");
        }

        public static void ShowLog()
        {
            string path = Directory.GetCurrentDirectory()+ "/logs/log.log";

            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }

        }

        public static ResponseModel JsonConvertRM(string json)
        {
            ResponseModel rm = new ResponseModel(1,"结果解析错误");
            try
            {
                rm =  JsonConvert.DeserializeObject<ResponseModel>(json);
            }
            catch (Exception e)
            {
                log.ErrorFormat("结果集:{0}",json);
                log.ErrorFormat("结果解析错误:{0}",e.Message);

            }
            return rm;
        }

        /// <summary>
        /// 是否安装了某软件
        /// </summary>
        /// <param name="softWareName">Screen Capturer Recorder</param>
        /// <returns></returns>
        public static bool CheckSoftWartInstallState(string softWareName)
        {
            Microsoft.Win32.RegistryKey uninstallNode =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.ReadKey);
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("DisplayName");
                if (displayName != null)
                {
                    if (displayName.ToString().Contains(softWareName))
                    {
                        return true;
                        // MessageBox.Show(displayName.ToString());    
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 执行软件
        /// </summary>
        /// <param name="fileName"></param>
        public static void execSoft(string fileName)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = "start /wait /verysilent sp-";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.FileName = fileName;
            Process.Start(psi);
        }

        /// <summary>
        /// 返回指示文件是否已被其它程序使用的布尔值
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名，例如：“C:\MyFile.txt”。</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static Boolean FileIsUsed(String fileFullName)
        {
            Boolean result = false;


            //判断文件是否存在，如果不存在，直接返回 false
            if (!System.IO.File.Exists(fileFullName))
            {
                result = false;


            }//end: 如果文件不存在的处理逻辑
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用


                //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);


                    result = false;
                }
                catch (System.IO.IOException ioEx)
                {
                    result = true;
                }
                catch (System.Exception ex)
                {
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }


            }//end: 如果文件存在的处理逻辑


            //返回指示文件是否已被其它程序使用的值
            return result;


        }//end method FileIsUsed
    }
}
