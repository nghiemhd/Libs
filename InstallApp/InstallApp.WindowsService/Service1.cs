using InstallApp.Logging;
using InstallApp.Synchronization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstallApp.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        string sourcePath = ConfigurationManager.AppSettings["source"];
        string targetPath = ConfigurationManager.AppSettings["target"];
        string intervalValue = ConfigurationManager.AppSettings["interval"];
        ILogger logger;
        int interval;

        public Service1()
        {
            InitializeComponent();

            this.ServiceName = "InstallAppSynchronizationService";
            this.eventLog = new EventLog();
            this.eventLog.Source = this.ServiceName;
            this.eventLog.Log = "Application";

            ((ISupportInitialize)(this.eventLog)).BeginInit();
            if (!EventLog.SourceExists(this.eventLog.Source))
            {
                EventLog.CreateEventSource(this.eventLog.Source, this.eventLog.Log);
            }
            ((ISupportInitialize)(this.eventLog)).EndInit();

            log4net.Config.XmlConfigurator.Configure();
            logger = new Logger("SyncService");

            int.TryParse(intervalValue, out interval);
            if (interval == 0)
            {
                interval = 20;
            }
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {
        }

        public void Process()
        {
            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = interval * 1000;
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            //timer.Start();

            var syncManager = new SyncManager(sourcePath, targetPath, SyncOptions.OneWay, logger);
            while (true)
            {
                syncManager.Start();
                Thread.Sleep(interval * 1000);
            }
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            var syncManager = new SyncManager(sourcePath, targetPath, SyncOptions.OneWay, logger);
            syncManager.Start();
        }
    }
}
