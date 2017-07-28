using GalaSoft.MvvmLight.Messaging;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPFClient.BAL;
using WPFClient.Common;
using WPFClient.Util;

namespace WPFClient.Form
{
    public partial class StudentCommentWindow : Window
    {
        public string _comment;
        public string _id;
        public string _name;

        public StudentCommentWindow()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ToStudentText.Text = _comment;
            StudentNameTxt.Text = _name;
            if (!string.IsNullOrEmpty(_comment))
            {
                ToStudentText.IsEnabled = false;
                OkButton.IsEnabled = false;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_comment))
            {
                return;
            }

            if (ToStudentText.Text.Trim() == "")
            {
                AskUtil.ShowAlert("请输入评论");
                return;
            }
            ResponseModel rm = SubjectOrderAction.ToStudentMsg(_id, ToStudentText.Text, -1);
            if(rm.Flag == 0)
            {
                AskUtil.ShowAlert("点评成功");

                Messenger.Default.Send<bool>(true, "RefOrders");
                Close();
            }else
            {
                AskUtil.ShowAlert(rm.Msg);
            }
        }

        private void ToStudentText_GotFocus(object sender, RoutedEventArgs e)
        {
            AskUtil.ShowTip();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
