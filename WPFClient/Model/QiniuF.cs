using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class QiniuF
    {
        private string orderId;
        private string localFile;
        private string bucket;
        private string qiniuFile;
        private int pid;

        public string LocalFile
        {
            get
            {
                return localFile;
            }

            set
            {
                localFile = value;
            }
        }

        public string Bucket
        {
            get
            {
                return bucket;
            }

            set
            {
                bucket = value;
            }
        }

        public string QiniuFile
        {
            get
            {
                return qiniuFile;
            }

            set
            {
                qiniuFile = value;
            }
        }

        public string OrderId
        {
            get
            {
                return orderId;
            }

            set
            {
                orderId = value;
            }
        }

        public int Pid
        {
            get
            {
                return pid;
            }

            set
            {
                pid = value;
            }
        }
    }
}
