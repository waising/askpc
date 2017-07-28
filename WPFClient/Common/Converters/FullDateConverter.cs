using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPFClient.Util;

namespace WPFClient.Common.Converters
{
    /// <summary>
    /// 时间戳转日期
    /// </summary>
    public class FullDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null && (long)value!=0)
                return DateUtil.ConvertTimeToDateTimeString((long)value);
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
