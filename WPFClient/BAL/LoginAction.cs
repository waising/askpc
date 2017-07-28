using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFClient.Common;
using WPFClient.Model;
using WPFClient.Util;

namespace WPFClient
{
    public class LoginAction
    {
        public static ResponseModel Login(string id,string password)
        {
            string param = AskUtil.GetParams(new object[] {"id="+id,"password="+password });
            string resstr = HttpHelper.SendPost("teacher/login",param);

            return AskUtil.JsonConvertRM(resstr);
        }

        public static NIMToken GetNIMToken(string id)
        {
            string param = AskUtil.GetParams(new object[] { "accid=" + id });
            string resstr = HttpHelper.SendFullUrlGet(HttpHelper.HTTP_API_URL+"cloudapi/nim/token?" + param);

            try
            {
                Console.WriteLine(resstr);
                return JsonConvert.DeserializeObject<NIMToken>(resstr);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

    }
}
