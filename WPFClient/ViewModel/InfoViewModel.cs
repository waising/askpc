using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFClient.BAL;
using WPFClient.Common;
using WPFClient.Form;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient.ViewModel
{
    public class InfoViewModel : ViewModelBase
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(InfoViewModel));

        private int _start = 0;

        private static InfoViewModel _infoViewModel = null;

        public static InfoViewModel GetInfoViewModelInstance()
        {
            if (_infoViewModel == null)
            {
                _infoViewModel = new InfoViewModel();
            }
            return _infoViewModel;
        }

        private Teacher _teacher;

        private ObservableCollection<TeachRecord> _teachRecords;

        private ObservableCollection<Message> _messagesss;

        private ObservableCollection<Withdraw> _withdraws;

        public RelayCommand<TeachRecord> SeeCommand { get; private set; }
        public RelayCommand<TeachRecord> CommentCommand { get; private set; }
        /// <summary>
        /// 删除
        /// </summary>
        public RelayCommand<Message> DeleteCommand { get; private set; }
        private double _star;
        private double _exprience;

        public InfoViewModel()
        {
            //SetTeacher();

            //辅导列表
            //LoadTeachRecordData(1,5);
            //提现记录
            //LoadMoneyRecordData(1,5);
            //消息记录
            //LoadGetMessageRecordData(1,5);

            SeeCommand = new RelayCommand<TeachRecord>(msg =>
            {
            if (msg.Evaluation != null &&  !string.IsNullOrEmpty(msg.Evaluation.Suggest))
                    ShowContentWindow(msg.Student.Name+"的评论",msg.Evaluation.Suggest);
                else
                {
                    AskUtil.ShowAlert("暂无数据");
                }
            });

            CommentCommand = new RelayCommand<TeachRecord>(msg =>
            {
                if (msg.Teacher == null)
                {
                    ShowCommentWindow(msg.Id, msg.Student.Name, "");
                }
                else
                {
                    ShowCommentWindow(msg.Id, msg.Student.Name, msg.Teacher.ToStudent);
                }
                
            });

            DeleteCommand = new RelayCommand<Message>(m => DeleteMsg(m));
        }

        private void ShowContentWindow(string title,string content)
        {
            ShowContentWindow scw = new ShowContentWindow();
            scw._title = title;
            scw._content = content;
            scw.ShowDialog();
        }

        private void ShowCommentWindow(string title,string name, string content)
        {
            StudentCommentWindow scw = new StudentCommentWindow();
            scw._id = title;
            scw._comment = content;
            scw._name = name;
            scw.ShowDialog();
        }

        public void SetTeacher()
        {
            ResponseModel rm = InfoAction.GetTeacher(AskUtil.Login.UserName, AskUtil.Login.Password);
            if (rm.Flag == 0)
            {
                Teacher = rm.ResModel<Teacher>();
                AskUtil.Teacher = Teacher;

                Star = double.Parse(string.Format("{0:N2}", Teacher.AskInfo.Star*100));
                Exprience = Teacher.AskInfo.Exprience * 1.00 / Teacher.AskInfo.ExprienceMax * 100;
                //Console.WriteLine(Teacher.AskInfo.ExprienceMax);
            }
            else
            {
                AskUtil.ShowAlert(rm.Msg);
            }

        }

        private void DeleteMsg(Message m)
        {
            if (SMessageBox.Show("确定要删除此条信息?", "删除信息", SMessageBoxButton.YesNo) == SMessageBoxResult.No) return;
            ResponseModel rm = InfoAction.DeleteMsg(m.Id);

            if (rm.Flag == 0)
            {
                AskUtil.ShowAlert("删除成功");
                _messagesss.Remove(m);
                RaisePropertyChanged(() => Messagesss);
            }
            else
            {
                AskUtil.ShowAlert(rm.Msg);
            }

        }

        private void GetMoney()
        {
            //ResponseModel rm = InfoAction.Withdraw(AskUtil.Teacher.Info., AskUtil.Login.Password);
            //if (rm.Flag == 0)
            //{
            //    Teacher = rm.ResModel<Teacher>();
            //    AskUtil.Teacher = Teacher;
            //}
            //else
            //{
            //    AskUtil.ShowAlert(rm.Msg);
            //}

        }

        private int GetTeachRecord(int start,int size)
        {
            int total = 0;
            ResponseModel rm = InfoAction.GetTeachRecord(Teacher.Id,"t", start,size);
            if (rm.Flag == 0)
            {
                RTeachRecords r = rm.ResModeList<RTeachRecords>();
                TeachRecords = r.Orders;
                total = r.Total;
            }
            else
            {
                AskUtil.ShowAlert(rm.Msg);
            }
            return total;
        }

        private int GetMoneyRecord(int start, int size)
        {
            int total = 0;
            ResponseModel rm = InfoAction.GetWithdraw(Teacher.Id, _start, size);
            if (rm.Flag == 0)
            {
                Withdrawss w = rm.ResModel<Withdrawss>();
                Withdraws = w.Withdraws;
                total = w.Total;
            }else
            {
                AskUtil.ShowAlert(rm.Msg);
            }
            return total;
        }

        private int GetMessageRecord(int start, int size)
        {
            int total = 0;
            ResponseModel rm = InfoAction.GetMsg(Teacher.Id, _start, size);
            if (rm.Flag == 0)
            {
                Messagess m = rm.ResModel<Messagess>();
                Messagesss = m.Messages;
                total = m.Total;
            }
            else
            {
                AskUtil.ShowAlert(rm.Msg);
            }
            return total;
        }

        public Teacher Teacher
        {
            get
            {
                return _teacher;
            }

            set
            {
                _teacher = value;
                RaisePropertyChanged(() => Teacher);
            }
        }

        public ObservableCollection<TeachRecord> TeachRecords
        {
            get
            {
                return _teachRecords;
            }

            set
            {
                _teachRecords = value;
                RaisePropertyChanged(() => TeachRecords);
            }
        }

        public ObservableCollection<Message> Messagesss
        {
            get
            {
                return _messagesss;
            }

            set
            {
                _messagesss = value;
                RaisePropertyChanged(() => Messagesss);
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
                RaisePropertyChanged(() => Withdraws);
            }
        }

        public double Star
        {
            get
            {
                return _star;
            }

            set
            {
                _star = value;
                RaisePropertyChanged(() => Star);
            }
        }

        public double Exprience
        {
            get
            {
                return _exprience;
            }

            set
            {
                _exprience = value;
                RaisePropertyChanged(() => Exprience);
            }
        }

        /// <summary>  
        /// 分页控件回调函数，返回总记录数  
        /// </summary>  
        /// <param name="pageNo">页码，由分页控件传入</param>  
        /// <param name="pageSize">页面记录大小，由分页控件传入</param>  
        /// <returns></returns>  
        public int LoadTeachRecordData(int pageNo, int pageSize)
        {
            return GetTeachRecord((pageNo-1) *  pageSize, pageSize);
        }

        public int LoadMoneyRecordData(int pageNo, int pageSize)
        {
            return GetMoneyRecord((pageNo - 1) * pageSize, pageSize);
        }

        public int LoadMessageRecordData(int pageNo, int pageSize)
        {
            return GetMessageRecord((pageNo - 1) * pageSize, pageSize);
        }
        
    }
}
