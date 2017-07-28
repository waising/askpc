using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Bill : ObservableObject
    {
        /// <summary>
        /// 悬赏金额
        /// </summary>
        private int _wanted;
        /// <summary>
        /// 结帐价格(asb)
        /// </summary>
        private int _price;

        public int Wanted
        {
            get
            {
                return _wanted;
            }

            set
            {
                _wanted = value;
            }
        }

        public int Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
            }
        }
    }
}
