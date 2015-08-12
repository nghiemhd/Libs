using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallApp.Common
{
    public class IISConfiguration
    {
        private static string[] featureNames = new[] 
        {
            "IIS-ApplicationDevelopment",
            "IIS-ASPNET",
            "IIS-ASPNET45",
            "IIS-CommonHttpFeatures",
            "IIS-DefaultDocument",
            "IIS-ISAPIExtensions",
            "IIS-ISAPIFilter",
            "IIS-ManagementConsole",
            "IIS-NetFxExtensibility /all",
            "IIS-NetFxExtensibility45 /all",
            "IIS-RequestFiltering",
            "IIS-Security",
            "IIS-StaticContent",
            "IIS-WebServer",
            "IIS-WebServerRole",
        };

        public static IISstate ValidateIIS(out string outputString)
        {
            var result = IISstate.Disabled;

            string arguments = string.Format("/Online /Get-FeatureInfo /FeatureName:IIS-WebServer");
            outputString = Run("dism", arguments);

            var messages = outputString.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var state = messages.Where(x => x.IndexOf("State :", 0) >= 0).FirstOrDefault();

            if (state != null)
            {
                string value = state.Split(':')[1].Trim();
                Enum.TryParse<IISstate>(value, true, out result);
            }
            return result;
        }
        
        public static string SetupIIS()
        {
            var arguments = string.Format(
                "/NoRestart /Online /Enable-Feature {0}",
                string.Join(
                    " ",
                    featureNames.Select(name => string.Format("/FeatureName:{0}", name))));

            return Run("dism", arguments);
        }

        private static string Run(string fileName, string arguments)
        {
            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }))
            {
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
            
        } 
    }
}
