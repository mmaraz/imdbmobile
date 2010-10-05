using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ImdbMobile.IMDBData;

namespace ImdbMobile.UI
{
    public class MovieHeader : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent);
        public event MouseEvent MouseUp;
        public event MouseEvent MouseDown;

        private string _text;
        private string _director;
        private string _writer;
        private string _runtime;
        private string _genres;
        private string _certificate;
        private Image _cover;

        private Font _bold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold);
        private Font _unbold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);

        public string MovieTitle
        {
            get { return _text; }
            set
            {
                _text = value;
            }
        }

        public string Director
        {
            get { return _director; }
            set { _director = value; }
        }

        public string Writer
        {
            get { return _writer; }
            set { _writer = value; }
        }

        public string Runtime
        {
            get { return _runtime; }
            set { _runtime = value; }
        }

        public string Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }

        public string Certificate
        {
            get { return _certificate; }
            set { _certificate = value; }
        }

        public double Rating { get; set; }

        public int YIndex { get; set; }
        public int Height { get; set; }
        public MichyPrima.ManilaDotNetSDK.KListControl Parent { get; set; }
        public Rectangle Bounds { get; set; }

        public Image Icon
        {
            get { return _cover; }
            set 
            {
                _cover = value;
                this.CalculateHeight();
                this.Parent.Refresh();
            }
        }
        
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }

        private Bitmap DrawnImage { get; set; }

        public MovieHeader()
        {
            PaddingTop = 20;
            PaddingBottom = 20;
        }

        public void CalculateHeight()
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            SizeF TitleSize = new SizeF();

            int imageWidth = 215;
            if (!SettingsWrapper.GlobalSettings.UseBigImages)
            {
                imageWidth = 108;
            }

            using (Graphics gHolder = Parent.CreateGraphics())
            {
                // Movie Title
                TitleSize = Extensions.MeasureStringExtended(gHolder, this.MovieTitle, _bold, (this.Parent.Width - imageWidth));
                this.Height = 300 + (int)TitleSize.Height + PaddingBottom + PaddingTop;
            }

            this.DrawnImage = new Bitmap(480, this.Height);

            using (Graphics g = Graphics.FromImage(this.DrawnImage))
            {
                g.Clear(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour);

                if (this.BackgroundColor != Color.Empty)
                {
                    g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, 480, this.Height * 2);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour), 0, 0, 480, this.Height * 2);
                }
                if (this.Icon != null)
                {
                    g.DrawImage(this.Icon, 5, PaddingTop);
                }
                else
                {
                    if (SettingsWrapper.GlobalSettings.UseBigImages)
                    {
                        g.DrawRectangle(new Pen(Color.Black), new Rectangle(5, PaddingTop, 170, 243));
                    }
                    else
                    {
                        g.DrawRectangle(new Pen(Color.Black), new Rectangle(5, PaddingTop, 98, 140));
                    }
                }
                StringFormat sf = new StringFormat();
                int TitleY = PaddingTop;
                int TitleX = 108;

                if (SettingsWrapper.GlobalSettings.UseBigImages)
                {
                    TitleX = 189;
                }



                g.DrawString(this.MovieTitle, _bold, new SolidBrush(Color.Black), new RectangleF(15 + TitleX, TitleY, (this.DrawnImage.Width - imageWidth), TitleSize.Height));
                TitleY += ((int)TitleSize.Height - 25);
                int CurrX = 15 + TitleX;

                int intRating = (int)Math.Floor(this.Rating);
                Size starSize = Extensions.GetBitmapDimensions("star_over");
                for (int i = 0; i < intRating; i++)
                {
                    Rectangle DestRect = new Rectangle(CurrX, TitleY + 30, starSize.Width, starSize.Height);
                    Extensions.DrawBitmap(g, DestRect, "star_over");
                    CurrX += 20;
                }
                for (int i = intRating; i < 10; i++)
                {
                    Rectangle DestRect = new Rectangle(CurrX, TitleY + 30, starSize.Width, starSize.Height);
                    Extensions.DrawBitmap(g, DestRect, "star_out");
                    CurrX += 20;
                }
                g.DrawString("" + this.Rating, _bold, new SolidBrush(Color.Black), CurrX + 15, TitleY + 25);

                TitleY -= 25;
                g.DrawString(UI.Translations.GetTranslated("0083") + ":", _unbold, new SolidBrush(Color.Black), 15 + TitleX, (74 + TitleY));
                g.DrawString(this.Director, _bold, new SolidBrush(Color.Black), 15 + TitleX, (96 + TitleY));
                g.DrawString(UI.Translations.GetTranslated("0084") + ":", _unbold, new SolidBrush(Color.Black), 15 + TitleX, (123 + TitleY));
                g.DrawString(this.Writer, _bold, new SolidBrush(Color.Black), 15 + TitleX, (145 + TitleY));
                g.DrawString(UI.Translations.GetTranslated("0085") + ":", _unbold, new SolidBrush(Color.Black), 15 + TitleX, (171 + TitleY));

                SizeF GenresSize = Extensions.MeasureStringExtended(g, this.Genres, _bold, (this.Bounds.Width - imageWidth));
                g.DrawString(this.Genres, _bold, new SolidBrush(Color.Black), new RectangleF(15 + TitleX, 194 + TitleY, (this.Bounds.Width - imageWidth), GenresSize.Height));
                TitleY += ((int)GenresSize.Height) - 25;
                                
                if (this.Certificate != null)
                {
                    g.DrawString(UI.Translations.GetTranslated("0086") + ":", _unbold, new SolidBrush(Color.Black), 15 + TitleX, (219 + TitleY));
                    g.DrawString(this.Certificate, _bold, new SolidBrush(Color.Black), 15 + TitleX, (242 + TitleY));
                }
                g.DrawString(UI.Translations.GetTranslated("0101") + ":", _unbold, new SolidBrush(Color.Black), 15 + TitleX, (268 + TitleY));
                g.DrawString(this.Runtime + " " + UI.Translations.GetTranslated("0102"), _bold, new SolidBrush(Color.Black), 15 + TitleX, (291 + TitleY));
            }
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            g.Clear(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour);
            int CurrY = Bounds.Y + PaddingTop;
            g.DrawImage(this.DrawnImage, 0, Bounds.Y + PaddingTop);
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            if (this.MouseDown != null)
            {
                MouseDown(X, Y, this.Parent);
            }
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            if (this.MouseUp != null)
            {
                MouseUp(X, Y, this.Parent);
            }
        }

        public void OnMouseMove(int X, int Y)
        {

        }
    }
}
