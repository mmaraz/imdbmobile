using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    class LoadingButton : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {

        private string _text;
        private int _textHeight;

        private Font _mainFont = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Regular);

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
            }
        }

        public int YIndex { get; set; }
        public int Height { get; set; }
        public MichyPrima.ManilaDotNetSDK.KListControl Parent { get; set; }
        public Rectangle Bounds { get; set; }
        public Image Icon { get; set; }
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public int CurrentFrame { get; set; }
        System.Windows.Forms.Timer t;

        public void Remove()
        {
            t.Enabled = false;
        }

        public LoadingButton()
        {
            PaddingTop = 20;
            PaddingBottom = 20;
            this.CurrentFrame = 1;
            t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
       }

        void t_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.Parent.Visible)
                {
                    t.Enabled = false;
                }
                else
                {
                    this.CurrentFrame++;
                    if (this.CurrentFrame > 8)
                    {
                        this.CurrentFrame = 1;
                    }
                    this.Parent.Invalidate();
                }
            }
            catch (ObjectDisposedException)
            {
                try
                {
                    t.Enabled = false;
                }
                catch (ObjectDisposedException) { }
            }
        }

        public void CalculateHeight()
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            using (Graphics g = Parent.CreateGraphics())
            {
                SizeF textSize = Extensions.MeasureStringExtended(g, _text, _mainFont, this.Parent.Width);
                _textHeight = (int)textSize.Height;
                this.Height += (int)textSize.Height;
            }
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            if (this.BackgroundColor != Color.Empty)
            {
                g.FillRectangle(new SolidBrush(this.BackgroundColor), Bounds);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.LightGoldenrodYellow), Bounds);
            }
            if (this.Icon != null)
            {
                switch (this.CurrentFrame)
                {
                    case 1: this.Icon = global::ImdbMobile.Properties.Resources.Loader1; break;
                    case 2: this.Icon = global::ImdbMobile.Properties.Resources.Loader2; break;
                    case 3: this.Icon = global::ImdbMobile.Properties.Resources.Loader3; break;
                    case 4: this.Icon = global::ImdbMobile.Properties.Resources.Loader4; break;
                    case 5: this.Icon = global::ImdbMobile.Properties.Resources.Loader5; break;
                    case 6: this.Icon = global::ImdbMobile.Properties.Resources.Loader6; break;
                    case 7: this.Icon = global::ImdbMobile.Properties.Resources.Loader7; break;
                    case 8: this.Icon = global::ImdbMobile.Properties.Resources.Loader8; break;
                }
                g.DrawImage(this.Icon, (Bounds.Width / 2) - (this.Icon.Width / 2) , Bounds.Y);
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.DrawString(this.Text, _mainFont, new SolidBrush(Color.Black), (Bounds.Width / 2), Bounds.Y + this.Icon.Height + 10, sf);
        }

        public void OnMouseDown(int x, int y, ref bool StateChanged) { }
        public void OnMouseUp(int x, int y, bool StateChanged) { }

        public void OnMouseMove(int X, int Y)
        {

        }
    }
}

