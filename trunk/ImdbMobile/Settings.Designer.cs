namespace ImdbMobile
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlLanguage = new System.Windows.Forms.ComboBox();
            this.chkThumbnails = new System.Windows.Forms.CheckBox();
            this.lblVideoPlayer = new System.Windows.Forms.Label();
            this.ddlVideoPlayer = new System.Windows.Forms.ComboBox();
            this.lblCustomVideo = new System.Windows.Forms.Label();
            this.txtCustomVideo = new System.Windows.Forms.TextBox();
            this.txtCustomArgs = new System.Windows.Forms.TextBox();
            this.lblCustomArgs = new System.Windows.Forms.Label();
            this.txtCachePath = new System.Windows.Forms.TextBox();
            this.lblCachePath = new System.Windows.Forms.Label();
            this.chkEnableAnimate = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(8, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 40);
            this.label2.Text = "IMDb Language";
            // 
            // ddlLanguage
            // 
            this.ddlLanguage.Location = new System.Drawing.Point(210, 154);
            this.ddlLanguage.Name = "ddlLanguage";
            this.ddlLanguage.Size = new System.Drawing.Size(242, 41);
            this.ddlLanguage.TabIndex = 9;
            // 
            // chkThumbnails
            // 
            this.chkThumbnails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkThumbnails.Location = new System.Drawing.Point(3, 275);
            this.chkThumbnails.Name = "chkThumbnails";
            this.chkThumbnails.Size = new System.Drawing.Size(422, 40);
            this.chkThumbnails.TabIndex = 10;
            // 
            // lblVideoPlayer
            // 
            this.lblVideoPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVideoPlayer.Location = new System.Drawing.Point(8, 315);
            this.lblVideoPlayer.Name = "lblVideoPlayer";
            this.lblVideoPlayer.Size = new System.Drawing.Size(417, 40);
            // 
            // ddlVideoPlayer
            // 
            this.ddlVideoPlayer.Items.Add("Windows Media Player");
            this.ddlVideoPlayer.Items.Add("HTC StreamingMedia");
            this.ddlVideoPlayer.Items.Add("CorePlayer");
            this.ddlVideoPlayer.Location = new System.Drawing.Point(8, 358);
            this.ddlVideoPlayer.Name = "ddlVideoPlayer";
            this.ddlVideoPlayer.Size = new System.Drawing.Size(443, 41);
            this.ddlVideoPlayer.TabIndex = 32;
            this.ddlVideoPlayer.SelectedIndexChanged += new System.EventHandler(this.ddlVideoPlayer_SelectedIndexChanged);
            // 
            // lblCustomVideo
            // 
            this.lblCustomVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomVideo.Location = new System.Drawing.Point(8, 402);
            this.lblCustomVideo.Name = "lblCustomVideo";
            this.lblCustomVideo.Size = new System.Drawing.Size(417, 40);
            // 
            // txtCustomVideo
            // 
            this.txtCustomVideo.Location = new System.Drawing.Point(8, 445);
            this.txtCustomVideo.Name = "txtCustomVideo";
            this.txtCustomVideo.Size = new System.Drawing.Size(443, 41);
            this.txtCustomVideo.TabIndex = 35;
            // 
            // txtCustomArgs
            // 
            this.txtCustomArgs.Location = new System.Drawing.Point(8, 542);
            this.txtCustomArgs.Name = "txtCustomArgs";
            this.txtCustomArgs.Size = new System.Drawing.Size(443, 41);
            this.txtCustomArgs.TabIndex = 37;
            // 
            // lblCustomArgs
            // 
            this.lblCustomArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCustomArgs.Location = new System.Drawing.Point(8, 499);
            this.lblCustomArgs.Name = "lblCustomArgs";
            this.lblCustomArgs.Size = new System.Drawing.Size(417, 40);
            // 
            // txtCachePath
            // 
            this.txtCachePath.Location = new System.Drawing.Point(8, 641);
            this.txtCachePath.Name = "txtCachePath";
            this.txtCachePath.Size = new System.Drawing.Size(444, 41);
            this.txtCachePath.TabIndex = 40;
            // 
            // lblCachePath
            // 
            this.lblCachePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCachePath.Location = new System.Drawing.Point(8, 598);
            this.lblCachePath.Name = "lblCachePath";
            this.lblCachePath.Size = new System.Drawing.Size(417, 40);
            // 
            // chkEnableAnimate
            // 
            this.chkEnableAnimate.Location = new System.Drawing.Point(8, 698);
            this.chkEnableAnimate.Name = "chkEnableAnimate";
            this.chkEnableAnimate.Size = new System.Drawing.Size(443, 40);
            this.chkEnableAnimate.TabIndex = 46;
            this.chkEnableAnimate.Text = "Enable Animations (BETA)";
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(210, 202);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(242, 41);
            this.comboBox1.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(5, 203);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 40);
            this.label1.Text = "UI Language:";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ClientSize = new System.Drawing.Size(480, 536);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkEnableAnimate);
            this.Controls.Add(this.txtCachePath);
            this.Controls.Add(this.lblCachePath);
            this.Controls.Add(this.txtCustomArgs);
            this.Controls.Add(this.lblCustomArgs);
            this.Controls.Add(this.txtCustomVideo);
            this.Controls.Add(this.lblCustomVideo);
            this.Controls.Add(this.ddlVideoPlayer);
            this.Controls.Add(this.lblVideoPlayer);
            this.Controls.Add(this.ddlLanguage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkThumbnails);
            this.Location = new System.Drawing.Point(0, 52);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Settings_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlLanguage;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.CheckBox chkThumbnails;
        private System.Windows.Forms.Label lblVideoPlayer;
        private System.Windows.Forms.ComboBox ddlVideoPlayer;
        private System.Windows.Forms.Label lblCustomVideo;
        private System.Windows.Forms.TextBox txtCustomVideo;
        private System.Windows.Forms.TextBox txtCustomArgs;
        private System.Windows.Forms.Label lblCustomArgs;
        private System.Windows.Forms.TextBox txtCachePath;
        private System.Windows.Forms.Label lblCachePath;
        private System.Windows.Forms.CheckBox chkEnableAnimate;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
    }
}