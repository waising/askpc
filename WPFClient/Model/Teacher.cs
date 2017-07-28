using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Teacher : ObservableObject
    {
        private string _id;

        /// <summary>
        /// Online State
        /// </summary>
        private int _olState;
        /// <summary>
        /// Account State
        /// </summary>
        private int _acState;

        private TeacherInfo _info;

        private PayInfo _payInfo;

        private TeacherAsk _askInfo;

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

        public int OlState
        {
            get
            {
                return _olState;
            }

            set
            {
                _olState = value;
            }
        }

        public int AcState
        {
            get
            {
                return _acState;
            }

            set
            {
                _acState = value;
            }
        }

        public TeacherInfo Info
        {
            get
            {
                return _info;
            }

            set
            {
                _info = value;
            }
        }

        public PayInfo PayInfo
        {
            get
            {
                return _payInfo;
            }

            set
            {
                _payInfo = value;
            }
        }

        public TeacherAsk AskInfo
        {
            get
            {
                return _askInfo;
            }

            set
            {
                _askInfo = value;
            }
        }
    }
}
