﻿namespace ImdbMobile.Controls
{
    partial class QuotesControl
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
            this.LoadingKlist = new MichyPrima.ManilaDotNetSDK.KListControl();
            this.kListControl1 = new MichyPrima.ManilaDotNetSDK.KListControl();
            this.SuspendLayout();
            // 
            // LoadingKlist
            // 
            this.LoadingKlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingKlist.BackColor = IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour;
            this.LoadingKlist.Location = new System.Drawing.Point(0, 0);
            this.LoadingKlist.Name = "LoadingKlist";
            this.LoadingKlist.Size = new System.Drawing.Size(474, 377);
            this.LoadingKlist.TabIndex = 5;
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
            this.kListControl1.TabIndex = 4;
            // 
            // QuotesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.LoadingKlist);
            this.Controls.Add(this.kListControl1);
            this.Name = "QuotesControl";
            this.Size = new System.Drawing.Size(474, 377);
            this.ResumeLayout(false);

        }

        #endregion

        private MichyPrima.ManilaDotNetSDK.KListControl LoadingKlist;
        private MichyPrima.ManilaDotNetSDK.KListControl kListControl1;
    }
}