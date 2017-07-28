using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFClient.Common;
using WPFClient.Model;
using WPFClient.Util;
using System.Runtime.InteropServices;
using GalaSoft.MvvmLight.Messaging;

namespace WPFClient.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoginViewModel));


        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        private static LoginViewModel _loginViewModel = null;

        private Login _login;
        
        public ICommand LoginCommand { get; private set; }
        public ICommand RememberCommand { get; private set; }
        public ICommand AutoLoginCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public static LoginViewModel getLoginViewModelInstance()
        {
            if (_loginViewModel == null)
            {
                _loginViewModel = new LoginViewModel();
            }
            return _loginViewModel;
        }

        public LoginViewModel()
        {
            _login = new Login();

            InitCommand();

            log.Info("--------------登录-----------");

            ShowLoginInfo();

            //自动登录
            if (bool.Parse(ConfigUtil.GetSettingString("isAutoLogin")))
            {
                Login();
            }

        }

        private void InitCommand()
        {
            LoginCommand = new RelayCommand(() => Login(), () => true);

            RememberCommand = new RelayCommand(() =>
            {
                _login.IsRemember = _login.IsRemember;
                if (!_login.IsRemember) _login.IsAutoLogin = false;
            });
            AutoLoginCommand = new RelayCommand(() =>
            {
                _login.IsAutoLogin = _login.IsAutoLogin;
                if (_login.IsAutoLogin)
                    _login.IsRemember = true;
            });

            CloseCommand = new RelayCommand(() => CloseLoginForm(), () => true);
        }

        public void Login()
        {
            if (!IsInternetAvailable())
            {
                SMessageBox.Show("网络异常，请检查网络连接");
                return;
            }

            if(String.IsNullOrEmpty(_login.UserName) || String.IsNullOrEmpty(_login.Password))
            {
                AskUtil.ShowAlert("用户名或密码不能为空");
                //SMessageBox.Show("用户名或密码不能为空", "信息提示", SMessageBoxButton.OK);
                return;
            }
            //Messenger.Default.Send<bool>(true, "ShowMainForm");
            ResponseModel rm = LoginAction.Login(_login.UserName, _login.Password);

            if (rm.Flag == 0)
            {
                SaveLoginInfo();
                //
                AskUtil.Teacher = rm.ResModel<Teacher>();
                Console.WriteLine(rm.Content);

                AskUtil.Login = _login;
                log.InfoFormat("登录id:{0}", _login.UserName);

                Messenger.Default.Send<bool>(true, "ShowMainForm");
            }
            else
            {
                AskUtil.ShowAlert(rm.Msg);
            }

        }


        private void CloseLoginForm()
        {
            SaveLoginInfo();
            Messenger.Default.Send<bool>(true, "CloseLoginForm");
        }

        private void SaveLoginInfo()
        {
            //记录登录信息
            ConfigUtil.UpdateSettingString("UserName", _login.UserName);
            ConfigUtil.UpdateSettingString("Password", _login.IsRemember ? _login.Password : "");
            ConfigUtil.UpdateSettingString("isRemember", _login.IsRemember.ToString());
            ConfigUtil.UpdateSettingString("isAutoLogin", _login.IsAutoLogin.ToString());
        }

        private void ShowLoginInfo()
        {
            _login.UserName = ConfigUtil.GetSettingString("UserName");

            _login.IsRemember = bool.Parse(ConfigUtil.GetSettingString("isRemember"));
            _login.IsAutoLogin = bool.Parse(ConfigUtil.GetSettingString("isAutoLogin"));

            if (_login.IsRemember)
                _login.Password = ConfigUtil.GetSettingString("Password");
        }

        public Login LoginModel
        {
            get
            {
                return _login;
            }

            set
            {
                _login = value;
                RaisePropertyChanged(() => LoginModel);
            }
        }

      
        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
        }
    }
}
