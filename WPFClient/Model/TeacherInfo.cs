using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class TeacherInfo : ObservableObject
    {
        /// <summary>
        /// 教师名
        /// </summary>
        private string _name;
        /// <summary>
        /// 教师姓
        /// </summary>
        private string _familyName;

        /// <summary>
        /// 教师全名
        /// </summary>
        private string _fullName;
        /// <summary>
        /// 教师所在学校
        /// </summary>
        private string _school;
        /// <summary>
        /// Email
        /// </summary>
        private string _email;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string FamilyName
        {
            get
            {
                return _familyName;
            }

            set
            {
                _familyName = value;
                RaisePropertyChanged(() => FamilyName);
            }
        }

        public string School
        {
            get
            {
                return _school;
            }

            set
            {
                _school = value;
                RaisePropertyChanged(() => School);
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public string FullName
        {
            get
            {
                return FamilyName+Name;
            }

            set
            {
                _fullName = value;
                RaisePropertyChanged(() => School);
            }
        }
    }
}
