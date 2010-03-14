namespace ImdbMobile.Controls
{
    partial class PhotoViewerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoViewerControl));
            this.LoadingList = new MichyPrima.ManilaDotNetSDK.KListControl();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pb6 = new System.Windows.Forms.PictureBox();
            this.pb5 = new System.Windows.Forms.PictureBox();
            this.pb4 = new System.Windows.Forms.PictureBox();
            this.pb3 = new System.Windows.Forms.PictureBox();
            this.pb2 = new System.Windows.Forms.PictureBox();
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // LoadingList
            // 
            this.LoadingList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingList.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.LoadingList.Location = new System.Drawing.Point(0, 0);
            this.LoadingList.Name = "LoadingList";
            this.LoadingList.Size = new System.Drawing.Size(474, 377);
            this.LoadingList.TabIndex = 23;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(426, 179);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(48, 48);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 179);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(-3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(480, 29);
            this.label1.Text = "Page 1 of 20";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pb6
            // 
            this.pb6.BackColor = System.Drawing.Color.DarkKhaki;
            this.pb6.Location = new System.Drawing.Point(248, 262);
            this.pb6.Name = "pb6";
            this.pb6.Size = new System.Drawing.Size(160, 106);
            // 
            // pb5
            // 
            this.pb5.BackColor = System.Drawing.Color.DarkKhaki;
            this.pb5.Location = new System.Drawing.Point(67, 262);
            this.pb5.Name = "pb5";
            this.pb5.Size = new System.Drawing.Size(160, 106);
            // 
            // pb4
            // 
            this.pb4.BackColor = System.Drawing.Color.DarkKhaki;
            this.pb4.Location = new System.Drawing.Point(248, 150);
            this.pb4.Name = "pb4";
            this.pb4.Size = new System.Drawing.Size(160, 106);
            // 
            // pb3
            // 
            this.pb3.BackColor = System.Drawing.Color.DarkKhaki;
            this.pb3.Location = new System.Drawing.Point(67, 150);
            this.pb3.Name = "pb3";
            this.pb3.Size = new System.Drawing.Size(160, 106);
            // 
            // pb2
            // 
            this.pb2.BackColor = System.Drawing.Color.DarkKhaki;
            this.pb2.Location = new System.Drawing.Point(248, 38);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(160, 106);
            // 
            // pb1
            // 
            this.pb1.BackColor = System.Drawing.Color.DarkKhaki;
            this.pb1.Location = new System.Drawing.Point(67, 38);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(160, 106);
            // 
            // PhotoViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.Controls.Add(this.LoadingList);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pb6);
            this.Controls.Add(this.pb5);
            this.Controls.Add(this.pb4);
            this.Controls.Add(this.pb3);
            this.Controls.Add(this.pb2);
            this.Controls.Add(this.pb1);
            this.Name = "PhotoViewerControl";
            this.Size = new System.Drawing.Size(474, 377);
            this.ResumeLayout(false);

        }

        #endregion

        private MichyPrima.ManilaDotNetSDK.KListControl LoadingList;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pb6;
        private System.Windows.Forms.PictureBox pb5;
        private System.Windows.Forms.PictureBox pb4;
        private System.Windows.Forms.PictureBox pb3;
        private System.Windows.Forms.PictureBox pb2;
        private System.Windows.Forms.PictureBox pb1;
    }
}
