using System;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using NimUtility;
using NIM;
using WPFClient.Model;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight.Messaging;
using WPFClient.ViewModel;
using WPFClient.Common;
using WPFClient.BAL;
using WPFClient.Util;
using System.Threading;
using System.Diagnostics;

namespace WPFClient.Form
{
    public partial class MainWindow : Window
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainWindow));

        private AccountCollection _accounts = null;

        private DispatcherTimer _timer;

        /// <summary>
        /// 翻页委托类型
        /// </summary>
        delegate void NewQus();

        #region 初始化事件

        public MainWindow()
        {
            this.DataContext = MainViewModel.GetMainViewModelInstance();
            InitializeComponent();

            log.Info("主窗体");
        }

        void SendMessageResultHandler(object sender, MessageArcEventArgs args)
        {
            if (args.ArcInfo.Response == ResponseCode.kNIMResSuccess)
                return;

        }

        #endregion

        #region 界面按钮

        private void imgDemo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DemoWindow demo = new DemoWindow();
            demo.mWindow = this;
            demo.Show();
            // 隐藏自己(父窗体)
            this.Visibility = Visibility.Hidden;
        }
        #endregion

        #region 辅助方法

        private void imgExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //是否有文件在上传
            if (bgnUp)
            {
                AskUtil.ShowAlert("答疑视频正在上传，请稍后关闭！");
                return;
            }

            if (SMessageBox.Show("确认要退出程序？", "提示信息", SMessageBoxButton.YesNo,
                    SMessageBoxImage.None) == SMessageBoxResult.Yes)
            {
                this.Close();
            }

        }

        #endregion


        private void Window_Closed(object sender, EventArgs e)
        {
            log.Info("关闭窗体");

            if (NIM.ClientAPI.GetLoginState() == NIMLoginState.kNIMLoginStateLogin)
            {
                //在释放前需要按步骤清理音视频模块和nim client模块
                NIM.VChatAPI.Cleanup();
                System.Threading.Semaphore s = new System.Threading.Semaphore(0, 1);
                NIM.ClientAPI.Logout(NIM.NIMLogoutType.kNIMLogoutAppExit, (r) =>
                {
                    s.Release();
                });

                //需要logout执行完才能退出程序
                s.WaitOne(TimeSpan.FromSeconds(10));
                NIM.ClientAPI.Cleanup();

            }

            Application.Current.Shutdown();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["MainHeight"] = this.Height;

            //10s刷新
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(10000);
            _timer.Tick += new EventHandler(timersTimer_Elapsed);

            ToListView();
            RegisterMessengers();
            LoginNIM();
            NIM.TalkAPI.OnSendMessageCompleted += SendMessageResultHandler;

            if (!bool.Parse(ConfigUtil.GetSettingString("isPlayVideo")))
            {
                imgVideo.Source = AskUtil.GetImgSource(@"/Resources/Image/main/close_sound.png");
            }
        }

        private void timersTimer_Elapsed(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Messenger.Default.Send<bool>(true, "RefreshOrders");
            }));
        }

        private void ToListView()
        {
            ToPage("../View/SubjectOrdersView.xaml");

            BtnState(WinState.List);

        }

        bool bgnUp = false;
        private void RegisterMessengers()
        {

            Messenger.Default.Register<bool>(this, "ShowOrderListView", (r) =>
            {
                //当前页刷新提示  其他页刷新跳转到列表
                ToListView();
            });


            Messenger.Default.Register<SubjectOrder>(this, "ShowOrderDetail", (order) =>
            {
                ToPage("../View/SubjectOrderView.xaml");

                Application.Current.Properties["OrderDetail"] = order;
                BtnState(WinState.Detail);
            });

            Messenger.Default.Register<SubjectOrder>(this, "ShowTeachView", (order) =>
            {
                Application.Current.Properties["OrderDetail"] = order;
                //接单
                CommitOrder(order);
            });

            Messenger.Default.Register<bool>(this, "ShowInfoWindow", (msg) =>
            {
                new InfoWindow().ShowDialog();
            });

            Messenger.Default.Register<QiniuF>(this, "UploadQiniu", (qf) =>
            {
                updateQiniu(qf);

            });

            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        private void CommitOrder(SubjectOrder order)
        {

            if(!AskUtil.IsInternetAvailable())
            {
                AskUtil.ShowAlert("连接异常，请检查网络连接");
                return;
            }

            if (NIM.ClientAPI.GetLoginState() != NIMLoginState.kNIMLoginStateLogin)
            {
                LoginNIM();
            }

            if (NIM.ClientAPI.GetLoginState() == NIMLoginState.kNIMLoginStateLogin)
            {
                ResponseModel rm = SubjectOrderAction.CommitOrder(order.Id, AskUtil.Login.UserName, AskUtil.Teacher.Info.FamilyName + AskUtil.Teacher.Info.Name, AskUtil.Teacher.AskInfo.Code);

                //接单成功
                if (rm.Flag == 0)
                {
                    TeachWindow nt = new TeachWindow();
                    nt.mWindow = this;

                    Messenger.Default.Send<SubjectOrder>(order, "TeachOrder");
                    nt.Show();

                    _timer.Stop();
                    //this.Visibility = Visibility.Hidden;
                }
                else
                {
                    AskUtil.ShowAlert(rm.Msg);
                }

            }else
            {
                AskUtil.ShowAlert("IM登录失败，请退出重新登录"); 
            }
            
        }

        public void LoginNIM()
        {
            log.Info("login nim");
            NIMToken token = LoginAction.GetNIMToken(AskUtil.Login.UserName);

            if(token == null)
            {
                AskUtil.ShowAlert("IM登录授权有误！");
                log.ErrorFormat("IM登录授权有误！");
                return;
            }

            if (NIMHelper.InitNIM())
            {
                string login = NIMHelper.LoginNIM(AskUtil.Login.UserName, token.Token, HandleLoginResult);

                if (!string.IsNullOrEmpty(login))
                {
                    AskUtil.ShowAlert(login);
                }
            }
        }

        void HandleLogoutResult(NIM.NIMLogoutResult result)
        {
            //quitA();
            SMessageBox.Show("学生端未连接成功，请重新接单");
        }

        private void HandleLoginResult(NIM.NIMLoginResult result)
        {
            Action action = () =>
            {
                if (result.LoginStep == NIM.NIMLoginStep.kNIMLoginStepLogin)
                {
                    if (result.Code == NIM.ResponseCode.kNIMResSuccess)
                    {
                        //初始化语音
                        if (!NIM.VChatAPI.Init())
                        {
                            AskUtil.ShowAlert("NIM VChatAPI init failed!");
                            log.Error("NIM VChatAPI init failed!");
                            return;
                        }

                        System.Threading.ThreadPool.QueueUserWorkItem((s) =>
                        {
                            SaveLoginAccount();
                        });
                    }
                    else
                    {
                        NIM.ClientAPI.Logout(NIM.NIMLogoutType.kNIMLogoutChangeAccout, HandleLogoutResult);
                    }
                }
            };
            this.Dispatcher.Invoke(action);
        }


        private void SaveLoginAccount()
        {
            if (_accounts == null)
                _accounts = new AccountCollection();
            var index = _accounts.IndexOf(AskUtil.Login.UserName);
            if (index != -1)
            {
                _accounts.List[index].Password = AskUtil.Login.Password;
                _accounts.LastIndex = index;
            }
            else
            {
                Account account = new Account();
                account.Name = AskUtil.Login.UserName;
                account.Password = AskUtil.Login.Password;
                _accounts.List.Insert(0, account);
                _accounts.LastIndex = 0;
            }

            AccountManager.SaveLoginAccounts(_accounts);
        }

        

        /// <summary>
        /// 退出回答
        /// </summary>
        private void QuitAnswer()
        {

        }

        private void ToPage(string page)
        {
            this.PageContext.Source = new Uri(page, UriKind.Relative);

        }

        private void btnBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToListView();
        }

        private void BtnState(WinState state)
        {
            if (WinState.List == state)
            {
                btnRefresh.Visibility = Visibility.Visible;
                btnOK.Visibility = Visibility.Collapsed;
                btnBack.Visibility = Visibility.Collapsed;
                if (!_timer.IsEnabled)
                    _timer.Start();
            }
            else if (WinState.Detail == state)
            {
                btnRefresh.Visibility = Visibility.Collapsed;
                btnOK.Visibility = Visibility.Visible;
                btnBack.Visibility = Visibility.Visible;
                if(_timer.IsEnabled)
                    _timer.Stop();
            }

        }

        enum WinState
        {
            List = 1,
            Detail = 2,
        }

        private void btnOK_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //接单
            CommitOrder((SubjectOrder)Application.Current.Properties["OrderDetail"]);
        }

        private void imgVideo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isVideo = bool.Parse(ConfigUtil.GetSettingString("isPlayVideo"));

            if (isVideo)
            {
                imgVideo.Source = AskUtil.GetImgSource(@"/Resources/Image/main/close_sound.png"); 
            }
            else
            {
                imgVideo.Source = AskUtil.GetImgSource(@"/Resources/Image/main/open_sound.png");
            }

            ConfigUtil.UpdateSettingString("isPlayVideo",(!isVideo).ToString());
        }

        /// <summary>
        /// 上传qiniu
        /// </summary>
        /// <param name="qf"></param>
        private void updateQiniu(QiniuF qf)
        {
            //
            int upState = 0;
            string filePath = "";
            Thread t = new Thread(() => {
                //this.Dispatcher.BeginInvoke(new Action(() =>
                //{
                    log.Info("UploadQiniu");
                    try
                    {
                        if (qf != null)
                        {
                            AskUtil.ShowAlert("正在生成视频，请稍等。。。");
                            log.Info("等待生成视频：" + new DateTime());
                            System.Threading.Thread.Sleep(10000);

                            //await System.Threading.Tasks.Task.Delay(5000);
                            log.Info("生成视频：" + new DateTime());
                            ScreenHelper.killProcess("ffmpeg");

                            try
                            {
                                Process p = Process.GetProcessById(qf.Pid);
                                if (p != null)
                                    p.Kill();
                            }
                            catch { }

                            int c = 0;
                            //判断文件是否被占用
                            while (AskUtil.FileIsUsed(qf.LocalFile))
                            {
                                log.Info("文件被占用，等待。。。");
                                System.Threading.Thread.Sleep(5000);
                                if (!AskUtil.FileIsUsed(qf.LocalFile) || c==3)
                                {
                                    log.Info("文件解除占用");
                                    break;
                                }
                                c++;
                            }

                            bgnUp = true;
                            upState = QiniuHelper.updateQiniu(qf.LocalFile, qf.QiniuFile);
                            filePath = string.Format("{0}", HttpHelper.QINIU_CDN_URL + qf.QiniuFile);
                            //string[] qnurl = new string[HttpHelper.QINIU_CDN_URL+qf.QiniuFile];
                            //保存上传视频地址
                            SubjectOrderAction.SaveVideoPath(qf.OrderId, filePath);
                            log.Info("保存视频地址成功1：" + qf.QiniuFile);
                            AskUtil.ShowAlert("视频生成完成");
                            bgnUp = false;

                    }
                    }
                    catch (Exception e)
                    {
                        bgnUp = false;
                        log.Info("UploadQiniu_fail:" + e.Message);
                    }
                    log.Info("UploadQiniu_end:" + upState);
                    try { 
                        //如果上传失败 保存失败记录 下次上传 
                        if (upState == 0)
                        {
                            System.Threading.Thread.Sleep(3000);
                            ScreenHelper.killProcess("ffmpeg");
                            ScreenHelper.killProcess("cmder");
                            upState = QiniuHelper.updateQiniu(qf.LocalFile, qf.QiniuFile);

                            //保存上传视频地址
                            SubjectOrderAction.SaveVideoPath(qf.OrderId, filePath);
                            log.Info("保存视频地址成功2：" + qf.QiniuFile);
                    }
                    }
                    catch
                    {

                    }
                //}));
            });

            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();

        }
    }
}
