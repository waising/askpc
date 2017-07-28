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
    public class BankStateConverter : IValueConverter
    {

        private Dictionary<int,string> _stateDic;
        public BankStateConverter()
        {
            SetStateDic();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int key = (int)value;
            return _stateDic[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private void SetStateDic()
        {
            _stateDic = new Dictionary<int,string>();
            _stateDic.Add(1,"申请中");
            _stateDic.Add(2, "待转账");
            _stateDic.Add(3, "已转账");
        }
    }
}
