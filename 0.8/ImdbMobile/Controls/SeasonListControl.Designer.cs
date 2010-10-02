namespace ImdbMobile.Controls
{
    partial class SeasonListControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoadingList = new MichyPrima.ManilaDotNetSDK.KListControl();
            this.kListControl1 = new MichyPrima.ManilaDotNetSDK.KListControl();
            this.SuspendLayout();
            // 
            // LoadingList
            // 
            this.LoadingList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingList.BackColor = IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour;
            this.LoadingList.Location = new System.Drawing.Point(0, 0);
            this.LoadingList.Name = "LoadingList";
            this.LoadingList.Size = new System.Drawing.Size(474, 377);
            this.LoadingList.TabIndex = 8;
            // 
            // kListControl1
            // 
            this.kListControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kListControl1.BackColor = IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour;
            this.kListControl1.Location = new System.Drawing.Point(0, 0);
            this.kListControl1.Name = "kListControl1";
            this.kListControl1.Size = new System.Drawing.Size(474, 377);
            this.kListControl1.TabIndex = 7;
            // 
            // SeasonListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.LoadingList);
            this.Controls.Add(this.kListControl1);
            this.Name = "SeasonListControl";
            this.Size = new System.Drawing.Size(474, 377);
            this.ResumeLayout(false);

        }

        #endregion

        private MichyPrima.ManilaDotNetSDK.KListControl LoadingList;
        private MichyPrima.ManilaDotNetSDK.KListControl kListControl1;
    }
}
