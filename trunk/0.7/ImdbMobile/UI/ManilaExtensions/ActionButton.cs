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

        private Font _mainFont = new Font(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.FontName, IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.FontSize_Large, IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.FontStyle_Large);

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
        private Bitmap DrawnImageHover { get; set; }
        private bool DoHover { get; set; }

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
                this.DrawnImage = new Bitmap(UI.WindowHandler.ParentForm.Width, this.Height);
                this.DrawnImageHover = new Bitmap(UI.WindowHandler.ParentForm.Width, this.Height);
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
                    g.FillRectangle(new SolidBrush(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour), 0, 0, this.DrawnImage.Width, this.DrawnImage.Height);
                }
                if (this.Icon != null)
                {
                    Size s = Extensions.GetBitmapDimensions(this.Icon);
                    Extensions.DrawBitmap(g, new Rectangle(5, PaddingTop, s.Width, s.Height), this.Icon);
                }

                Size s2 = Extensions.GetBitmapDimensions("next");
                Extensions.DrawBitmap(g, new Rectangle(UI.WindowHandler.ParentForm.Width - s2.Width - 5, ((this.Height / 2) - (s2.Height / 2)), s2.Width, s2.Height), "next");
                g.DrawString(this.Text, _mainFont, new SolidBrush(this.FontColor), 65, PaddingTop);
            }

            using (Graphics g = Graphics.FromImage(this.DrawnImageHover))
            {
                // Draw Info to in-memory bitmap
                Rectangle DestRect = new Rectangle(0, 0, this.DrawnImageHover.Width, this.DrawnImageHover.Height);
                Color start = IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.ActionHoverFrom;
                Color stop = IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.ActionHoverTo;
                GradientFill.Fill(g, DestRect, start, stop, GradientFill.FillDirection.TopToBottom);
                
                if (this.Icon != null)
                {
                    Size s = Extensions.GetBitmapDimensions(this.Icon);
                    Extensions.DrawBitmap(g, new Rectangle(5, PaddingTop, s.Width, s.Height), this.Icon);
                }

                Size s2 = Extensions.GetBitmapDimensions("next");
                Extensions.DrawBitmap(g, new Rectangle(UI.WindowHandler.ParentForm.Width - s2.Width - 5, ((this.Height / 2) - (s2.Height / 2)), s2.Width, s2.Height), "next");
                g.DrawString(this.Text, _mainFont, new SolidBrush(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.ActionTextHover), 65, PaddingTop);
            }
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            if (this.DoHover)
            {
                g.DrawImage(this.DrawnImageHover, 0, Bounds.Y);
            }
            else
            {
                g.DrawImage(this.DrawnImage, 0, Bounds.Y);
            }
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            if (IsSamePoint)
            {
                this.DoHover = true;
            }
            this.Parent.Invalidate();
            if (this.MouseDown != null)
            {
                MouseDown(X, Y, this.Parent, this);
            }
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            this.DoHover = false;

            if (this.MouseUp != null)
            {
                MouseUp(X, Y, this.Parent, this);
            }
        }

        public void OnMouseMove(int X, int Y)
        {
            this.DoHover = false;
        }
    }
}
