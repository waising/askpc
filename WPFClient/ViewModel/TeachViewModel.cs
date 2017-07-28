using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFClient.BAL;
using WPFClient.Common;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient.ViewModel
{
    public class TeacherViewModel : ViewModelBase
    {
        private static TeacherViewModel _teacherViewModel = null;

        private SubjectOrder _subjectOrder;

        private bool _isStart;
        private bool _isTeach;
        private int start = 0;
        private int limit = 3;
        private AskPen _pen;

        public static TeacherViewModel GetTeachViewModelInstance()
        {
            if (_teacherViewModel == null)
            {
                _teacherViewModel = new TeacherViewModel();
            }
            return _teacherViewModel;
        }

        private bool _isPen;
        private bool _isEraser;
        /// <summary>
        /// 颜色
        /// </summary>
        public RelayCommand<object> PenColorRdoCommand { get; private set; }

        /// <summary>
        /// 粗细
        /// </summary>
        public RelayCommand<string> PenSizeRdoCommand { get; private set; }

        /// <summary>
        /// 取消
        /// </summary>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// 结算
        /// </summary>
        public RelayCommand FinishCommand { get; private set; }
        
        /// <summary>
        /// 笔样式
        /// </summary>
        public RelayCommand<string> PenStyleCommand { get; private set; }

        /// <summary>
        /// 开始
        /// </summary>
        public RelayCommand<string> StartCommand { get; private set; }

        public TeacherViewModel()
        {
            _subjectOrder = (SubjectOrder)Application.Current.Properties["OrderDetail"];

            _pen = new AskPen();
            _pen.Style = 0;
            _pen.Size = 2;
            _pen.Color = "#FF000000";

            IsEraser = false;
            IsPen = true;

            if (_subjectOrder != null)
            {
                SubjectURL = HttpHelper.HTTP_SUBJECT_HTML_URL + AskUtil.GetParams(new object[] { "id=" + _subjectOrder.Id, "start=" + start, "limit=" + limit });
                //Console.WriteLine(_subjectOrder.Student.Name);
                Console.WriteLine(SubjectURL);
            }

            _isStart = false;
            _isTeach = true;
            InitCommand();

            //
            Messenger.Default.Register<bool>(this, "ResetState", (e) =>
            {
                IsStart = false;
            });

            Messenger.Default.Register<SubjectOrder>(this, "TeachOrder", (order) =>
            {
                SubjectOrder = order;
            });
        }

        private void InitCommand()
        {
            PenColorRdoCommand = new RelayCommand<object>(color =>
            {
                _pen.Color = color.ToString();
                _pen.Style = PenStyleEnum.Pen;
                IsEraser = false;
                IsPen = true;

                SendPen();
            });

            PenSizeRdoCommand = new RelayCommand<string>(size =>
            {
                _pen.Size = int.Parse(size);
                _pen.Style = PenStyleEnum.Pen;
                IsEraser = false;
                IsPen = true;
                SendPen();
            });

            CancelCommand = new RelayCommand(() => CancelTeach());

            FinishCommand = new RelayCommand(() =>
            {
                FinishTeach();
            });

            StartCommand = new RelayCommand<string>((type) => 
            {
                StartTeach();
            });

            PenStyleCommand = new RelayCommand<string>((style) =>
            {
                _pen.Style = (PenStyleEnum)Enum.ToObject(typeof(PenStyleEnum), int.Parse(style));
                SendPen();
            });
        }

        public void StartTeach()
        {
            ResponseModel rm = SubjectOrderAction.StartTeach(_subjectOrder.Id);

            if (rm.Flag == 0)
            {
                Messenger.Default.Send<SubjectOrder>(_subjectOrder, "StartTeach");
            }else
            {
                AskUtil.ShowAlert(rm.Msg);
                Messenger.Default.Send<bool>(true, "TeachFail");
            }

        }

        public void StartConn()
        {
            ResponseModel rm = SubjectOrderAction.Connect(_subjectOrder.Id);
            if(rm.Flag == 0)
                Messenger.Default.Send<SubjectOrder>(_subjectOrder, "StartConn");
            else
            {
                AskUtil.ShowAlert(rm.Msg);
                Messenger.Default.Send<bool>(true, "TeachFail");
            }
        }

        private void CancelTeach()
        {
            if (SMessageBox.Show("取消订单将不会得到任何奖励,请确认.", "结束教学", SMessageBoxButton.OKCancel) == SMessageBoxResult.OK)
            {
                ResponseModel rm = SubjectOrderAction.GiveUpOrder(_subjectOrder.Id);
                Messenger.Default.Send<ResponseModel>(rm, "CancelCommand");
                
            }
        }

        //结账
        private void FinishTeach()
        {
            //结算
            if (SMessageBox.Show("确定结束本次教学?", "结束教学", SMessageBoxButton.OKCancel) == SMessageBoxResult.OK)
            {
                ResponseModel rm = SubjectOrderAction.FinishOrder(_subjectOrder.Id);
                Messenger.Default.Send<ResponseModel>(rm, "FinishCommand");
            }

        }


        private void SendPen()
        {
            Messenger.Default.Send<AskPen>(_pen, "PenStyle");
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

        public string SubjectURL
        {
            get
            {
                return _subjectURL;
            }

            set
            {
                _subjectURL = value;
                RaisePropertyChanged(() => SubjectURL);
            }
        }

        public AskPen Pen
        {
            get
            {
                return _pen;
            }

            set
            {
                _pen = value;
                RaisePropertyChanged(() => Pen);
            }
        }

        public bool IsStart
        {
            get
            {
                return _isStart;
            }

            set
            {
                _isStart = value;
                RaisePropertyChanged(() => IsStart);
            }
        }

        public bool IsTeach
        {
            get
            {
                return _isTeach;
            }

            set
            {
                _isTeach = value;
                RaisePropertyChanged(() => IsTeach);
            }
        }

        public bool IsPen
        {
            get
            {
                return _isPen;
            }

            set
            {
                _isPen = value;
                RaisePropertyChanged(() => IsPen);
            }
        }

        public bool IsEraser
        {
            get
            {
                return _isEraser;
            }

            set
            {
                _isEraser = value;
                RaisePropertyChanged(() => IsEraser);
            }
        }

        private string _subjectURL;

    }
}
