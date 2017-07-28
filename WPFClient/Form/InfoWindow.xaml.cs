using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WPFClient.Util;
using WPFClient.ViewModel;

namespace WPFClient.Form
{
    public partial class InfoWindow : Window
    {
        #region 初始化
        public InfoWindow()
        {
            this.DataContext = InfoViewModel.GetInfoViewModelInstance();
            InitializeComponent();          
        }

        bool _isLoaded = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ((InfoViewModel)this.DataContext).SetTeacher();

            OrderPager.PageCurrent = 1;
            OrderPager.NMax = ((InfoViewModel)this.DataContext).LoadTeachRecordData(OrderPager.PageCurrent, OrderPager.PageSize);

            MoneyPager.PageCurrent = 1; 
            MoneyPager.NMax = ((InfoViewModel)this.DataContext).LoadMoneyRecordData(MoneyPager.PageCurrent, MoneyPager.PageSize);

            MessagePager.PageCurrent = 1;
            MessagePager.NMax = ((InfoViewModel)this.DataContext).LoadMessageRecordData(MessagePager.PageCurrent, MessagePager.PageSize);

            _isLoaded = true;

            expTxt.Text = string.Format("{0}",AskUtil.Teacher.AskInfo.Exprience);
            expMaxTxt.Text = string.Format("/{0}", AskUtil.Teacher.AskInfo.ExprienceMax);
            starTxt.Text = string.Format("{0}%", ((InfoViewModel)this.DataContext).Star);

            Messenger.Default.Register<bool>(this, "RefMoneyDg", (msg) =>
            {
                MoneyPager_EventPaging(null);
            });

            Messenger.Default.Register<bool>(this, "RefOrders", (msg) =>
            {
                OrderPager_EventPaging(null);
            });

        }
        #endregion

        #region 关闭
        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        
        private int OrderPager_EventPaging(Controller.EventPagingArg e)
        {
            return ((InfoViewModel)this.DataContext).LoadTeachRecordData(OrderPager.PageCurrent, OrderPager.PageSize);
        }
        
        private int MoneyPager_EventPaging(Controller.EventPagingArg e)
        {
            return ((InfoViewModel)this.DataContext).LoadMoneyRecordData(MoneyPager.PageCurrent, MoneyPager.PageSize);
        }

        private int MessagePager_EventPaging(Controller.EventPagingArg e)
        {
            return ((InfoViewModel)this.DataContext).LoadMessageRecordData(MessagePager.PageCurrent, MessagePager.PageSize);
        }


        private void OrderRb_Checked(object sender, RoutedEventArgs e)
        {
            if (_isLoaded)
            {
                OrdersPanel.Visibility = Visibility.Visible;
                MoneyPanel.Visibility = Visibility.Collapsed;
                MessagePanel.Visibility = Visibility.Collapsed;

                MoneyBtn.Visibility = Visibility.Collapsed;
                Tel.Visibility = Visibility.Collapsed;
                TelIco.Visibility = Visibility.Collapsed;
            }

        }

        private void MoneyRb_Checked(object sender, RoutedEventArgs e)
        {
            OrdersPanel.Visibility = Visibility.Collapsed;
            MoneyPanel.Visibility = Visibility.Visible;
            MessagePanel.Visibility = Visibility.Collapsed;

            MoneyBtn.Visibility = Visibility.Visible;
            Tel.Visibility = Visibility.Collapsed;
            TelIco.Visibility = Visibility.Collapsed;
        }

        private void MessageRb_Checked(object sender, RoutedEventArgs e)
        {
            OrdersPanel.Visibility = Visibility.Collapsed;
            MoneyPanel.Visibility = Visibility.Collapsed;
            MessagePanel.Visibility = Visibility.Visible;

            MoneyBtn.Visibility = Visibility.Collapsed;
            Tel.Visibility = Visibility.Visible;
            TelIco.Visibility = Visibility.Visible;
        }

        private void MoneyBtn_Click(object sender, RoutedEventArgs e)
        {
            new GetCashWindow().ShowDialog();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new ImageWindow().ShowDialog();
        }
    }
}
