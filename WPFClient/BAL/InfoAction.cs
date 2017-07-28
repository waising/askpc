using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClient.Common;
using WPFClient.Util;

namespace WPFClient.BAL
{
    public class InfoAction
    {
        /// <summary>
        /// 辅导记录
        /// </summary>
        /// <param name="account">教师帐号</param>
        /// <param name="role">t：默认老师</param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ResponseModel GetTeachRecord(string account, string role, int start,int limit)
        {
            string param = AskUtil.GetParams(new object[] { "start=" + start, "limit=" + limit, "account=" + account, "role=" + role });
            string resstr = HttpHelper.SendGet("order/history?" + param);

            return AskUtil.JsonConvertRM(resstr); ;
        }

        /// <summary>
        /// 消息记录
        /// </summary>
        /// <param name="id">老师账号</param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ResponseModel GetMsg(string id, int start, int limit)
        {
            string param = AskUtil.GetParams(new object[] { "start=" + start, "limit=" + limit, "id=" + id });
            string resstr = HttpHelper.SendGet("teacher/msg?" + param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="id">消息id</param>
        /// <returns></returns>
        public static ResponseModel DeleteMsg(string id)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id });
            string resstr = HttpHelper.SendDelete("teacher/msg?" + param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <param name="id">教师帐号</param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ResponseModel GetWithdraw(string id, int start, int limit)
        {
            string param = AskUtil.GetParams(new object[] { "start=" + start, "limit=" + limit, "id=" + id });
            string resstr = HttpHelper.SendGet("teacher/withdraw?" + param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 提现
        /// 教师手机号 银行卡号  银行公司 金额
        /// </summary>
        /// <returns></returns>
        public static ResponseModel Withdraw(string tel, string account, string company,int money)
        {
            string param = AskUtil.GetParams(new object[] { "tel=" + tel, "account=" + account, "company=" + company, "askcoin=" + money });
            string resstr = HttpHelper.SendPost("teacher/withdraw", param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 教师信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ResponseModel GetTeacher(string id, string password)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id, "password=" + password });
            string resstr = HttpHelper.SendGet("teacher/info?" + param);

            return AskUtil.JsonConvertRM(resstr);
        }
    }
}
