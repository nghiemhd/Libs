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

        #region Events
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
                this.txtLogger.Text = string.Empty;
                var isValid = ValidateBeforeInstall();
                if (isValid)
                {
                    var dialogResult = MessageBox.Show("All files and folders in destination will be deleted. Are you sure?", "Install", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        UpdateLogger("...Clean " + this.txtDestination.Text);
                        CleanDestination();

                        UpdateLogger("...Update " + Path.Combine(this.txtSource.Text, Constants.Webconfig));
                        var canUpdateWebconfig = UpdateWebconfig();

                        UpdateLogger("...Publish to " + this.txtDestination.Text);
                        Helper.Copy(txtSource.Text, txtDestination.Text);

                        var account = "IIS_IUSRS";
                        var mediaPath = Path.Combine(this.txtDestination.Text, Constants.Media);
                        if (Directory.Exists(mediaPath))
                        {
                            Helper.UpdateFolderPermission(account, mediaPath, FileSystemRights.Modify, AccessControlType.Allow);
                            UpdateLogger(string.Format("...Add modify permissions for {0} for accessing {1}", account, mediaPath));
                        }

                        var appDataPath = Path.Combine(this.txtDestination.Text, Constants.AppData);
                        if (Directory.Exists(appDataPath))
                        {
                            Helper.UpdateFolderPermission(account, appDataPath, FileSystemRights.Modify, AccessControlType.Allow);
                            UpdateLogger(string.Format("...Add modify permissions for {0} for accessing {1}", account, appDataPath));
                        }

                        UpdateLogger("Install successfully.");
                        MessageBox.Show("Install successfully.", "Install");
                    }
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnTurnOnIIS_Click(object sender, EventArgs e)
        {
            this.txtLogger.Text = string.Empty;
            UpdateLogger("...turning on IIS (it will take some minutes)");
            try
            {
                var message = IISConfiguration.SetupIIS();
                UpdateLogger(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        } 
        #endregion Events

        private bool ValidateBeforeInstall()
        {
            var errors = new List<string>();
            var errorMessage = string.Empty;

            string iisMessage = string.Empty;
            var iisState = IISConfiguration.ValidateIIS(out iisMessage);
            if (iisState == IISstate.Disabled)
            {
                errors.Add(iisMessage);                
            }
            UpdateLogger(iisMessage);

            var netFramework = NETFrameworkDetection.Get45orLaterFromRegistry();
            var isNet45Installed = netFramework == string.Empty ? false : true;
            if (!isNet45Installed)
            {
                errorMessage = ".NET Framework 4.5 hasn't been installed yet.";
                errors.Add(errorMessage);
                UpdateLogger("ERROR - " + errorMessage);
            }
            else
            {
                UpdateLogger(netFramework);
            }

            if (!Directory.Exists(this.txtSource.Text))
            {
                errorMessage = "Source folder not found.";
                errors.Add(errorMessage);
                UpdateLogger("ERROR - " + errorMessage);
            }

            if (!Directory.Exists(this.txtDestination.Text))
            {
                errorMessage = "Destination root not found.";
                errors.Add(errorMessage);
                UpdateLogger("ERROR - " + errorMessage);
            }

            if (errors.Count > 0)
            {
                //MessageBox.Show(string.Join(Environment.NewLine, errors), "Errors");
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

        private void UpdateLogger(string message)
        {
            this.txtLogger.Text += message + Environment.NewLine;
        }
    }
}
