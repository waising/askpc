using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using WPFClient.Util;

namespace WPFClient
{

    /// <summary>
    /// 屏幕录制
    /// 需要下载  Screen Capturer Recorder
    /// https://nchc.dl.sourceforge.net/project/screencapturer/Setup%20Screen%20Capturer%20Recorder%20v0.12.8.exe
    /// </summary>
    public class ScreenHelper
    {

        static log4net.ILog log = log4net.LogManager.GetLogger(typeof(ScreenHelper));

        //ffmpeg
        public static void killProcess(string name)
        {
            Process[] KillProcessArray = Process.GetProcessesByName(name);

            Debug.WriteLine(KillProcessArray.Length.ToString());

            foreach (var KillProcess in KillProcessArray)
            {
                KillProcess.Kill();
            }
        }

        /// <summary>
        /// 开始录屏
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="x">起始坐标x</param>
        /// <param name="y">起始坐标y</param>
        /// <param name="screenWidth">截取范围</param>
        /// <param name="screenHeight">截取范围</param>
        /// <param name="output">错误代理</param>
        /// <returns></returns>
        public static Process runFfmpegScreen(string formTitle,int x,int y,int screenWidth,int screenHeight,string outPath ,DataReceivedEventHandler output)
        {
            killProcess("ffmpeg");
            Process p = new Process();

            //判断是否安装了
            if (AskUtil.CheckSoftWartInstallState("Screen Capturer Recorder"))
            {

                try
                {
                    log.Info("开始录制");
                    string ffds = @"ffmpeg\bin\ffmpeg.exe";
                
                    p.StartInfo.FileName = Directory.GetCurrentDirectory()+"\\" + ffds;   //ffmpeg.exe的绝对路径

                    string cursize = Convert.ToString(screenWidth) + "x" + Convert.ToString(screenHeight); // "150x200";
                    //string mvSize = "640x480";
                    //string path = "  C:\\Users\\Public\\outputfile1x1112.mkv"; 麦克风 (Realtek High Definition Audio)

                    //TODO 更换语音识别模块
                    //平板
                    string audio = "Microphone (Intel SST Audio Device (WDM))";
                    //本机
                    //audio = "麦克风 (Realtek High Definition Audio)";
                    StringBuilder www = new StringBuilder();
                    www.Append(" -y -f gdigrab -framerate 15 -offset_x ").Append(x).Append(" -offset_y ").Append(y).Append(" -video_size ").Append(cursize).Append(" -i title=").Append(formTitle)
                        .Append(@" -f dshow -i audio=""virtual-audio-capturer"" -f dshow -i audio=""").Append(audio).Append(@""" -filter_complex ""[1:0][2:0]amix = inputs = 2:duration = shortest"" -vcodec libx264 -preset veryfast -pix_fmt yuv420p -s 640x480 -r 15 -acodec libmp3lame -ab 64k -ac 2 -ar 16000 ").Append(outPath);
                    p.StartInfo.Arguments = www.ToString();   //ffmpeg的参数

                    log.Info("录制参数："+www.ToString());
                    Console.WriteLine(www);
                    p.StartInfo.UseShellExecute = false;           //是否使用操作系统shell启动

                    p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息

                    p.StartInfo.RedirectStandardError = true;      //重定向标准错误输出

                    p.StartInfo.CreateNoWindow = true;             //不显示程序窗口
                    if(output!=null)
                        p.ErrorDataReceived += new DataReceivedEventHandler(output);
                    p.Start();
                    //获取关联进程的唯一标识符
                    p.BeginErrorReadLine();
                }
                catch
                {

                }

            }
            //int pId = p == null ? -1 : p.Id;

            return p;
        }

        //private void Output(object sendProcess, DataReceivedEventArgs output)

        //{

        //    //if (!String.IsNullOrEmpty(output.Data))

        //    //    Debug.WriteLine(output.Data.ToString());

        //}

        /// <summary>
        /// 停止录屏
        /// 
        /// </summary>
        /// <param name="pId"></param>
        public static int stopScreen(Process p, DataReceivedEventHandler output)
        {

            int r = -1;
            try
            {

                if (p != null)
                {
                    log.Info("结束录制:" + p.Id);
                    log.Info("Process:q");
                    p.StartInfo.Arguments = "q";   //ffmpeg的参数 退出录制

                    p.StartInfo.UseShellExecute = false;           //是否使用操作系统shell启动

                    p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息

                    p.StartInfo.RedirectStandardError = true;      //重定向标准错误输出

                    p.StartInfo.CreateNoWindow = true;             //不显示程序窗口
                    if (output != null)
                        p.ErrorDataReceived += new DataReceivedEventHandler(output);

                    p.Start();
                    //向CMD窗口发送输入信息： 
                    //p.StandardInput.WriteLine("q");
                    //p.BeginErrorReadLine();
                    //Console.WriteLine("sleep");
                    //System.Threading.Thread.Sleep(10000);
                    //Console.WriteLine("kill");
                    //p.Kill();
                    //p.StandardOutput.ReadToEnd();
                    log.Info("结束录制:Kill"+ p.StandardOutput.ReadToEnd());
                }

                r = 1;
            }
            catch (Exception e)
            {
                log.Info("ffmpeg kill err:"+e.Message);
            }

            return r;
        }
    }
}
