using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Media;
using System.Windows.Input;
using WPFClient.BAL;
using WPFClient.Common;
using WPFClient.Model;
using WPFClient.Util;
using static WPFClient.Model.SubjectOrder;

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
    public class SubjectOrdersViewModel : ViewModelBase
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(SubjectOrdersViewModel));

        private int start = 0;
        private int limit = 5;

        SoundPlayer _player;

        private static SubjectOrdersViewModel _subjectOrderListViewModel = null;

        private SubjectOrder _subjectOrder;

        public static SubjectOrdersViewModel GetSubjectOrdersViewModelInstance()
        {
            if (_subjectOrderListViewModel == null)
            {
                _subjectOrderListViewModel = new SubjectOrdersViewModel();
            }
            return _subjectOrderListViewModel;
        }

        /// <summary>
        /// 接单
        /// </summary>
        public RelayCommand<SubjectOrder> CommitCommand { get; private set; }
        /// <summary>
        /// 详情
        /// </summary>
        public RelayCommand<SubjectOrder> DetailCommand { get; private set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public SubjectOrdersViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            _player = new SoundPlayer();

            _subjectOrder = new SubjectOrder();

            GetSubjectOrder();

            DetailCommand = new RelayCommand<SubjectOrder>(order => ShowOrderDetailView(order));

            CommitCommand = new RelayCommand<SubjectOrder>(order =>
            {
                Messenger.Default.Send<SubjectOrder>(order, "ShowTeachView");
            });

            Messenger.Default.Register<bool>(this, "RefreshOrders", (r) =>
            {
                GetSubjectOrder();
            });
        }

        private void ShowOrderDetailView(SubjectOrder order)
        {
            Messenger.Default.Send<SubjectOrder>(order, "ShowOrderDetail");
        }

        private ObservableCollection<SubjectOrder> _subjectOrders;

        /// <summary>
        /// 订单信息集合
        /// </summary>
        public ObservableCollection<SubjectOrder> SubjectOrders
        {
            get { return _subjectOrders; }
            set
            {
                _subjectOrders = value;
                RaisePropertyChanged(() => SubjectOrders);
            }
        }

        public SubjectOrder SubjectOrder
        {
            get
            {
                return _subjectOrder;
            }

            set
            {
                _subjectOrder = value;
                RaisePropertyChanged(() => SubjectOrder);
            }
        }

        public void GetSubjectOrder()
        {
            ResponseModel rm = SubjectOrderAction.GetSubjectOrder(AskUtil.GetSubject(), AskUtil.GetGrade(), start, limit);

            if (rm.Flag == 0)
            {
                string r = rm.Content.ToString();
                SubjectsOrders sOrders = rm.ResModel<SubjectsOrders>();

                //Console.WriteLine(sOrders.Subjects.Count);
                //Console.WriteLine(sOrders.Subjects[0].Subject.Grade);
                //Console.WriteLine(sOrders.Subjects[0].Bill.Wanted);
                //Console.WriteLine(sOrders.Subjects[0].Student.Name);
                //Console.WriteLine(sOrders.Subjects[0].AskTime.UploadTime);

                //条数大于0 并且数据集有改变  播放声音
                if (sOrders.Subjects.Count > 0 && !sOrders.Equals(_subjectOrders)
                                            && bool.Parse(ConfigUtil.GetSettingString("isPlayVideo")))
                {
                    _player.SoundLocation = "Resources/Wav/ding.wav";
                    _player.Load();
                    _player.Play();
                }


                _subjectOrders = sOrders.Subjects;
                RaisePropertyChanged(() => SubjectOrders);
            }
            else
            {
                AskUtil.ShowAlert(rm.Msg);
            }
        }

    }
}