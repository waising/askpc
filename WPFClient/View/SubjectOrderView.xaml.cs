using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFClient.ViewModel
{
    /// <summary>
    /// SubjectOrderView.xaml 的交互逻辑
    /// </summary>
    public partial class SubjectOrderView : Page
    {
        public SubjectOrderView()
        {
            this.DataContext = SubjectOrderViewModel.GetSubjectOrderViewModelInstance();
            InitializeComponent();
            try
            {
                double h = Double.Parse(Application.Current.Properties["MainHeight"].ToString());
                if (h > 0)
                {
                    WbBrowser.Height = h - 30;
                    gridMain.Height = h;
                }
            }
                    
            finally
            {

            }

            //WbBrowser.Source = new Uri("http://192.168.9.202:21704/commonapi/subject/search/html?stage=2&code=M&s=1&start=0&limit=3");
        }

        private double _W = 1080;
        private double _H = 800;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string wanted = BillWantedTxt.Text;
            if (string.IsNullOrEmpty(wanted) || BillWantedTxt.Text == "0")
            {
                BillWantedPanel.Visibility = Visibility.Hidden;
            }

            _W = sImg.ActualWidth;
            _H = sImg.ActualHeight;

            //SMessageBox.Show(_W.ToString());
            //SMessageBox.Show(_H.ToString());
        }

        private int xzjd = 0;
        private void xzImg_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            sImg.VerticalAlignment = VerticalAlignment.Center;
            double width = sImg.ActualWidth;
            double height = sImg.ActualHeight;
            xzjd += 90;
            xzjd = xzjd % 360;

            sImg.LayoutTransform = new RotateTransform(xzjd, width / 2, height / 2);

            
            //sImg.Margin = new Thickness(0,-120,0,0);
        }
    }
}
