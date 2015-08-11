using InstallApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstallApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSource.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.SelectedPath = this.txtDestination.Text;
            DialogResult result = this.folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtDestination.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            try
            {
                var isValid = ValidateBeforeInstall();
                if (isValid)
                {
                    var dialogResult = MessageBox.Show("All files and folders in destination will be deleted. Are you sure?", "Install", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        CleanDestination();
                        var canUpdateWebconfig = UpdateWebconfig();

                        Helper.Copy(txtSource.Text, txtDestination.Text);

                        var mediaPath = Path.Combine(this.txtDestination.Text, "Media");
                        Helper.UpdateFolderPermission("IIS_IUSRS", mediaPath, FileSystemRights.Modify, AccessControlType.Allow);

                        MessageBox.Show("Install successfully.", "Install");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateBeforeInstall()
        {
            var errors = new List<string>();
            string iisMessage = string.Empty;

            var iisState = IISConfiguration.ValidateIIS(out iisMessage);
            if (iisState == IISstate.Disabled)
            {
                errors.Add(iisMessage);
            }

            var isNet45Installed = NETFrameworkDetection.Get45orLaterFromRegistry() == string.Empty ? false : true;
            if (!isNet45Installed)
            {
                errors.Add(".NET Framework 4.5 hasn't been installed yet.");
            }

            if (!Directory.Exists(this.txtSource.Text))
            {
                errors.Add("Source folder not found.");
            }

            if (!Directory.Exists(this.txtDestination.Text))
            {
                errors.Add("Destination root not found.");
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors), "Errors");
                return false;
            }

            return true;
        }

        private bool UpdateWebconfig()
        { 
            var webconfigPath = Path.Combine(this.txtSource.Text, Constants.Webconfig);
            if (File.Exists(webconfigPath))
            {
                var appSettings = new Dictionary<string, string>
                {
                    { Constants.VDUId, this.txtVDUId.Text },
                    { Constants.ApiEndpoint, this.txtApiEndpoint.Text }
                };
                Helper.UpdateWebconfig(webconfigPath, appSettings);
            }
            else
            { 
                var message = "Cannot find web.config file in the source folder.";
                MessageBox.Show(message);
                return false;
            }

            return true;
        }

        private void CleanDestination()
        {
            var directoryInfo = new DirectoryInfo(this.txtDestination.Text);
            directoryInfo.Empty();
        }        
    }
}
