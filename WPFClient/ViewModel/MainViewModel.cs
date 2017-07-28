using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Media;
using System.Windows.Input;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainViewModel));

        private static MainViewModel _mainViewModel = null;

        public static MainViewModel GetMainViewModelInstance()
        {
            if (_mainViewModel == null)
            {
                _mainViewModel = new MainViewModel();
            }
            return _mainViewModel;
        }

        private string _avatar;
        public ICommand InfoCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public string Avatar
        {
            get
            {
                return _avatar;
            }

            set
            {
                _avatar = value;
                RaisePropertyChanged(() => Avatar);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            InfoCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send<bool>(true, "ShowInfoWindow");
            });

            RefreshCommand = new RelayCommand(() =>
            {
                SubjectOrdersViewModel.GetSubjectOrdersViewModelInstance().GetSubjectOrder();

                Messenger.Default.Send<bool>(false, "RefreshOrders");
                
            });

            Avatar = AskUtil.Teacher.AskInfo.Avatar;

        }

    }
}