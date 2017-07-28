using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class RequestInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public DataInfo info { get; set; }
    }

    public class DataInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string accid { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string name { get; set; }

    }
}
