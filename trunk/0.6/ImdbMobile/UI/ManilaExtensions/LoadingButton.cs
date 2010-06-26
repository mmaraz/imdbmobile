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

        public LoadingButton()
        {
            PaddingTop = 20;
            PaddingBottom = 20;
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
                g.DrawImage(this.Icon, (Bounds.Width / 2) - (this.Icon.Width / 2) , Bounds.Y);
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.DrawString(this.Text, _mainFont, new SolidBrush(Color.Black), (Bounds.Width / 2), Bounds.Y + this.Icon.Height + 10, sf);
        }

        public void OnMouseDown(int x, int y, ref bool StateChanged) { }
        public void OnMouseUp(int x, int y, bool StateChanged) { }
    }
}

