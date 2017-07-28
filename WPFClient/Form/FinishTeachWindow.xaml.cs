using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.BAL;
using WPFClient.Common;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient.Form
{
    public partial class FinishTeachWindow : Window
    {

        log4net.ILog log = log4net.LogManager.GetLogger(typeof(FinishTeachWindow));

        public SubjectOrder _subjectOrder;

        #region 初始化
        public FinishTeachWindow()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string id = "58d8a7ef6380b700075f26f5";
            //ResponseModel rm = SubjectOrderAction.FinishOrder(_id);

            //if (rm.Flag == 0)
            //{

            //}

            if (_subjectOrder != null)
            {
                PriceTxt.Text = string.Format("{0}",_subjectOrder.Bill.Price);
                WantedTxt.Text = string.Format("{0}", _subjectOrder.Bill.Wanted);

                TimeSpan ts = new TimeSpan(0, 0, (int)_subjectOrder.AskTime.HoldingSeconds);

                string h = string.Format("{0}", (int)ts.Hours).PadLeft(2, '0');
                string m = string.Format("{0}", ts.Minutes).PadLeft(2, '0');
                string s = string.Format("{0}", ts.Seconds).PadLeft(2, '0');

                TimeTxt.Text = string.Format("{0}:{1}:{2}", h, m, s);
            }

            btnOk.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(btnOk_MouseLeftButtonDown), true);
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void btnOk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //给学生的话
            int same = SameSubjectCK.IsChecked.Value ? 1 : 0;
            string t = ToStudentText.Text;
            if (!string.IsNullOrEmpty(t) && _subjectOrder!=null)
            {
                ResponseModel rm = SubjectOrderAction.ToStudentMsg(_subjectOrder.Id, t, same);
                if (rm.Flag == 0)
                {
                    log.Info("toStudent success"); 
                }
            }

            Close();
        }

        private void ToStudentText_GotFocus(object sender, RoutedEventArgs e)
        {
            AskUtil.ShowTip();
        }
    }
}
