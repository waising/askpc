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
    public partial class GetCashWindow : Window
    {
        public GetCashWindow()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (AskUtil.Teacher.AskInfo.Askcoin > 0)
            {
                AskMoney.Text = AskUtil.Teacher.AskInfo.Askcoin.ToString();
                GetMoney.Text = (AskUtil.Teacher.AskInfo.Askcoin * 1.0 / 10).ToString();
            }
            if (AskUtil.Teacher.PayInfo.Bandcards.Count > 0)
            {
                BankName.Text = AskUtil.Teacher.PayInfo.Bandcards[0].Company;
                BankNo.Text = AskUtil.Teacher.PayInfo.Bandcards[0].Account;
            }
            
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string money = MoneyTxt.Text;
            if (!string.IsNullOrEmpty(money))
            {
                System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^[+-]?\d+[\d]+(\.\d)?$");

                if (rex.IsMatch(money))
                {
                    if (double.Parse(money) < 100)
                    {
                        AskUtil.ShowAlert("最少提现金额为100元");
                        return;
                    }
                    if(double.Parse(money) > double.Parse(GetMoney.Text))
                    {
                        AskUtil.ShowAlert("提现金额超出可提现金额");
                        return;
                    }

                    ResponseModel rm = InfoAction.Withdraw(AskUtil.Teacher.Id, BankName.Text, BankNo.Text,int.Parse((double.Parse(money)*10).ToString()));

                    if (rm.Flag == 0)
                    {
                        AskUtil.ShowAlert("提现成功");

                        int i = int.Parse((double.Parse(money) * 10).ToString());
                        AskUtil.Teacher.AskInfo.Askcoin = AskUtil.Teacher.AskInfo.Askcoin - i;
                        Close();

                        Messenger.Default.Send<bool>(true, "RefMoneyDg");
                    }
                    else
                    {
                        AskUtil.ShowAlert(rm.Msg);
                    }

                }
                else
                {
                    AskUtil.ShowAlert("请输入正确的格式:100.5");
                }
            }
            else
            {
                AskUtil.ShowAlert("请输入提现金额");
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MoneyTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            AskUtil.ShowTip();
        }
    }
}
