using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace InstallApp.Common
{
    public static class Helper
    {
        public static void UpdateWebconfig(string filePath, IDictionary<string, string> appSettings)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var xDoc = XDocument.Load(filePath);
            var settings = xDoc.Descendants("appSettings").Elements("add");
            foreach (var item in appSettings)
            {
                var setting = settings.Where(x => x.Attribute("key").Value == item.Key).FirstOrDefault();
                if (setting != null)
                {
                    setting.Attribute("value").Value = item.Value;
                }
                else 
                {
                    var newSetting = new XElement("add",
                        new XAttribute("key", item.Key),
                        new XAttribute("value", item.Value));

                    xDoc.Descendants("appSettings").FirstOrDefault().Add(newSetting);                    
                }
            }

            xDoc.Save(filePath);
        }

        public static void Copy(string sourcePath, string targetPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
            DirectoryInfo targetDirectory = new DirectoryInfo(targetPath);

            CopyAll(sourceDirectory, targetDirectory);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }

            foreach (DirectoryInfo subDirectory in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(subDirectory.Name);
                CopyAll(subDirectory, nextTargetSubDir);
            }
        }

        public static void UpdateFolderPermission(string accountName, string folderPath, FileSystemRights Rights, AccessControlType controlType)
        {

        }

    }
}
