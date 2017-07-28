using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(App));
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherHelper.Initialize();
            RegisterEvents();
            base.OnStartup(e);

            //SetNewtonsoft();
            log4net.Config.XmlConfigurator.Configure();
            log.Info("<<<=======启动阿思可教师端=====");

        }

        protected override void OnExit(ExitEventArgs e)
        {
            log.Info("<<<=======退出阿思可教师端=====");
            base.OnExit(e);
        }

        private void RegisterEvents()
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                MessageBox.Show(args.Exception.Message);
                args.SetObserved();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                SMessageBox.Show("程序异常");
                log.ErrorFormat("程序异常:{0}",args.ExceptionObject.ToString());
                log.ErrorFormat("程序异常:{0}", args.ToString());
            };
        }
    }
}
