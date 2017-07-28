using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPFClient.Util;

namespace WPFClient.Common.Converters
{
    public class GradeConverter : IValueConverter
    {

        private Dictionary<int,string> _gradeDic;
        public GradeConverter()
        {
            SetGradeDic();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int key = (int)value;
            return _gradeDic[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private void SetGradeDic()
        {
            _gradeDic = new Dictionary<int,string>();
            _gradeDic.Add(1,"一年级");
            _gradeDic.Add(2, "二年级");
            _gradeDic.Add(3, "三年级");
            _gradeDic.Add(4, "四年级");
            _gradeDic.Add(5, "五年级");
            _gradeDic.Add(6, "六年级");
            _gradeDic.Add(7, "七年级");
            _gradeDic.Add(8, "八年级");
            _gradeDic.Add(9, "九年级");
            _gradeDic.Add(10, "高一");
            _gradeDic.Add(11, "高二");
            _gradeDic.Add(12, "高三");
        }
    }
}
