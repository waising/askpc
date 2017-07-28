using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClient.Model
{
    public class TeachRecord : ObservableObject
    {
        private string _id;
        private int _state;

        private Subject _subject;
        private Bill _bill;

        private TeacherRecord _teacher;

        private Evaluation _evaluation;
        private Student _student;
        [JsonProperty("time")]
        private AskTime _askTime;
        private Audio _audio;
        private Whiteboard _whiteboard;

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

        public TeacherRecord Teacher
        {
            get
            {
                return _teacher;
            }

            set
            {
                _teacher = value;
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

        public Audio Audio
        {
            get
            {
                return _audio;
            }

            set
            {
                _audio = value;
            }
        }

        public Whiteboard Whiteboard
        {
            get
            {
                return _whiteboard;
            }

            set
            {
                _whiteboard = value;
            }
        }

        public Evaluation Evaluation
        {
            get
            {
                return _evaluation;
            }

            set
            {
                _evaluation = value;
            }
        }

    }

    public class RTeachRecords : ObservableObject
    {
        private int _total;
        private ObservableCollection<TeachRecord> _orders;

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

        public ObservableCollection<TeachRecord> Orders
        {
            get
            {
                return _orders;
            }

            set
            {
                _orders = value;
            }
        }
    }

    public class TeacherRecord : ObservableObject
    {
        private string _account;
        private string _name;
        private string _avatar;
        private string _code;
        private string _toStudent;

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

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public string ToStudent
        {
            get
            {
                return _toStudent;
            }

            set
            {
                _toStudent = value;
            }
        }
    }
}
