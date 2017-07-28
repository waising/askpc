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
    public partial class ShowContentWindow : Window
    {
        public ShowContentWindow()
        {
            InitializeComponent();
            this.Topmost = true;
        }
        public string _title;
        public string _content;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTxt.Text = _title;
            ContentTxt.Text = _content;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
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
