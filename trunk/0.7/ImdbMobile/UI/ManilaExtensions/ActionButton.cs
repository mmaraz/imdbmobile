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

        private Font _mainFont = new Font(IMDBData.SettingsWrapper.GlobalSettings.FontName, IMDBData.SettingsWrapper.GlobalSettings.FontSize_Large, IMDBData.SettingsWrapper.GlobalSettings.FontStyle_Large);

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
        public string Icon { get; set; }
        public Image HoverIcon { get; set; }
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public Color FontColor { get; set; }
        private Bitmap DrawnImage { get; set; }

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
                this.DrawnImage = new Bitmap(this.Parent.Width, this.Height);
            }
            using(Graphics g = Graphics.FromImage(this.DrawnImage))
            {
                // Draw Info to in-memory bitmap
                if (this.BackgroundColor != Color.Empty)
                {
                    g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, this.DrawnImage.Width, this.DrawnImage.Height);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.LightGoldenrodYellow), 0, 0, this.DrawnImage.Width, this.DrawnImage.Height);
                }
                if (this.Icon != null)
                {
                    Size s = Extensions.GetBitmapDimensions(this.Icon);
                    Extensions.DrawBitmap(g, new Rectangle(5, PaddingTop, s.Width, s.Height), this.Icon);
                }

                Size s2 = Extensions.GetBitmapDimensions("next");
                Extensions.DrawBitmap(g, new Rectangle(this.Parent.Width - s2.Width - 5, ((this.Height / 2) - (s2.Height / 2)), s2.Width, s2.Height), "next");
                g.DrawString(this.Text, _mainFont, new SolidBrush(this.FontColor), 65, PaddingTop);
            }
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            g.DrawImage(this.DrawnImage, 0, Bounds.Y);
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
