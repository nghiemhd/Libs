namespace InstallApp
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtVDUId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtApiEndpoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.txtLogger = new System.Windows.Forms.TextBox();
            this.btnTurnOnIIS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "VDUId";
            // 
            // txtVDUId
            // 
            this.txtVDUId.Location = new System.Drawing.Point(139, 22);
            this.txtVDUId.Name = "txtVDUId";
            this.txtVDUId.Size = new System.Drawing.Size(100, 22);
            this.txtVDUId.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "ApiEndpoint";
            // 
            // txtApiEndpoint
            // 
            this.txtApiEndpoint.Location = new System.Drawing.Point(139, 63);
            this.txtApiEndpoint.Name = "txtApiEndpoint";
            this.txtApiEndpoint.Size = new System.Drawing.Size(100, 22);
            this.txtApiEndpoint.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Source Folder";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(139, 106);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(234, 22);
            this.txtSource.TabIndex = 5;
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Location = new System.Drawing.Point(379, 106);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSource.TabIndex = 6;
            this.btnBrowseSource.Text = "Browse";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Target Folder";
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(139, 149);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(234, 22);
            this.txtDestination.TabIndex = 8;
            this.txtDestination.Text = "C:\\inetpub\\wwwroot";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.Location = new System.Drawing.Point(379, 149);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDestination.TabIndex = 9;
            this.btnBrowseDestination.Text = "Browse";
            this.btnBrowseDestination.UseVisualStyleBackColor = true;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(32, 205);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 10;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // txtLogger
            // 
            this.txtLogger.Location = new System.Drawing.Point(32, 250);
            this.txtLogger.Multiline = true;
            this.txtLogger.Name = "txtLogger";
            this.txtLogger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogger.Size = new System.Drawing.Size(422, 379);
            this.txtLogger.TabIndex = 11;
            // 
            // btnTurnOnIIS
            // 
            this.btnTurnOnIIS.Location = new System.Drawing.Point(137, 205);
            this.btnTurnOnIIS.Name = "btnTurnOnIIS";
            this.btnTurnOnIIS.Size = new System.Drawing.Size(102, 23);
            this.btnTurnOnIIS.TabIndex = 12;
            this.btnTurnOnIIS.Text = "Turn on IIS";
            this.btnTurnOnIIS.UseVisualStyleBackColor = true;
            this.btnTurnOnIIS.Click += new System.EventHandler(this.btnTurnOnIIS_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 641);
            this.Controls.Add(this.btnTurnOnIIS);
            this.Controls.Add(this.txtLogger);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.btnBrowseDestination);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBrowseSource);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtApiEndpoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtVDUId);
            this.Controls.Add(this.label1);
            this.Name = "Main";
            this.Text = "InstallApp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVDUId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtApiEndpoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox txtLogger;
        private System.Windows.Forms.Button btnTurnOnIIS;
    }
}

