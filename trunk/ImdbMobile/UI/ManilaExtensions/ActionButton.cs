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
        public bool ShowHover { get; set; }

        public ActionButton()
        {
            PaddingTop = 20;
            PaddingBottom = 20;
        }

        public void CalculateHeight()
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            SizeF textSize = Extensions.MeasureStringExtended(Parent.CreateGraphics(), _text, _mainFont, this.Parent.Width);
            _textHeight = (int)textSize.Height;
            this.Height += (int)textSize.Height;
        }

        public void Render(Graphics g, Rectangle Bounds, bool Param)
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
                g.DrawImage(this.Icon, 5, Bounds.Y + PaddingTop);
            }
            if (this.HoverIcon != null && this.ShowHover)
            {
                g.DrawImage(this.HoverIcon, 5, Bounds.Y + PaddingTop);
            }
            g.DrawString(this.Text, _mainFont, new SolidBrush(Color.Black), 65, Bounds.Y + PaddingTop);
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            this.Parent.Invalidate();
            if (this.MouseDown != null)
            {
                MouseDown(X, Y, this.Parent, this);
            }
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            if (this.MouseUp != null)
            {
                this.ShowHover = true;
                MouseUp(X, Y, this.Parent, this);
            }
        }
    }
}
