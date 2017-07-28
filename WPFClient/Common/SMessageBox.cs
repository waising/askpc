using WPFClient.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFClient
{
    /// <summary>
    /// CMessageBox显示的按钮类型
    /// </summary>
    public enum SMessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNo = 2,
        YesNoCancel = 3
    }

    /// <summary>
    /// CMessageBox显示的图标类型
    /// </summary>
    public enum SMessageBoxImage
    {
        None = 0,
        Error = 1,
        Question = 2,
        Warning = 3
    }

    /// <summary>
    /// 消息的重点显示按钮
    /// </summary>
    public enum SMessageBoxDefaultButton
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 3,
        No = 4
    }

    /// <summary>
    /// 消息框的返回值
    /// </summary>
    public enum SMessageBoxResult
    {
        //用户直接关闭了消息窗口
        None = 0,
        //用户点击确定按钮
        OK = 1,
        //用户点击取消按钮
        Cancel = 2,
        //用户点击是按钮
        Yes = 3,
        //用户点击否按钮
        No = 4
    }

    public class SMessageBox
    {
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="cmessageBoxText">消息内容</param>
        public static SMessageBoxResult Show(string cmessageBoxText)
        {
            SMessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new SMessageBoxWindow();
            }));
            window.MessageBoxText = cmessageBoxText;
            window.OKButtonVisibility = Visibility.Visible;
            Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    window.ShowDialog();
                }));
            return window.Result;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="cmessageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        public static SMessageBoxResult Show(string cmessageBoxText, string caption)
        {
            SMessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new SMessageBoxWindow();
            }));
            window.MessageBoxText = cmessageBoxText;
            window.MessageBoxTitle = caption;
            window.OKButtonVisibility = Visibility.Visible;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window.ShowDialog();
            }));
            return window.Result;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="cmessageBoxText">消息内容</param>
        /// <param name="CMessageBoxButton">消息框按钮</param>
        public static SMessageBoxResult Show(string cmessageBoxText, SMessageBoxButton CMessageBoxButton)
        {
            SMessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new SMessageBoxWindow();
            }));
            window.MessageBoxText = cmessageBoxText;
            switch(CMessageBoxButton)
            {
                case SMessageBoxButton.OK:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.OKCancel:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNo:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNoCancel:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window.ShowDialog();
            }));
            return window.Result;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="cmessageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        /// <param name="CMessageBoxButton">消息框按钮</param>
        public static SMessageBoxResult Show(string cmessageBoxText, string caption, SMessageBoxButton CMessageBoxButton)
        {
            SMessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new SMessageBoxWindow();
            }));
            window.MessageBoxText = cmessageBoxText;
            window.MessageBoxTitle = caption;
            switch (CMessageBoxButton)
            {
                case SMessageBoxButton.OK:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.OKCancel:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNo:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNoCancel:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window.ShowDialog();
            }));
            return window.Result;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="cmessageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        /// <param name="CMessageBoxButton">消息框按钮</param>
        /// <param name="CMessageBoxImage">消息框图标</param>
        /// <returns></returns>
        public static SMessageBoxResult Show(string cmessageBoxText, string caption, SMessageBoxButton CMessageBoxButton, SMessageBoxImage CMessageBoxImage)
        {
            SMessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new SMessageBoxWindow();
            }));
            
            window.MessageBoxText = cmessageBoxText;
            window.MessageBoxTitle = caption;
            switch (CMessageBoxButton)
            {
                case SMessageBoxButton.OK:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.OKCancel:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNo:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNoCancel:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window.ShowDialog();
            }));
            return window.Result;
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="cmessageBoxText">消息内容</param>
        /// <param name="caption">消息标题</param>
        /// <param name="CMessageBoxButton">消息框按钮</param>
        /// <param name="CMessageBoxImage">消息框图标</param>
        /// <param name="CMessageBoxDefaultButton">消息框默认按钮</param>
        /// <returns></returns>
        public static SMessageBoxResult Show(string cmessageBoxText, string caption, SMessageBoxButton CMessageBoxButton, SMessageBoxImage CMessageBoxImage, SMessageBoxDefaultButton CMessageBoxDefaultButton)
        {
            SMessageBoxWindow window = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window = new SMessageBoxWindow();
            }));
            window.MessageBoxText = cmessageBoxText;
            window.MessageBoxTitle = caption;

            #region 按钮
            switch (CMessageBoxButton)
            {
                case SMessageBoxButton.OK:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.OKCancel:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNo:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        break;
                    }
                case SMessageBoxButton.YesNoCancel:
                    {
                        window.YesButtonVisibility = Visibility.Visible;
                        window.NoButtonVisibility = Visibility.Visible;
                        window.CancelButtonVisibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        window.OKButtonVisibility = Visibility.Visible;
                        break;
                    }
            }
            #endregion

            #region 默认按钮
            switch(CMessageBoxDefaultButton)
            {
                case SMessageBoxDefaultButton.OK:
                    {
                        window.OKButtonStyle = ButtonStyle.NormalButtonStyle;
                        window.CancelButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.YesButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.NoButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        break;
                    }
                case SMessageBoxDefaultButton.Cancel:
                    {
                        window.OKButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.CancelButtonStyle = ButtonStyle.NormalButtonStyle;
                        window.YesButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.NoButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        break;
                    }
                case SMessageBoxDefaultButton.Yes:
                    {
                        window.OKButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.CancelButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.YesButtonStyle = ButtonStyle.NormalButtonStyle;
                        window.NoButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        break;
                    }
                case SMessageBoxDefaultButton.No:
                    {
                        window.OKButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.CancelButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.YesButtonStyle = ButtonStyle.NotNormalButtonStyle;
                        window.NoButtonStyle = ButtonStyle.NormalButtonStyle;
                        break;
                    }
                case SMessageBoxDefaultButton.None:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            #endregion

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                window.ShowDialog();
            }));
            return window.Result;
        }
    }
}
