using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Student : ObservableObject
    {
        /// <summary>
        /// 帐号
        /// </summary>
        private string _account;
        /// <summary>
        /// 名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 头像
        /// </summary>
        private string _avatar;

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

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Avatar
        {
            get
            {
                return _avatar;
            }

            set
            {
                _avatar = value;
            }
        }
    }
}
