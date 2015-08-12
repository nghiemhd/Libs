using InstallApp.Logging;
using InstallApp.Synchronization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InstallApp.ConsoleSync
{
    class Program
    {
        static ILogger logger;
        static void Main(string[] args)
        {
            string sourcePath = ConfigurationManager.AppSettings["source"];
            string targetPath = ConfigurationManager.AppSettings["target"];
            string intervalValue = ConfigurationManager.AppSettings["interval"];
            int interval;

            int.TryParse(intervalValue, out interval);
            if (interval == 0)
            {
                interval = 20;
            }
            log4net.Config.XmlConfigurator.Configure();
            logger = new Logger("SyncService");
            var syncManager = new SyncManager(sourcePath, targetPath, SyncOptions.Both, logger);

            while (true)
            {
                syncManager.Start();
                Thread.Sleep(interval * 1000);
            }
        }

        
    }
}
