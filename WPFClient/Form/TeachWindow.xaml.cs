using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows;
using System.Media;
using System.Windows.Media.Imaging;
using System.Windows.Ink;
using System.Windows.Threading;
using System.Collections.Generic;
using WPFClient.Util;
using WPFClient.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using WPFClient.Model;
using NIM;
using WPFClient.Common;
using System.Windows.Input;
using WPFClient.BAL;
using System.Windows.Media;
using System.Diagnostics;

namespace WPFClient.Form
{
    /// <summary>
    /// NIMTeachWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TeachWindow : Window
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(TeachWindow));

        private string _sessionId;
        private DispatcherTimer _timer;
        private DispatcherTimer _sendDataTimer;
        SoundPlayer _player;
        //审题时间
        private int _seconds = 12;

        private string _orderId;

        public MainWindow mWindow;

        private bool _drawing = false;

        //状态
        private CallState _CallState = CallState.Analyze;

        private readonly System.Drawing.Pen _myPen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);
        private readonly System.Drawing.Pen _peerPen = new System.Drawing.Pen(System.Drawing.Color.Brown, 3);
        private readonly PaintingRecord _selfPaintingRecord = new PaintingRecord();
        private readonly PaintingRecord _peerPaintingRecord = new PaintingRecord();
        //StrokeCollection _stroke = new StrokeCollection();
        //记录笔画
        LinkedList<Stroke> _strokes = null;

        //教师结算
        bool _isTCount = false;

        public delegate void CloseWindowDelegate();
        CloseWindowDelegate closeD;

        //draw 坐标
        Point _drawPoint;
        double _drawAcWidth;
        double _drawAcHeight;

        public TeachWindow()
        {
            this.DataContext = TeacherViewModel.GetTeachViewModelInstance();
            InitializeComponent();
            Topmost = true;

            _player = new SoundPlayer();
            _player.SoundLocation = "Resources/Wav/CallingRing.wav";
            _player.Load();
            log.Info("教学窗体");
        }

        private void OpenDevice()
        {
            //开启音频
            NIM.DeviceAPI.StartDevice(NIM.NIMDeviceType.kNIMDeviceTypeAudioIn, "", 0, null);

            string open = "开启音频";
            log.Info(open);
            string close = "关闭音频";
            NIM.RtsAPI.Control(_sessionId, open, (int code, string sessionId, string info) =>
            {
                if (code == 200)
                {
                    Func<string, int> func = (ret) =>
                    {
                        //已开启
                        if (info == open)
                        {
                            //消除回音 降噪 人言检测
                            NIM.DeviceAPI.SetAudioProcessInfo(true, true, true);
                        }
                        else if (info == close)
                        {
                            //yi关闭
                        }
                        return 0;
                    };
                }
            });
        }

        string strText = null;
        private void timersTimer_Elapsed(object sender, EventArgs e)
        {
            if (_seconds < 0 && CallState.Analyze == _CallState)
            {
                //审题时间到
                //_timer.Stop();
                //开始呼叫
                ((TeacherViewModel)this.DataContext).StartConn();
                return;
            }
            else if (_seconds > 30 && CallState.Calling == _CallState)
            {
                _timer.Stop();
                //呼叫超时
                CallMsg(CallState.CallTimeOut);
            }

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_CallState == CallState.Analyze)
                    _seconds--;
                else
                    _seconds++;

                if (_seconds < 3600)
                    strText = string.Format("{0:D2}:{1:D2}", (int)(_seconds / 60), _seconds % 60);
                else
                    strText = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)(_seconds / 3600), (int)(_seconds / 60), _seconds % 60);

                if (_seconds >= 0)
                    TeachTime.Text = strText;

            }));

        }


        private void sendDataTimer_Elapsed(object sender, EventArgs e)
        {
            SendData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //记录时间
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(timersTimer_Elapsed);

            _timer.Start();

            _sendDataTimer = new DispatcherTimer();
            _sendDataTimer.Interval = TimeSpan.FromMilliseconds(100);
            _sendDataTimer.Tick += new EventHandler(sendDataTimer_Elapsed);

            ////默认设置黑色
            //DrawCanvas.DefaultDrawingAttributes.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Black");

            double dh = DrawCanvas.ActualHeight;
            DrawCanvas.Height = dh + this.ActualHeight;

            SetPenStyle(getPenARGB(), getPenSize());

            _selfPaintingRecord.height = DrawCanvas.Height;
            _selfPaintingRecord.width = DrawCanvas.ActualWidth;

            _strokes = new LinkedList<Stroke>();
            closeD = new CloseWindowDelegate(this.CloseWindow);

            _CallState = CallState.Analyze;

            ///注册事件
            RegisterMessengers();
            
            DrawCanvas.AddHandler(InkCanvas.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DrawCanvas_MouseDown), true);
            SameSubButton.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(SameSubButton_MouseLeftButtonDown), true);

            ((TeacherViewModel)this.DataContext).IsStart = false;


            Window window = Window.GetWindow(DrawCanvas);
            _drawPoint = DrawCanvas.TransformToAncestor(window).Transform(new Point(0, 0));
            _drawAcWidth = DrawCanvas.ActualWidth;
            _drawAcHeight = DrawCanvas.ActualHeight;
        }

        private void RegisterMessengers()
        {
            //取消教学
            Messenger.Default.Register<ResponseModel>(this, "CancelCommand", (rm) =>
            {
                CancelTeach(rm);
            });
            //结算
            Messenger.Default.Register<ResponseModel>(this, "FinishCommand", (rm) => {
                FinishTeach(rm);
            });

            //
            Messenger.Default.Register<SubjectOrder>(this, "StartConn", (order) =>
            {
                StartTeach(order,false);
            });

            //开始教学
            Messenger.Default.Register<SubjectOrder>(this, "StartTeach", (order) =>
            {
                StartTeach(order,true);
            });

            Messenger.Default.Register<AskPen>(this, "PenStyle", pen =>
            {
                DrawPenStyle(pen);
            });

            Messenger.Default.Register<bool>(this, "TeachFail", tf =>
            {
                TeachState.Text = "订单已被取消，请取消答疑！";
                log.Info(TeachState.Text);
                //关闭计时器
                _timer.Stop();

            });

            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }

        private void DrawPenStyle(AskPen pen)
        {
            //橡皮擦
            if (pen.Style == PenStyleEnum.Eraser)
            {
                DrawCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                DrawCanvas.DefaultDrawingAttributes.IgnorePressure = true;
                DrawCanvas.EraserShape = new EllipseStylusShape(50, 50);

                DrawCanvas.DefaultDrawingAttributes.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(DrawCanvas.Background.ToString());
                DrawCanvas.DefaultDrawingAttributes.Width = 50;
                DrawCanvas.DefaultDrawingAttributes.Height = 50;
            }
            else if (pen.Style == PenStyleEnum.Clear)
            {
                //清除
                ClearPanel();
                //SetPenStyle(pen);
            }
            else if (pen.Style == PenStyleEnum.Back)
            {
                BackLine();
                //SetPenStyle(pen);
            }
            else
            {
                SetPenStyle(pen);
            }
            log.InfoFormat("{0}", pen.Style);

            SetPenStyle(getPenARGB(), getPenSize());
        }

        private void SetPenStyle(AskPen pen)
        {
            DrawCanvas.DefaultDrawingAttributes.Width = pen.Size;
            DrawCanvas.DefaultDrawingAttributes.Height = pen.Size;
            DrawCanvas.DefaultDrawingAttributes.IgnorePressure = true;
            //DrawCanvas.DefaultDrawingAttributes.IsHighlighter = true;
            DrawCanvas.EditingMode = InkCanvasEditingMode.Ink;

            DrawCanvas.DefaultDrawingAttributes.Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(pen.Color);
        }

        public void SetRtsNotifyCallback()
        {
            NIM.RtsAPI.SetHungupNotify((string sessionId, string uid) =>
            {
                if (_sendDataTimer != null)
                    _sendDataTimer.Stop();
                if (_timer != null)
                    _timer.Stop();
                //SMessageBox.Show(uid + " 挂断");
                if (SMessageBox.Show("本次辅导已结束", "结束教学", SMessageBoxButton.YesNo) == SMessageBoxResult.No) return;
                log.Info("学生取消学习");
                this.Dispatcher.Invoke(closeD);
            });

            NIM.RtsAPI.SetConnectionNotifyCallback((string sessionId, int channelType, int code) =>
            {

            });

            NIM.RtsAPI.SetControlNotifyCallback((string sessionId, string info, string uid) =>
            {
                if (sessionId != _sessionId) return;
            });
        }

        private string getPenSize()
        {
            long rech = (((int)DrawCanvas.DefaultDrawingAttributes.Width << 8) & 0xff00) |
                (((int)DrawCanvas.DefaultDrawingAttributes.Height << 0) & 0xff);

            return Convert.ToString(rech);
        }

        private string getPenARGB()
        {
            long rgbInt = ((DrawCanvas.DefaultDrawingAttributes.Color.R << 16) & 0xff0000) |
                            ((DrawCanvas.DefaultDrawingAttributes.Color.G << 8) & 0xff00) |
                                ((DrawCanvas.DefaultDrawingAttributes.Color.B << 0) & 0xff);

            return Convert.ToString(rgbInt);
        }


        /// <summary>
        /// 设置笔画样式
        /// </summary>
        /// <param name="rgb"></param>
        /// <param name="lineSize"></param>
        private void SetPenStyle(string rgb, string lineSize)
        {
            _selfPaintingRecord.rgb = rgb;
            _selfPaintingRecord.lineSize = lineSize;
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPanel()
        {
            DrawCanvas.Strokes.Clear();
            if (_strokes != null)
                _strokes.Clear();
            SendControlCommand(CommandType.DrawOpClear);
        }


        NIM.NIMTextMessage _textMsg;
        private void sentMessage(string text)
        {
            _textMsg = new NIM.NIMTextMessage();
            _textMsg.SessionType = NIM.Session.NIMSessionType.kNIMSessionTypeP2P;
            _textMsg.ReceiverID = _sessionId;
            _textMsg.TextContent = text;
            _textMsg.PushContent = "";
            NIM.TalkAPI.SendMessage(_textMsg);
        }

        #region 笔画图
        private void DrawCanvas_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            if (!DrawCanvas.AreAnyTouchesOver)
            {
                //橡皮擦
                if (DrawCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint)
                    DrawCanvas.EditingMode = InkCanvasEditingMode.Ink;

                _drawing = true;
                _selfPaintingRecord.Add(CommandType.DrawOpStart, (float)e.StylusDevice.GetPosition(DrawCanvas).X, (float)e.StylusDevice.GetPosition(DrawCanvas).Y);

            }
        }

        private void DrawCanvas_StylusUp(object sender, System.Windows.Input.StylusEventArgs e)
        {
            if (DrawCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
            {
                _drawing = false;
                _selfPaintingRecord.Add(CommandType.DrawOpEnd, (float)e.StylusDevice.GetPosition(DrawCanvas).X, (float)e.StylusDevice.GetPosition(DrawCanvas).Y);
                SendData();

                //DrawCanvas.Strokes.Add(_stroke);
            }

        }

        private void DrawCanvas_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            if (DrawCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint)
                DrawCanvas.EditingMode = InkCanvasEditingMode.None;

            _drawing = false;
        }


        private void DrawCanvas_StylusMove(object sender, System.Windows.Input.StylusEventArgs e)
        {
            if (DrawCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint || _drawing)
            {
                _selfPaintingRecord.Add(CommandType.DrawOpMove, (float)e.StylusDevice.GetPosition(DrawCanvas).X, (float)e.StylusDevice.GetPosition(DrawCanvas).Y);
                //var count = _selfPaintingRecord.Count;
                //if (count >= 2)
                //{
                //Draw(_selfPaintingRecord, count - 2, count - 1, _myDrawingGraphics, _myPen);
                //}
            }
        }

        #endregion

        private void DrawCanvas_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {

        }

        private void DrawCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            if (_strokes != null)
                _strokes.AddLast(e.Stroke);
        }


        #region  鼠标画图

        private void DrawCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AskUtil.IsDebug(this))
            {
                if (!DrawCanvas.AreAnyTouchesOver)
                {
                    //橡皮擦
                    if (DrawCanvas.EditingMode != InkCanvasEditingMode.EraseByPoint)
                        DrawCanvas.EditingMode = InkCanvasEditingMode.Ink;

                    _drawing = true;
                    _selfPaintingRecord.Add(CommandType.DrawOpStart, (float)e.MouseDevice.GetPosition(DrawCanvas).X, (float)e.MouseDevice.GetPosition(DrawCanvas).Y);
                    SendData();
                }
            }
        }

        private void DrawCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AskUtil.IsDebug(this))
            {
                if (DrawCanvas.EditingMode == InkCanvasEditingMode.Ink)
                {
                    _drawing = false;
                    _selfPaintingRecord.Add(CommandType.DrawOpEnd, (float)e.MouseDevice.GetPosition(DrawCanvas).X, (float)e.MouseDevice.GetPosition(DrawCanvas).Y);
                    SendData();

                    //DrawCanvas.Strokes.Add(_stroke);
                    //DrawCanvas.EditingMode = InkCanvasEditingMode.None;
                }
            }
        }

        private void DrawCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AskUtil.IsDebug(this))
            {
                if (DrawCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint || _drawing)
                {
                    _selfPaintingRecord.Add(CommandType.DrawOpMove, (float)e.MouseDevice.GetPosition(DrawCanvas).X, (float)e.MouseDevice.GetPosition(DrawCanvas).Y);
                    //var count = _selfPaintingRecord.Count;
                    //if (count >= 2)
                    //{
                    //Draw(_selfPaintingRecord, count - 2, count - 1, _myDrawingGraphics, _myPen);
                    //}
                }
            }
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                TeacherCount();

                if (mWindow != null)
                {
                    //mWindow.Visibility = System.Windows.Visibility.Visible;
                    Messenger.Default.Send<bool>(true, "RefreshOrders");
                    Messenger.Default.Send<bool>(true, "ShowOrderListView");
                }

            }
            finally
            {

            }
        }

        void SendControlCommand(CommandType type)
        {
            string cmdStr = string.Format("{0}:0,0;", (int)type);
            Console.WriteLine(cmdStr);
            SendRtsData(cmdStr);
        }

        void SendSizeCommand(double w, double h)
        {
            string cmdStr = string.Format("{0}:{1},{2};", (int)CommandType.DrawOpDrawPanelSize, w, h);
            Console.WriteLine(cmdStr);
            SendRtsData(cmdStr);
        }

        void SendRtsData(string cmdStr)
        {
            var ptr = Marshal.StringToHGlobalAnsi(cmdStr);
            NIM.RtsAPI.SendData(_sessionId, NIM.NIMRts.NIMRtsChannelType.kNIMRtsChannelTypeTcp, ptr, cmdStr.Length);
        }

        /// <summary>
        /// 发送滚动命令
        /// </summary>
        /// <param name="type"></param>
        /// <param name="t">1：竖 0：横</param>
        /// <param name="h"></param>
        void SendScrollCommand(CommandType type, double x, double y)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                string cmdStr = string.Format("{0}:{1},{2};", (int)type, x, y);
                SendRtsData(cmdStr);
            }));
        }

        private void SendData()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                var cmdStr = _selfPaintingRecord.CreateCommand();
                if (string.IsNullOrEmpty(cmdStr))
                    return;

                //Console.WriteLine(cmdStr);
                var ptr = Marshal.StringToHGlobalAnsi(cmdStr);
                NIM.RtsAPI.SendData(_sessionId, NIM.NIMRts.NIMRtsChannelType.kNIMRtsChannelTypeTcp, ptr, cmdStr.Length);

            }));

        }


        /// <summary>
        /// 回退一步
        /// </summary>
        private void BackLine()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (_strokes.Count > 0)
                {
                    SendControlCommand(CommandType.DrawOpUndo);
                    DrawCanvas.Strokes.Remove(_strokes.Last.Value);
                    _strokes.RemoveLast();
                    _selfPaintingRecord.Revert();
                }
            }));

        }

        private void TeacherCount()
        {
            try
            {
                if (!_isTCount)
                {

                    if (_sendDataTimer != null)
                        _sendDataTimer.Stop();
                    if (_timer != null)
                        _timer.Stop();

                    //清理
                    ///NIM.VChatAPI.Cleanup();
                    //NIM.ClientAPI.Cleanup();

                }
                stopScreenUp();
            }
            finally
            {
                CloseRts();
            }

        }

        private void CloseRts()
        {
            NIM.DeviceAPI.EndDevice(NIM.NIMDeviceType.kNIMDeviceTypeAudioIn);
            NIM.DeviceAPI.EndDevice(NIM.NIMDeviceType.kNIMDeviceTypeAudioOutChat);
            NIM.RtsAPI.Hangup(_sessionId, (a, b) =>
            {
                log.InfoFormat("nimrts hangup:{0},{1},sessionId:{2}",a,b, _sessionId);
            });

        }

        private bool _isRtsTeach;

        //教学开始
        private void StartTeach(SubjectOrder order,bool isTeach)
        {

            if (isTeach && CallState.Analyze == _CallState)
            {
                _isRtsTeach = true;
            }
            else
            {
                _isRtsTeach = false;
            }


            //审题或者已经过了审题时间
            if ((isTeach && CallState.Analyze == _CallState) || !isTeach)
            {
                PlayWav();
                //order.Student.Account

                _orderId = order.Id;

                OpenRts(order.Student.Account);

            }else if (isTeach)
            {
                CallMsg(CallState.Teach);
            }

            if (!isTeach)
            {
                CallMsg(CallState.WaitTeach);
            }
        }

        private void ResetState()
        {
            StopPlayer();
            ((TeacherViewModel)this.DataContext).IsStart = false;
            if (_timer.IsEnabled)
                _timer.Stop();

            TeachTime.Text = "00:00";

            CallMsg(CallState.CallFail);
            _CallState = CallState.Analyze;

            TeachState.Visibility = Visibility.Visible;
            StartConnTeachButton.Visibility = Visibility.Collapsed;
            StartTeachButton.Visibility = Visibility.Visible;
            FinishTeachButton.Visibility = Visibility.Collapsed;

            Messenger.Default.Send<bool>(true, "ResetState");
            Messenger.Default.Send<bool>(true, "ShowOrderListView");


        }


        private void StopPlayer()
        {
            if (_player != null)
                _player.Stop();
        }

        /// <summary>
        /// 结账
        /// </summary>
        /// <param name="rm"></param>
        private void FinishTeach(ResponseModel rm)
        {
            if (rm.Flag == 0)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    TeacherCount();
                    _isTCount = true;
                    FinishTeachWindow finishWindow = new FinishTeachWindow();
                    finishWindow._subjectOrder = rm.ResModel<SubjectOrder>();

                    finishWindow.ShowDialog();

                    log.InfoFormat("结算订单：{0}", _orderId);
                    this.Close();
                }));

            }
            {
                AskUtil.ShowAlert(rm.Msg);
            }

        }


        /// <summary>
        /// 取消订单
        /// </summary>
        public void CancelTeach(ResponseModel rm)
        {
            if (rm.Flag == 0)
            {
                //暂停播放
                StopPlayer();

                TeacherCount();
                _isTCount = true;
                log.InfoFormat("取消订单：{0}", _orderId);

                this.Close();
            }
            {
                AskUtil.ShowAlert(rm.Msg);
            }

        }

        private void CloseWindow()
        {
            ResponseModel rm = SubjectOrderAction.FinishOrder(_orderId);

            FinishTeach(rm);
        }


        Process ffProcess = null;
        string mvPath = "c:\\users\\public\\music\\askdata\\";
        string qiniuname = "";
        #region 白板
        //发起白板请求
        private void OpenRts(string callId)
        {

            if (string.IsNullOrEmpty(callId)) return;
            if (NIM.ClientAPI.GetLoginState() == NIMLoginState.kNIMLoginStateLogin)
            {
                NIM.ClientAPI.Relogin();
            }

            RegisterClientCallback();

            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                NIM.NIMRts.RtsStartInfo info = new NIM.NIMRts.RtsStartInfo();
                info.ApnsText = "发起教学";

                //大于0表示开启录制   audio 音频  data白板数据
                info.AudioRecord = 1;
                info.DataRecord = 1;
                //窗口比例
                info.CustomInfo = string.Format("{0}", DrawCanvas.ActualWidth / DrawCanvas.ActualHeight);

                log.InfoFormat("session_id:{0},callId:{1}", _sessionId,callId);

                RtsAPI.Start((NIM.NIMRts.NIMRtsChannelType.kNIMRtsChannelTypeTcp | NIM.NIMRts.NIMRtsChannelType.kNIMRtsChannelTypeVchat), callId.ToLower(), info,
                    (code, sessionId, channelType, uid) =>
                    {
                        this.Dispatcher.Invoke(() =>
                        {

                            log.Info("NIM.NIMRts-------code:" + code);
                            if (code == 200)
                            {
                                log.Info("邀请已发送，等待对方加入");
                            }
                            else if (code == 11001)
                            {
                                AskUtil.ShowAlert("邀请失败:学生端已下线或正在学习中");
                                log.Info("邀请失败:学生端已下线或正在学习中");
                            }else if (code == 11417)
                            {
                                CloseRts();
                                
                                log.InfoFormat("{0}", "rts会话 音视频已存在");
                            }
                            else
                            {
                                AskUtil.ShowAlert("邀请失败:学生端已下线或已取消接听");
                                //((NIM.ResponseCode)code).ToString()
                            }

                            if (code != 200)
                            {
                                //chuli
                                log.InfoFormat("呼叫失败sessionId:{0},code:{1}",sessionId,code);
                                //还原状态
                                ResetState();
                            }
                        });
                    });
            });

            RtsAPI.SetAckNotifyCallback((a, b, c, d) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    //挂断
                    if (!c)
                    {
                        //quitA();
                        AskUtil.ShowAlert("学生端已挂断，请取消重新接单");

                        //停止录制
                        stopScreenUp();

                        ResetState();
                        log.Info("学生端已挂断");
                    }
                    else if(!string.IsNullOrEmpty(a))
                    {
                        //sessionid 不为空
                        
                        _sessionId = a;
                        //接通
                        StopPlayer();
                        //开启音频
                        _sendDataTimer.Start();
                        OpenDevice();
                        SetRtsNotifyCallback();
                        NIM.DeviceAPI.StartDevice(NIM.NIMDeviceType.kNIMDeviceTypeAudioOutChat, "", 0, null);//开启扬声器播放对方语音

                        log.InfoFormat("连接成功sessionId:{0}",a);
                        if (_isRtsTeach)
                        {
                            CallMsg(CallState.Teach);
                        }

                        //设置按钮状态
                        ((TeacherViewModel)this.DataContext).IsStart = true;

                        //重新计时
                        TeachTime.Text = "00:00";
                        _seconds = 0;

                        if (!_timer.IsEnabled)
                        {
                            _timer.Start();
                        }
                        ////发送画板宽高
                        SendSizeCommand(DrawCanvas.ActualWidth, DrawCanvas.ActualHeight);

                        try
                        {
                            mvPath = mvPath + AskUtil.Teacher.Id + "-" + _sessionId + "\\";
                            if (!System.IO.Directory.Exists(mvPath))
                            {
                                System.IO.Directory.CreateDirectory(mvPath);//不存在就创建目录
                            }
                            //录制视频
                            qiniuname = "oto_"+AskUtil.Teacher.Id + "_" + _sessionId + ".mkv";
                           
                            mvPath = mvPath + qiniuname;
                            Console.WriteLine(mvPath);
                            log.Info(mvPath);
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                ffProcess = ScreenHelper.runFfmpegScreen(this.Title, (int)Math.Ceiling(_drawPoint.X), (int)Math.Ceiling(_drawPoint.Y), (int)Math.Ceiling(_drawAcWidth), (int)Math.Ceiling(_drawAcHeight) - 10, mvPath, null);
                            }));
                            
                        }
                        catch
                        {
                            
                        }
                    }
     
                });
            });

        }

        private void RegisterClientCallback()
        {
            NIM.ClientAPI.RegDisconnectedCb(() =>
            {
                AskUtil.ShowAlert("网络连接断开，网络恢复后后会自动重连");
                log.Info("网络连接断开");
            });

            NIM.ClientAPI.RegKickoutCb((r) =>
            {
                log.Info("被踢下线，请重新登录");
                SMessageBox.Show("被踢下线，请重新登录");
                ResetState();
                //踢下线操作
                Application.Current.Shutdown();
            });

            NIM.ClientAPI.RegAutoReloginCb((r) =>
            {
                if (r.Code == ResponseCode.kNIMResSuccess && r.LoginStep == NIMLoginStep.kNIMLoginStepLogin)
                {
                    //AskUtil.ShowAlert("重连成功");
                    log.Info("重连成功");
                }
            });
        }

        private void PlayWav()
        {
            _player.PlayLooping();
        }
        #endregion

        private void CallMsg(CallState state)
        {
            _CallState = state;
            string msg = "";
            if (state == CallState.Calling)
            {
                msg = "正在呼叫。。。";
            }
            else if (state == CallState.CallFail)
            {
                msg = "呼叫失败";

            }
            else if (state == CallState.CallTimeOut)
            {
                msg = "呼叫超时";
            }
            else if (_CallState == CallState.WaitTeach)
            {
                msg = "准备开始";
                StartConnTeachButton.Visibility = Visibility.Visible;
                StartTeachButton.Visibility = Visibility.Collapsed;
            }
            else if(_CallState == CallState.Teach)
            {
                TeachState.Visibility = Visibility.Hidden;
                StartConnTeachButton.Visibility = Visibility.Collapsed;
                StartTeachButton.Visibility = Visibility.Collapsed;
                FinishTeachButton.Visibility = Visibility.Visible;

                //发送开始教学指令 重新计时
                SendControlCommand(CommandType.DrawOpBeginTeach);
                _seconds = 0;
            }
            TeachState.Text = msg;
        }

        private void SameSubButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WbBrowser.Visibility = Visibility.Collapsed ==WbBrowser.Visibility ? Visibility.Visible :Visibility.Collapsed;
        }

        private void ScrollDraw_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //Console.WriteLine(e.VerticalOffset);
            //Console.WriteLine(e.VerticalChange);
            //Console.WriteLine(e.ViewportHeight);
            //Console.WriteLine(ScrollDraw.ScrollableHeight);

            //Console.WriteLine(DrawCanvas.Height - ScrollDraw.ScrollableHeight - e.VerticalOffset);
            //Console.WriteLine((DrawCanvas.Height - ScrollDraw.ScrollableHeight - e.VerticalOffset) / DrawCanvas.Height);

            //Console.WriteLine(Math.Round(0.5 - (DrawCanvas.Height - ScrollDraw.ScrollableHeight - e.VerticalOffset + 30.24999999999942) / DrawCanvas.Height, 2));
            //0.5到底
            if(!_drawing)
                SendScrollCommand(CommandType.DrawOpSrollDrawPanel, 0, Math.Round(0.5 - (DrawCanvas.Height - ScrollDraw.ScrollableHeight - e.VerticalOffset + 30.24999999999942) /DrawCanvas.Height,2));
        }

        int xzjd = 0;
        private void xzImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sImg.VerticalAlignment = VerticalAlignment.Center;
            double width = sImg.ActualWidth;
            double height = sImg.ActualHeight;
            xzjd += 90;
            xzjd = xzjd % 360;

            sImg.LayoutTransform = new RotateTransform(xzjd, width / 2, height / 2);


        }

        bool _isUp = false;

        public void stopScreenUp()
        {
            if (_isUp || String.IsNullOrEmpty(qiniuname)) return;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {

                try
                {
                    //停止录制
                    int i = ScreenHelper.stopScreen(ffProcess, null);
                    //上传
                    QiniuF qf = new QiniuF();
                    qf.LocalFile = mvPath;
                    qf.QiniuFile = qiniuname;
                    qf.OrderId = _orderId;
                    qf.Pid = ffProcess.Id;
                    log.Info("Send UploadQiniu");
                    Messenger.Default.Send<QiniuF>(qf, "UploadQiniu");
                }
                finally
                {
                    _isUp = true;
                }


            }));

        }
    }
}
