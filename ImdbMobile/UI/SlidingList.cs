using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ImdbMobile.IMDBData;

namespace ImdbMobile.UI
{
    public class SlidingList : System.Windows.Forms.UserControl, IDisposable
    {
        public System.Windows.Forms.Form ParentForm { get; set; }
        private System.Windows.Forms.Timer LeftTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer RightTimer = new System.Windows.Forms.Timer();
        public List<System.Threading.Thread> ThreadList = new List<System.Threading.Thread>();
        public List<ImageDownloader> ImageDownloaderList = new List<ImageDownloader>();

        public void Close()
        {
            SlideOutRightKill();
        }

        public void Dispose()
        {
            try
            {
                foreach (System.Threading.Thread t in ThreadList)
                {
                    if (t != null)
                    {
                        try
                        {
                            t.Abort();
                        }
                        catch (Exception) { }
                    }
                }
                foreach (ImageDownloader id in ImageDownloaderList)
                {
                    if (id != null)
                    {
                        id.Kill();
                    }
                }
            }
            catch (Exception) { }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialise()
        {
            // TODO - (W)QVGA
            this.Location = new System.Drawing.Point(0, 156);
            this.Width = this.ParentForm.Width;
            this.Height = this.ParentForm.Height - 156;
            this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ParentForm.Controls.Add(this);
            this.BringToFront();
            SlideInLeft();
        }

        public void SlideInLeft()
        {
            this.ParentForm.Controls.Add(this);
            this.BringToFront();
            if (SettingsWrapper.GlobalSettings.UseAnimations)
            {
                this.Location = new System.Drawing.Point(0 - this.ParentForm.Width, 156);
                RightTimer.Enabled = false;
                LeftTimer.Interval = 1;
                LeftTimer.Tick += new EventHandler(t_Tick);
                LeftTimer.Enabled = true;
            }
            else
            {
                this.Visible = true;
            }
        }

        public void SlideOutRight()
        {
            foreach (ImageDownloader id in ImageDownloaderList)
            {
                if (id != null)
                {
                    id.Kill();
                }
            }

            if (SettingsWrapper.GlobalSettings.UseAnimations)
            {
                LeftTimer.Enabled = false;
                RightTimer.Interval = 1;
                RightTimer.Tick += new EventHandler(t_TickOut);
                RightTimer.Enabled = true;
            }
            else
            {
                this.Visible = false;
            }
        }

        public void SlideOutRightKill()
        {
            foreach (ImageDownloader id in ImageDownloaderList)
            {
                if (id != null)
                {
                    id.Kill();
                }
            }
            
            if (SettingsWrapper.GlobalSettings.UseAnimations)
            {
                LeftTimer.Enabled = false;
                RightTimer.Interval = 1;
                RightTimer.Tick += new EventHandler(t_TickOutKill);
                RightTimer.Enabled = true;
            }
            else
            {
                this.Dispose();
            }
        }

        private delegate void Action();

        void t_Tick(object sender, EventArgs e)
        {
            Action IncreaseX = delegate
            {
                this.Location = new System.Drawing.Point(this.Location.X + 10, this.Location.Y);
                if (this.Location.X >= 0)
                {
                    Action StopTimer = delegate
                    {
                        this.LeftTimer.Enabled = false;
                        this.Location = new System.Drawing.Point(0, this.Location.Y);
                    };
                    this.Invoke(StopTimer);
                }
            };
            this.Invoke(IncreaseX);
        }

        void t_TickOut(object sender, EventArgs e)
        {
            Action DescreaseX = delegate
            {
                this.Location = new System.Drawing.Point(this.Location.X + 30, this.Location.Y);
                if (this.Location.X >= (this.ParentForm.Width))
                {
                    Action StopTimer = delegate
                    {
                        this.RightTimer.Enabled = false;
                        this.Location = new System.Drawing.Point(0 - this.ParentForm.Width, this.Location.Y);
                        this.ParentForm.Controls.Remove(this);
                    };
                    this.Invoke(StopTimer);
                }
            };
            this.Invoke(DescreaseX);
        }

        void t_TickOutKill(object sender, EventArgs e)
        {
            Action DescreaseX = delegate
            {
                this.Location = new System.Drawing.Point(this.Location.X + 30, this.Location.Y);
                if (this.Location.X >= (this.ParentForm.Width))
                {
                    Action StopTimer = delegate
                    {
                        this.RightTimer.Enabled = false;
                        this.Location = new System.Drawing.Point(0 - this.ParentForm.Width, this.Location.Y);
                        this.Dispose();
                    };
                    this.Invoke(StopTimer);
                }
            };
            this.Invoke(DescreaseX);
        }
    }
}
