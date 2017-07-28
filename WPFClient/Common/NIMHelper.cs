using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NIM.ClientAPI;

namespace WPFClient.Common
{
    public class NIMHelper
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static bool InitNIM()
        {
            var config = NimUtility.ConfigReader.GetServerConfig();
            return NIM.ClientAPI.Init("91ASKING", null, config);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="loginResultDelegate"></param>
        /// <returns></returns>
        public static string LoginNIM(string id, string password, LoginResultDelegate loginResultDelegate)
        {
            //;NIM.ToolsAPI.GetMd5(_password);
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password))
            {
                string appkey = NimUtility.ConfigReader.GetAppKey();
                if (string.IsNullOrEmpty(appkey))
                {
                    return "授权KEY配置有误！";
                }
                NIM.ClientAPI.Login(appkey, id, password, loginResultDelegate);
            }

            return "";
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="logoutType"></param>
        /// <param name="logoutResultDelegate"></param>
        public static void LogoutNIM(NIM.NIMLogoutType logoutType, LogoutResultDelegate logoutResultDelegate)
        {
            NIM.ClientAPI.Logout(logoutType, logoutResultDelegate);
        }
          
    }
}
