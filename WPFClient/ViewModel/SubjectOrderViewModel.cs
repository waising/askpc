using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient.ViewModel
{
    public class SubjectOrderViewModel : ViewModelBase
    {
        private static SubjectOrderViewModel _subjectOrderViewModel = null;

        private SubjectOrder _subjectOrder;

        private int start = 0;
        private int limit = 3;

        public static SubjectOrderViewModel GetSubjectOrderViewModelInstance()
        {
            if (_subjectOrderViewModel == null)
            {
                _subjectOrderViewModel = new SubjectOrderViewModel();
            }
            return _subjectOrderViewModel;
        }

        public SubjectOrderViewModel()
        {
            if (_subjectOrder == null)
                _subjectOrder = new SubjectOrder();
            _subjectOrder = (SubjectOrder)Application.Current.Properties["OrderDetail"];

            SubjectURL = HttpHelper.HTTP_SUBJECT_HTML_URL + AskUtil.GetParams(new object[] { "id=" + _subjectOrder.Id, "start=" + start, "limit=" + limit });
            //Console.WriteLine(_subjectOrder.Student.Name);
            Console.WriteLine(SubjectURL);

            TeacherGet = new Random().Next(5,21).ToString();

            TeacherGet = "有" + TeacherGet;

            Messenger.Default.Register<SubjectOrder>(this, "ShowOrderDetail", (order) =>
            {
                SubjectOrder = order;
            });
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

        private string _teacherGet;

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

        public string TeacherGet
        {
            get
            {
                return _teacherGet;
            }

            set
            {
                _teacherGet = value;
                RaisePropertyChanged(() => TeacherGet);
            }
        }

        private string _subjectURL;

    }
}
