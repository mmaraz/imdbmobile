using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    class PagerDisplay : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, PagerDisplay Sender);
        public event MouseEvent Next;
        public event MouseEvent Previous;
        public event MouseEvent MouseDown;

        private string _text;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private Font _mainFont = new Font(FontFamily.GenericSansSerif, 11f, FontStyle.Regular);

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
        public Image HoverIcon { get; set; }
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public bool ShowHoverLeft { get; set; }
        public bool ShowHoverRight { get; set; }

        public PagerDisplay()
        {
            PaddingTop = 5;
            PaddingBottom = 5;
        }

        public void CalculateHeight()
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            this.Height += 32;

            this.Parent.MouseMove += new System.Windows.Forms.MouseEventHandler(Parent_MouseMove);
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            if (this.ShowHoverLeft)
            {
                g.DrawImage(global::ImdbMobile.Properties.Resources.previous_Over, 0, 0);
            }
            else
            {
                g.DrawImage(global::ImdbMobile.Properties.Resources.previous, 0, 0);
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.DrawString("Page " + CurrentPage + " of " + TotalPages, _mainFont, new SolidBrush(Color.Black), new RectangleF(32, 0, Bounds.Width - 64, 32), sf);
            if (this.ShowHoverRight)
            {
                g.DrawImage(global::ImdbMobile.Properties.Resources.next_Over, Bounds.Width - 32, 0);
            }
            else
            {
                g.DrawImage(global::ImdbMobile.Properties.Resources.next, Bounds.Width - 32, 0);
            }
        }

        void Parent_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                this.ShowHoverLeft = false;
                this.ShowHoverRight = false;
            }
            catch (Exception) { }
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            this.Parent.Invalidate();
            if (this.MouseDown != null)
            {
                if (X >= 0 && X <= 32 && Y >= 0 && Y <= 32)
                {
                    this.ShowHoverLeft = true;
                }
                if ((X >= this.Bounds.Width - 32) && (X <= this.Bounds.Width) && Y >= 0 && Y <= 32)
                {
                    this.ShowHoverRight = true;
                }
                MouseDown(X, Y, this.Parent, this);
            }
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            if (X >= 0 && X <= 32 && Y >= 0 && Y <= 32)
            {
                if (this.Previous != null)
                {
                    Previous(X, Y, this.Parent, this);
                }
            }
            if ((X >= this.Bounds.Width - 32) && (X <= this.Bounds.Width) && Y >= 0 && Y <= 32)
            {
                if (this.Next != null)
                {
                    Next(X, Y, this.Parent, this);
                }
            }
        }

        public void OnMouseMove(int X, int Y)
        {

        }
    }
}
