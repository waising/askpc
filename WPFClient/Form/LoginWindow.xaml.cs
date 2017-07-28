using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WPFClient.Util;
using WPFClient.ViewModel;

namespace WPFClient.Form
{
    public partial class Login : Window
    {

        log4net.ILog log = log4net.LogManager.GetLogger(typeof(Login));


        #region 初始化
        public Login()
        {
            this.DataContext = LoginViewModel.getLoginViewModelInstance();
            InitializeComponent();

            //版本号
            //this.lblVersion.Content = "当前版本："+System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                this.lblVersion.Content = "当前版本：" + ApplicationDeployment.CurrentDeployment.CurrentVersion;
                log.Info("<<<=======版本号=====" + this.lblVersion.Content);
            }

            lblTel.Content = "热线电话：400-918-1915";

            if (AskUtil.IsDebug(this))
            {
                this.txtUser.Text = "12345678902";
                this.txtPsw.Password = "123";
            }

            Messenger.Default.Register<bool>(this, "ShowMainForm", (msg) =>
            {
                new MainWindow().Show();
                Close();
            });

            Messenger.Default.Register<bool>(this, "CloseLoginForm", (msg) =>
            {
                Application.Current.Shutdown();
            });

            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        #endregion


        #region 设置类事件

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 键盘自动弹出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            AskUtil.ShowTip();
        }

        /// <summary>
        /// 键盘自动弹出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPsw_GotFocus(object sender, RoutedEventArgs e)
        {
            AskUtil.ShowTip();     
        }

        /// <summary>
        /// 标题栏拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }
        #endregion

        private void lblTel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AskUtil.ShowLog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPsw.Password))
                {
                    ((LoginViewModel)this.DataContext).Login();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //如果程序没有安装screen 自动安装
            if (!AskUtil.CheckSoftWartInstallState("Screen Capturer Recorder"))
            {
                //AskUtil.ShowAlert("本机没有安装一对一视频录制插件，请安装");
                AskUtil.execSoft("Setup Screen Capturer Recorder.exe");
            }
        }
    }
}
