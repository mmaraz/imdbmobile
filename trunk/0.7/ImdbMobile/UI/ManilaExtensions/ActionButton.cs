using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    class ActionButton : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ActionButton Sender);
        public event MouseEvent MouseUp;
        public event MouseEvent MouseDown;

        private string _text;
        private int _textHeight;

        private Font _mainFont = new Font(FontFamily.GenericSansSerif, 12f, FontStyle.Bold);

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
        public Color FontColor { get; set; }

        public ActionButton()
        {
            PaddingTop = 20;
            PaddingBottom = 20;
            this.FontColor = Color.Black;
        }

        public void CalculateHeight()
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            using(Graphics g = Parent.CreateGraphics())
            {
                SizeF textSize = Extensions.MeasureStringExtended(g, _text, _mainFont, UI.WindowHandler.ParentForm.Width);
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
                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorKey(Color.FromArgb(250, 250, 210), Color.FromArgb(250, 250, 210));
                g.DrawImage(this.Icon, new Rectangle(5, Bounds.Y + PaddingTop, this.Icon.Width, this.Icon.Height), 0, 0, this.Icon.Width, this.Icon.Height, GraphicsUnit.Pixel, ia);
            }
            g.DrawString(this.Text, _mainFont, new SolidBrush(this.FontColor), 65, Bounds.Y + PaddingTop);
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            if (IsSamePoint)
            {
                this.BackgroundColor = Color.FromArgb(41, 47, 49);
                this.FontColor = Color.FromArgb(250, 250, 210);
            }
            this.Parent.Invalidate();
            if (this.MouseDown != null)
            {
                MouseDown(X, Y, this.Parent, this);
            }
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            this.BackgroundColor = Color.LightGoldenrodYellow;
            this.FontColor = Color.Black;
            if (this.MouseUp != null)
            {
                MouseUp(X, Y, this.Parent, this);
            }
        }

        public void OnMouseMove(int X, int Y)
        {
            this.BackgroundColor = Color.LightGoldenrodYellow;
            this.FontColor = Color.Black;
        }
    }
}
