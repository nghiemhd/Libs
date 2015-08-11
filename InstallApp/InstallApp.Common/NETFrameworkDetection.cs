using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallApp.Common
{
    public class NETFrameworkDetection
    {
        public static List<string> GetVersionsFromRegistry()
        {
            var result = new List<string>();

            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey = RegistryKey
                .OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                        {
                            result.Add(versionKeyName + "  " + name);
                        }
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                result.Add(versionKeyName + "  " + name + "  SP" + sp);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                            {
                                sp = subKey.GetValue("SP", "").ToString();
                            }
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                            {
                                result.Add(versionKeyName + "  " + name);
                            }
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    result.Add("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    result.Add("  " + subKeyName + "  " + name);
                                }

                            }

                        }

                    }
                }
            }

            return result;
        }

        public static string Get45orLaterFromRegistry()
        {
            string result = string.Empty;

            using (RegistryKey ndpKey = RegistryKey
                .OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                .OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (true)
                {
                    result = CheckFor45DotVersion(releaseKey);
                }
            }
            return result;
        }

        private static string CheckFor45DotVersion(int releaseKey)
        {
            if (releaseKey >= 393273)
            {
                return "4.6 RC or later";
            }
            if (releaseKey >= 379893)
            {
                return "4.5.2 or later";
            }
            if (releaseKey >= 378675)
            {
                return "4.5.1 or later";
            }
            if (releaseKey >= 378389)
            {
                return "4.5 or later";
            }
            return string.Empty;
        }
    }
}
