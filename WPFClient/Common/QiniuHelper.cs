using qiniu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFClient.Common
{
    public class QiniuHelper
    {

        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(QiniuHelper));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localfile">本地文件</param>
        /// <param name="qiniukey">上传qiniu后的文件名</param>
        public static int updateQiniu(string localfile, string qiniukey)
        {
            int r = 0;
            // 初始化qiniu配置，主要是API Keys
            qiniu.Config.ACCESS_KEY = "hDTxHB2h9KTZT2vUviZ0nxZJoLyLfmXk5NJKkQ0N";
            qiniu.Config.SECRET_KEY = "mvWkaKZGOCfSuBmLGsFnTb1ZLX8Y548qsNpPYZcK";

            /**********************************************************************
			可以用下面的方法从配置文件中初始化
			qiniu.Config.InitFromAppConfig ();
			**********************************************************************/

            string bucket = "zhibird";
            //string qiniukey = "tzd.rmvb";

            //======================================================================
            {
                QiniuFile qfile = new QiniuFile(bucket, qiniukey, localfile);

                ResumbleUploadEx puttedCtx = new ResumbleUploadEx(localfile); //续传

                //ManualResetEvent done = new ManualResetEvent(false);
                qfile.UploadCompleted += (sender, e) => {
                    Console.WriteLine(e.key);
                    Console.WriteLine(e.Hash);
                    //done.Set();
                    log.Info("上传完成");
                    r = 1;
                };
                qfile.UploadFailed += (sender, e) => {
                    Console.WriteLine(e.Error.ToString());
                    puttedCtx.Save();
                    //done.Set();
                    log.Info("上传失败");
                    r = 2;
                };
                qfile.UploadProgressChanged += (sender, e) => {
                    int percentage = (int)(100 * e.BytesSent / e.TotalBytes);
                    Console.Write(percentage);

                };
                qfile.UploadBlockCompleted += (sender, e) => {
                    //上传结果持久化
                    puttedCtx.Add(e.Index, e.Ctx);
                    puttedCtx.Save();
                };
                qfile.UploadBlockFailed += (sender, e) => {
                    //
                };

                //上传为异步操作
                //上传本地文件到七牛云存储
                qfile.Upload();

                //如果要续传，调用下面的方法
                //qfile.Upload (puttedCtx.PuttedCtx);

                //done.WaitOne();
            }

            return r;
        }
    }
}
