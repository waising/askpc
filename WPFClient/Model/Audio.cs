using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Audio : ObservableObject
    {
        private AudioTimeMsg _AudioTimeMsg;

        public AudioTimeMsg AudioTimeMsg
        {
            get
            {
                return _AudioTimeMsg;
            }

            set
            {
                _AudioTimeMsg = value;
            }
        }
    }

    public class AudioTimeMsg : ObservableObject
    {
        private string _channelId;
        private long _createtime;
        private int _duration;
        private int _eventTyoe;
        private int _live;
        private string _members;
        private string _status;
        private string _type;

        public string ChannelId
        {
            get
            {
                return _channelId;
            }

            set
            {
                _channelId = value;
            }
        }

        public long Createtime
        {
            get
            {
                return _createtime;
            }

            set
            {
                _createtime = value;
            }
        }

        public int Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                _duration = value;
            }
        }

        public int EventTyoe
        {
            get
            {
                return _eventTyoe;
            }

            set
            {
                _eventTyoe = value;
            }
        }

        public int Live
        {
            get
            {
                return _live;
            }

            set
            {
                _live = value;
            }
        }

        public string Members
        {
            get
            {
                return _members;
            }

            set
            {
                _members = value;
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }
    }
}
