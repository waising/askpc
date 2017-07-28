using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    /// <summary>
    /// 教师提现信息对象
    /// </summary>
    public class Withdraw : ObservableObject
    {
        private string _id;
        /// <summary>
        /// 申请时间
        /// </summary>
        private long _applicationDate;
        /// <summary>
        /// 提现金额（是asi币）
        /// </summary>
        private int _askcoin;
        /// <summary>
        /// 所在银行,登陆时获取
        /// </summary>
        private string _company;
        /// <summary>
        /// 银行账号
        /// </summary>
        private string _account;
        /// <summary>
        /// 申请状态
        ///1：申请中
        ///2：待转账
        ///3：已转账
        /// </summary>
        private int _state;
        /// <summary>
        /// 到账时间
        /// </summary>
        /// 
        private long _accountingDate;
        /// <summary>
        /// 操作员工
        /// </summary>
        private string _operator;
        /// <summary>
        /// 手机号（教师Id）
        /// </summary>
        private string _tel;

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long ApplicationDate
        {
            get
            {
                return _applicationDate;
            }

            set
            {
                _applicationDate = value;
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

        public int State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long AccountingDate
        {
            get
            {
                return _accountingDate;
            }

            set
            {
                _accountingDate = value;
            }
        }

        public string Operator
        {
            get
            {
                return _operator;
            }

            set
            {
                _operator = value;
            }
        }

        public string Tel
        {
            get
            {
                return _tel;
            }

            set
            {
                _tel = value;
            }
        }

        public int Askcoin
        {
            get
            {
                return _askcoin;
            }

            set
            {
                _askcoin = value;
            }
        }
    }

    public class Withdrawss : ObservableObject
    {
        private int _total;
        private ObservableCollection<Withdraw> _withdraws;

        public int Total
        {
            get
            {
                return _total;
            }

            set
            {
                _total = value;
            }
        }

        public ObservableCollection<Withdraw> Withdraws
        {
            get
            {
                return _withdraws;
            }

            set
            {
                _withdraws = value;
            }
        }
    }
}
