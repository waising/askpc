using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClient.ViewModel;

namespace WPFClient.View
{
    /// <summary>
    /// SubjectOrderListView.xaml 的交互逻辑
    /// </summary>
    public partial class SubjectOrdersView : Page
    {
        public SubjectOrdersView()
        {
            this.DataContext = SubjectOrdersViewModel.GetSubjectOrdersViewModelInstance();
            InitializeComponent();
        }
    }
}
