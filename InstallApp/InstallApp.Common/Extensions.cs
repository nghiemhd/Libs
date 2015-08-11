using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallApp.Common
{
    public static class Extensions
    {
        public static void Empty(this DirectoryInfo directoryInfo)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                subDirectory.Delete(true);
            }

            
        }
        
    }
}
