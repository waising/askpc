using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class PayInfo : ObservableObject
    {
        private List<Bank> _bandcards;

        public List<Bank> Bandcards
        {
            get
            {
                return _bandcards;
            }

            set
            {
                _bandcards = value;
            }
        }
    }

    public class Bank : ObservableObject
    {
        /// <summary>
        /// 账户
        /// </summary>
        private string _account;
        /// <summary>
        /// 名称
        /// </summary>
        private string _company;

        public string Account
        {
            get
            {
                return _account;
            }

            set
            {
                _account = value;
                
            }
        }

        public string Company
        {
            get
            {
                return _company;
            }

            set
            {
                _company = value;
            }
        }
    }

}
