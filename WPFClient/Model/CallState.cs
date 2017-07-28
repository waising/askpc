using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    /// <summary>
    /// 审题 呼叫 连接成功 教学中 呼叫失败 jieshu
    /// </summary>
    public enum CallState
    {
        Analyze = 0,
        Calling = 1,
        WaitTeach = 2,
        Teach = 3,
        CallTimeOut = 4,
        CallFail = 5,
        CallFinish = 6,
    }
}
