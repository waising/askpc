using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WPFClient.Common;

namespace WPFClient.Model
{
    public class AskTime : ObservableObject
    {
        /// <summary>
        /// 上传时间
        /// </summary>
        private long _uploadTime;
        private DateTime _uTime;
        /// <summary>
        /// 教师与学生接通时间
        /// </summary>
        private long _connectTime;
        private DateTime _cTime;
        /// <summary>
        /// 教师开始教学时间
        /// </summary>
        private long _startTime;
        private DateTime _sTime;
        /// <summary>
        /// 教师结束教学时间
        /// </summary>
        private long _endTime;
        private DateTime _eTime;
        /// <summary>
        /// 教学持续时间（秒）
        /// </summary>
        private long _holdingSeconds;

        public long UploadTime
        {
            get
            {
                return _uploadTime;
            }

            set
            {
                _uploadTime = value;
            }
        }

        public long ConnectTime
        {
            get
            {
                return _connectTime;
            }

            set
            {
                _connectTime = value;
            }
        }

        public long StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                _startTime = value;
            }
        }

        public long EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                _endTime = value;
            }
        }

        public long HoldingSeconds
        {
            get
            {
                return _holdingSeconds;
            }

            set
            {
                _holdingSeconds = value;
            }
        }

        public DateTime UTime
        {
            get
            {
                return _uTime;
            }

            set
            {
                _uTime = value;
            }
        }

        public DateTime CTime
        {
            get
            {
                return _cTime;
            }

            set
            {
                _cTime = value;
            }
        }

        public DateTime STime
        {
            get
            {
                return _sTime;
            }

            set
            {
                _sTime = value;
            }
        }

        public DateTime ETime
        {
            get
            {
                return _eTime;
            }

            set
            {
                _eTime = value;
            }
        }
    }
}
