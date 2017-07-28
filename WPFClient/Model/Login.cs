using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Login : ObservableObject
    {
        private string _userName;
        private string _password;
        private bool _isRemember;
        private bool _isAutoLogin;

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool IsRemember
        {
            get
            {
                return _isRemember;
            }

            set
            {
                _isRemember = value;
                RaisePropertyChanged(() => IsRemember);
            }
        }

        public bool IsAutoLogin
        {
            get
            {
                return _isAutoLogin;
            }

            set
            {
                _isAutoLogin = value;
                RaisePropertyChanged(() => IsAutoLogin);
            }
        }
    }

    public class NIMToken : ObservableObject
    {
        private string _id;
        private string _accid;
        private string _token;
        private long _date;

        public string Accid
        {
            get
            {
                return _accid;
            }

            set
            {
                _accid = value;
                RaisePropertyChanged(() => Accid);
            }
        }

        public string Token
        {
            get
            {
                return _token;
            }

            set
            {
                _token = value;
                RaisePropertyChanged(() => Token);
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public long Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
                RaisePropertyChanged(() => Date);
            }
        }
    }
}
