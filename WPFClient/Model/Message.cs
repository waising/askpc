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
    /// <summary>
    /// 教师消息记录对象
    /// </summary>
    public class Message : ObservableObject
    {
        private string _id;
        /// <summary>
        /// 教师帐号
        /// </summary>
        private string _tel;

        [JsonProperty("message")]
        private string _msg;
        /// <summary>
        /// 创建时间
        /// </summary>
        private long _date;
        /// <summary>
        /// 1在教师端显示，0时不显示
        /// </summary>
        private int _state;

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

        public string Tel
        {
            get
            {
                return _tel;
            }

            set
            {
                _tel = value;
            }
        }

        public string Msg
        {
            get
            {
                return _msg;
            }

            set
            {
                _msg = value;
            }
        }

        public long Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
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
    }

    public class Messagess : ObservableObject
    {
        private int _total;
        [JsonProperty("msgs")]
        private ObservableCollection<Message> _messages;

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

        public ObservableCollection<Message> Messages
        {
            get
            {
                return _messages;
            }

            set
            {
                _messages = value;
            }
        }
    }
}
