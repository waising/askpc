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
    public class SubjectOrder : ObservableObject
    {

        private string _id;
        /// <summary>
        /// 1、队列中
        /// 2、审题中
        /// 3、开始解答
        /// 4、正常结束（未评论）
        /// 5、已评论
        /// 
        /// 
        /// -1、学生放弃
        /// -5、学生端网络异常
        /// -6、教师端网络异常
        /// 
        /// -999、未知错误
        /// </summary>
        private int _state;
        private Subject _subject;
        private Bill _bill;
        private Student _student;

        [JsonProperty("time")]
        private AskTime _askTime;

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

        public Subject Subject
        {
            get
            {
                return _subject;
            }

            set
            {
                _subject = value;
            }
        }

        public Bill Bill
        {
            get
            {
                return _bill;
            }

            set
            {
                _bill = value;
            }
        }

        public Student Student
        {
            get
            {
                return _student;
            }

            set
            {
                _student = value;
            }
        }

        public AskTime AskTime
        {
            get
            {
                return _askTime;
            }

            set
            {
                _askTime = value;
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
            }
        }

        public class SubjectsOrders
        {
            private int _total;
            private ObservableCollection<SubjectOrder> _subjects;

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

            public ObservableCollection<SubjectOrder> Subjects
            {
                get
                {
                    return _subjects;
                }

                set
                {
                    _subjects = value;
                }
            }
        }
    }
}
