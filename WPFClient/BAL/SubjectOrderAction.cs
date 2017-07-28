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
    public class SubjectOrderAction
    {
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="grade"></param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ResponseModel GetSubjectOrder(string subject,string grade, int start, int limit)
        {
            string param = AskUtil.GetParams(new object[] { "start=" + start, "limit=" + limit, "subject="+ subject, "grade="+ grade });
            string resstr = HttpHelper.SendGet("order?"+ param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 
        /// 获取10.搜题展示html
        /// </summary>
        /// <param name="stage">初中为2、高中为3；由登陆后给的教师信息Code判断下划线后两位小于10的为初中，即该值为2 下划线后两位大于等于10的为高中，即该值为3</param>
        /// <param name="code">M为数学，P为物理，C为化学</param>
        /// <param name="s">搜题文本</param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ResponseModel GetSubjectHtml(string stage, string code,string s, int start, int limit)
        {
            string param = AskUtil.GetParams(new object[] { "start=" + start, "limit=" + limit, "stage=" + stage, "code=" + code,"s="+ s });
            string resstr = HttpHelper.SendGet("order?" + param);

            return AskUtil.JsonConvertRM(resstr);
        }



        /// <summary>
        /// 接通
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static ResponseModel Connect(string id)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id });
            string resstr = HttpHelper.SendPost("order/connect", param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="account">老师账号</param>
        /// <param name="name">老师姓名</param>
        /// <param name="code">教师编号</param>
        /// <returns></returns>
        public static ResponseModel CommitOrder(string id, string account, string name, string code)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id, "account=" + account, "name=" + name, "code=" + code});
            string resstr = HttpHelper.SendPost("order/catch" , param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 开始教学
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static ResponseModel StartTeach(string id)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id });
            string resstr = HttpHelper.SendPost("order/start", param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 结账
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static ResponseModel FinishOrder(string id)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id});
            string resstr = HttpHelper.SendPost("order/checkbill", param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 放弃订单  不回到订单列表
        /// </summary>
        /// <param name="id">订单id</param>
        /// <returns></returns>
        public static ResponseModel GiveUpOrder(string id)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id });
            string resstr = HttpHelper.SendPost("order/giveup", param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 告诉学生的话
        /// </summary>
        /// <param name="id">订单id</param>
        /// matched 0：不匹配 1：匹配  -1 不参与
        /// <returns></returns>
        public static ResponseModel ToStudentMsg(string id,string say,int matched)
        {
            string param = AskUtil.GetParams(new object[] { "id=" + id, "toStudent=" + say , "matched="+ matched });
            string resstr = HttpHelper.SendPost("order/tostudent", param);

            return AskUtil.JsonConvertRM(resstr);
        }

        /// <summary>
        /// 告诉学生的话
        /// </summary>
        /// <param name="id">订单id</param>
        /// matched 0：不匹配 1：匹配  -1 不参与
        /// <returns></returns>
        public static ResponseModel SaveVideoPath(string orderId, string videos)
        {
            string param = AskUtil.GetParams(new object[] { "videos=" + videos});
            string resstr = HttpHelper.SendPost("order/videos/" + orderId, param);

            return AskUtil.JsonConvertRM(resstr);
        }
    }
}
