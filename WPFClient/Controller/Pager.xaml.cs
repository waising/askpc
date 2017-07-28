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

namespace WPFClient.Controller
{

    /// <summary>
    /// 申明委托
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate int EventPagingHandler(EventPagingArg e);
    /// <summary>
    /// Pager.xaml 的交互逻辑
    /// </summary>
    public partial class Pager : UserControl
    {
        public Pager()
        {
            InitializeComponent();
        }

        public event EventPagingHandler EventPaging;
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        private int _pageSize = 20;
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                GetPageCount();
            }
        }

        private int _nMax = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int NMax
        {
            get { return _nMax; }
            set
            {
                _nMax = value;
                GetPageCount();
                lblinfo.Content = "" + NMax + "条 " + this.PageCurrent.ToString() + "/" + this.PageCount.ToString() + "";

                this.txtCurrentPage.Text = this.PageCurrent.ToString();
            }
        }

        private int _pageCount = 0;
        /// <summary>
        /// 页数=总记录数/每页显示记录数
        /// </summary>
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        private int _pageCurrent = 1;
        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageCurrent
        {
            get { return _pageCurrent; }
            set { _pageCurrent = value; }
        }



        private void GetPageCount()
        {
            if (this.NMax > 0)
            {
                this.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.NMax) / Convert.ToDouble(this.PageSize)));
            }
            else
            {
                this.PageCount = 0;
            }
        }

        /// <summary>
        /// 翻页控件数据绑定的方法
        /// </summary>
        public void Bind()
        {
            if (this.EventPaging != null)
            {
                this.NMax = this.EventPaging(new EventPagingArg(this.PageCurrent));
            }

            if (this.PageCurrent > this.PageCount)
            {
                this.PageCurrent = this.PageCount;
            }
            if (this.PageCount == 1)
            {
                this.PageCurrent = 1;
            }
            lblinfo.Content = "" + NMax + "条 " + this.PageCurrent.ToString() + "/" + this.PageCount.ToString() + "";

            this.txtCurrentPage.Text = this.PageCurrent.ToString();

            if (this.PageCurrent == 1)
            {
                this.btnPrev.IsEnabled = false;
                this.btnFirst.IsEnabled = false;
            }
            else
            {
                btnPrev.IsEnabled = true;
                btnFirst.IsEnabled = true;
            }

            if (this.PageCurrent == this.PageCount)
            {
                this.btnLast.IsEnabled = false;
                this.btnNext.IsEnabled = false;
            }
            else
            {
                btnLast.IsEnabled = true;
                btnNext.IsEnabled = true;
            }

            if (this.NMax == 0)
            {
                btnNext.IsEnabled = false;
                btnLast.IsEnabled = false;
                btnFirst.IsEnabled = false;
                btnPrev.IsEnabled = false;
            }
        }



        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            PageCurrent = PageCount;
            this.Bind();
        }




        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.PageCurrent += 1;
            if (PageCurrent > PageCount)
            {
                PageCurrent = PageCount;
            }
            this.Bind();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtCurrentPage.Text != null && txtCurrentPage.Text != "")
            {
                if (Int32.TryParse(txtCurrentPage.Text, out _pageCurrent))
                {
                    this.Bind();
                }
                else
                {
                    SMessageBox.Show("输入数字格式错误！");
                }
            }
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            PageCurrent = 1;
            this.Bind();
        }



        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            PageCurrent -= 1;
            if (PageCurrent <= 0)
            {
                PageCurrent = 1;
            }
            this.Bind();
        }

    }
    /// <summary>
    /// 自定义事件数据基类
    /// </summary>
    public class EventPagingArg : EventArgs
    {
        private int _intPageIndex;
        public EventPagingArg(int PageIndex)
        {
            _intPageIndex = PageIndex;
        }
    }
}

