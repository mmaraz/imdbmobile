using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    class ReviewDisplay : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ReviewDisplay Sender);
        public event MouseEvent MouseUp;
        public event MouseEvent MouseDown;

        private string _heading;
        private string _text;
        private double _rating;

        public Font _bold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold);
        private Font _unbold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
            }
        }

        public string Heading
        {
            get { return _heading; }
            set { _heading = value; }
        }

        public double Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }

        public int YIndex { get; set; }
        public int Height { get; set; }
        public MichyPrima.ManilaDotNetSDK.KListControl Parent { get; set; }
        public Rectangle Bounds { get; set; }

        public string Icon
        {
            get;
            set;
        }

        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public bool ShowSeparator { get; set; }

        SizeF HeadingSize;
        SizeF TextSize;

        private Bitmap DrawnImage { get; set; }

        public ReviewDisplay()
        {
            PaddingTop = 5;
            PaddingBottom = 5;
        }

        public void CalculateHeight(int ParentWidth)
        {
            try
            {
                this.Height = 0;
                this.Height += (PaddingBottom + PaddingTop);

                using (Graphics graphicsHolder = Graphics.FromImage(new Bitmap(ParentWidth, this.Height)))
                {
                    // Heading
                    HeadingSize = Extensions.MeasureStringExtended(graphicsHolder, _heading, _bold, ParentWidth - 57);
                    this.Height += (int)HeadingSize.Height;

                    // Body
                    TextSize = Extensions.MeasureStringExtended(graphicsHolder, _text, _unbold, ParentWidth - 57);
                    this.Height += (int)TextSize.Height + 25;

                    // If it has an icon, ensure that the height is at least 48 + Padding
                    if (this.Icon != null && (this.Height < 48 + PaddingBottom + PaddingTop))
                    {
                        this.Height = 48 + PaddingTop + PaddingBottom;
                    }
                }

                this.DrawnImage = new Bitmap(ParentWidth, this.Height);

                using (Graphics g = Graphics.FromImage(this.DrawnImage))
                {
                    if (this.BackgroundColor != Color.Empty)
                    {
                        g.Clear(this.BackgroundColor);
                    }
                    else
                    {
                        g.Clear(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour);
                    }
                    if (this.Icon != null)
                    {
                        Size s = Extensions.GetBitmapDimensions(this.Icon);
                        int Y = Bounds.Y + PaddingTop;
                        Rectangle DestRect = new Rectangle(5, Y, s.Width, s.Height);
                        Extensions.DrawBitmap(g, DestRect, this.Icon);
                        if (this.Heading != null)
                        {
                            g.DrawString(this.Heading, _bold, new SolidBrush(Color.Black), new RectangleF(53, Y, (ParentWidth - 53), HeadingSize.Height));
                            Y += (int)HeadingSize.Height + 5;
                        }
                        if (this.Rating > 0)
                        {
                            int intRating = (int)Math.Floor(this.Rating);
                            int CurrX = 53;
                            Size starSize = Extensions.GetBitmapDimensions("star_over");
                            for (int i = 0; i < intRating; i++)
                            {
                                Rectangle DestRectS = new Rectangle(CurrX, Y, starSize.Width, starSize.Height);
                                Extensions.DrawBitmap(g, DestRectS, "star_over");
                                CurrX += 20;
                            }
                            for (int i = intRating; i < 10; i++)
                            {
                                Rectangle DestRectS = new Rectangle(CurrX, Y, starSize.Width, starSize.Height);
                                Extensions.DrawBitmap(g, DestRectS, "star_out");
                                CurrX += 20;
                            }
                            g.DrawString("" + this.Rating, _bold, new SolidBrush(Color.Black), CurrX + 15, Y - 5);
                            Y += 20;
                        }
                        g.DrawString(this.Text, _unbold, new SolidBrush(Color.Black), new RectangleF(53, Y, (ParentWidth - 53), TextSize.Height));
                        Y += (int)TextSize.Height + 15;
                        if (this.ShowSeparator)
                        {
                            g.DrawLine(new Pen(Color.Black), 53, Y, (ParentWidth - 53), Y);
                        }
                    }
                    else
                    {
                        int Y = Bounds.Y + PaddingTop;
                        if (this.Heading != null)
                        {
                            g.DrawString(this.Heading, _bold, new SolidBrush(Color.Black), new RectangleF(5, Y, (ParentWidth - 5), HeadingSize.Height));
                            Y += (int)HeadingSize.Height + 5;
                        }
                        if (this.Rating > 0)
                        {
                            int intRating = (int)Math.Floor(this.Rating);
                            int CurrX = 5;
                            Size starSize = Extensions.GetBitmapDimensions("star_over");
                            for (int i = 0; i < intRating; i++)
                            {
                                Rectangle DestRectS = new Rectangle(CurrX, Y, starSize.Width, starSize.Height);
                                Extensions.DrawBitmap(g, DestRectS, "star_over");
                                CurrX += 20;
                            }
                            for (int i = intRating; i < 10; i++)
                            {
                                Rectangle DestRectS = new Rectangle(CurrX, Y, starSize.Width, starSize.Height);
                                Extensions.DrawBitmap(g, DestRectS, "star_out");
                                CurrX += 20;
                            }
                            g.DrawString("" + this.Rating, _bold, new SolidBrush(Color.Black), CurrX + 15, Y - 5);
                            Y += 20;
                        }
                        g.DrawString(this.Text, _unbold, new SolidBrush(Color.Black), new RectangleF(5, Y, (ParentWidth - 5), TextSize.Height));
                        Y += (int)TextSize.Height + 5;
                        if (this.ShowSeparator)
                        {
                            g.DrawLine(new Pen(Color.Black), 5, Y, (ParentWidth - 5), Y);
                        }
                    }
                }
            }
            catch (Exception e) { }
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            if (this.DrawnImage != null)
            {
                int CurrY = Bounds.Y + PaddingTop;
                g.DrawImage(this.DrawnImage, 0, Bounds.Y + PaddingTop);
            }
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            if (this.MouseDown != null)
            {
                MouseDown(X, Y, this.Parent, this);
            }
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            if (this.MouseUp != null)
            {
                MouseUp(X, Y, this.Parent, this);
            }
        }

        public void OnMouseMove(int X, int Y)
        {

        }
    }
}
