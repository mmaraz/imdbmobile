namespace ImdbMobile
{
    partial class Settings
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.lblImdbLocale = new System.Windows.Forms.Label();
            this.ddlImdbLocale = new System.Windows.Forms.ComboBox();
            this.ddlUILang = new System.Windows.Forms.ComboBox();
            this.lblUILanguage = new System.Windows.Forms.Label();
            this.ddlVideoPlayer = new System.Windows.Forms.ComboBox();
            this.lblVideoPlayer = new System.Windows.Forms.Label();
            this.lblCustomVideo = new System.Windows.Forms.Label();
            this.txtCustomVideo = new System.Windows.Forms.TextBox();
            this.txtThumbnailCache = new System.Windows.Forms.TextBox();
            this.lblThumbnailCache = new System.Windows.Forms.Label();
            this.chkDownloadThumbs = new System.Windows.Forms.CheckBox();
            this.chkEnableGZip = new System.Windows.Forms.CheckBox();
            this.chkUseAnimations = new System.Windows.Forms.CheckBox();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.panel1.Controls.Add(this.chkUseAnimations);
            this.panel1.Controls.Add(this.chkEnableGZip);
            this.panel1.Controls.Add(this.chkDownloadThumbs);
            this.panel1.Controls.Add(this.txtThumbnailCache);
            this.panel1.Controls.Add(this.lblThumbnailCache);
            this.panel1.Controls.Add(this.txtCustomVideo);
            this.panel1.Controls.Add(this.lblCustomVideo);
            this.panel1.Controls.Add(this.ddlVideoPlayer);
            this.panel1.Controls.Add(this.lblVideoPlayer);
            this.panel1.Controls.Add(this.ddlUILang);
            this.panel1.Controls.Add(this.lblUILanguage);
            this.panel1.Controls.Add(this.ddlImdbLocale);
            this.panel1.Controls.Add(this.lblImdbLocale);
            this.panel1.Location = new System.Drawing.Point(3, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 377);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem2);
            // 
            // lblImdbLocale
            // 
            this.lblImdbLocale.Location = new System.Drawing.Point(0, 0);
            this.lblImdbLocale.Name = "lblImdbLocale";
            this.lblImdbLocale.Size = new System.Drawing.Size(456, 41);
            this.lblImdbLocale.Text = "IMDb Locale:";
            // 
            // ddlImdbLocale
            // 
            this.ddlImdbLocale.Location = new System.Drawing.Point(0, 34);
            this.ddlImdbLocale.Name = "ddlImdbLocale";
            this.ddlImdbLocale.Size = new System.Drawing.Size(456, 41);
            this.ddlImdbLocale.TabIndex = 1;
            // 
            // ddlUILang
            // 
            this.ddlUILang.Location = new System.Drawing.Point(0, 115);
            this.ddlUILang.Name = "ddlUILang";
            this.ddlUILang.Size = new System.Drawing.Size(456, 41);
            this.ddlUILang.TabIndex = 3;
            // 
            // lblUILanguage
            // 
            this.lblUILanguage.Location = new System.Drawing.Point(0, 81);
            this.lblUILanguage.Name = "lblUILanguage";
            this.lblUILanguage.Size = new System.Drawing.Size(456, 41);
            this.lblUILanguage.Text = "IMDb Mobile Language:";
            // 
            // ddlVideoPlayer
            // 
            this.ddlVideoPlayer.Items.Add("Custom Video Player");
            this.ddlVideoPlayer.Items.Add("Windows Media Player");
            this.ddlVideoPlayer.Items.Add("HTC StreamingMedia");
            this.ddlVideoPlayer.Items.Add("CorePlayer");
            this.ddlVideoPlayer.Location = new System.Drawing.Point(0, 193);
            this.ddlVideoPlayer.Name = "ddlVideoPlayer";
            this.ddlVideoPlayer.Size = new System.Drawing.Size(456, 41);
            this.ddlVideoPlayer.TabIndex = 6;
            // 
            // lblVideoPlayer
            // 
            this.lblVideoPlayer.Location = new System.Drawing.Point(0, 159);
            this.lblVideoPlayer.Name = "lblVideoPlayer";
            this.lblVideoPlayer.Size = new System.Drawing.Size(456, 41);
            this.lblVideoPlayer.Text = "Video Player:";
            // 
            // lblCustomVideo
            // 
            this.lblCustomVideo.Location = new System.Drawing.Point(0, 237);
            this.lblCustomVideo.Name = "lblCustomVideo";
            this.lblCustomVideo.Size = new System.Drawing.Size(456, 41);
            this.lblCustomVideo.Text = "Custom Video Player:";
            // 
            // txtCustomVideo
            // 
            this.txtCustomVideo.Location = new System.Drawing.Point(3, 271);
            this.txtCustomVideo.Name = "txtCustomVideo";
            this.txtCustomVideo.ReadOnly = true;
            this.txtCustomVideo.Size = new System.Drawing.Size(456, 41);
            this.txtCustomVideo.TabIndex = 11;
            // 
            // txtThumbnailCache
            // 
            this.txtThumbnailCache.Location = new System.Drawing.Point(3, 346);
            this.txtThumbnailCache.Name = "txtThumbnailCache";
            this.txtThumbnailCache.Size = new System.Drawing.Size(456, 41);
            this.txtThumbnailCache.TabIndex = 13;
            // 
            // lblThumbnailCache
            // 
            this.lblThumbnailCache.Location = new System.Drawing.Point(0, 312);
            this.lblThumbnailCache.Name = "lblThumbnailCache";
            this.lblThumbnailCache.Size = new System.Drawing.Size(456, 41);
            this.lblThumbnailCache.Text = "Thumbnail Cache Path:";
            // 
            // chkDownloadThumbs
            // 
            this.chkDownloadThumbs.Location = new System.Drawing.Point(3, 393);
            this.chkDownloadThumbs.Name = "chkDownloadThumbs";
            this.chkDownloadThumbs.Size = new System.Drawing.Size(455, 40);
            this.chkDownloadThumbs.TabIndex = 17;
            this.chkDownloadThumbs.Text = "Download Thumbnails";
            // 
            // chkEnableGZip
            // 
            this.chkEnableGZip.Location = new System.Drawing.Point(3, 430);
            this.chkEnableGZip.Name = "chkEnableGZip";
            this.chkEnableGZip.Size = new System.Drawing.Size(455, 40);
            this.chkEnableGZip.TabIndex = 18;
            this.chkEnableGZip.Text = "Enable GZip Compression";
            // 
            // chkUseAnimations
            // 
            this.chkUseAnimations.Location = new System.Drawing.Point(3, 466);
            this.chkUseAnimations.Name = "chkUseAnimations";
            this.chkUseAnimations.Size = new System.Drawing.Size(455, 40);
            this.chkUseAnimations.TabIndex = 19;
            this.chkUseAnimations.Text = "Use Animations (BETA)";
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Save";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Cancel";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ClientSize = new System.Drawing.Size(480, 536);
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(0, 52);
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtCustomVideo;
        private System.Windows.Forms.Label lblCustomVideo;
        private System.Windows.Forms.ComboBox ddlVideoPlayer;
        private System.Windows.Forms.Label lblVideoPlayer;
        private System.Windows.Forms.ComboBox ddlUILang;
        private System.Windows.Forms.Label lblUILanguage;
        private System.Windows.Forms.ComboBox ddlImdbLocale;
        private System.Windows.Forms.Label lblImdbLocale;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.TextBox txtThumbnailCache;
        private System.Windows.Forms.Label lblThumbnailCache;
        private System.Windows.Forms.CheckBox chkUseAnimations;
        private System.Windows.Forms.CheckBox chkEnableGZip;
        private System.Windows.Forms.CheckBox chkDownloadThumbs;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;

    }
}