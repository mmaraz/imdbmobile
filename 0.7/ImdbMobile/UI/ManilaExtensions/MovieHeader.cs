using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

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

            using (Graphics gHolder = Parent.CreateGraphics())
            {

                // Movie Title
                TitleSize = Extensions.MeasureStringExtended(gHolder, this.MovieTitle, _bold, (this.Parent.Width - 200));
                this.Height = 220 + (int)TitleSize.Height + PaddingBottom + PaddingTop;
            }

            this.DrawnImage = new Bitmap(480, this.Height);

            using (Graphics g = Graphics.FromImage(this.DrawnImage))
            {
                g.Clear(Color.LightGoldenrodYellow);

                if (this.BackgroundColor != Color.Empty)
                {
                    g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, 480, this.Height * 2);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.LightGoldenrodYellow), 0, 0, 480, this.Height * 2);
                }
                if (this.Icon != null)
                {
                    g.DrawImage(this.Icon, 5, PaddingTop);
                }
                else
                {
                    g.DrawRectangle(new Pen(Color.Black), new Rectangle(5, PaddingTop, 98, 140));
                }
                StringFormat sf = new StringFormat();
                int TitleY = PaddingTop;

                g.DrawString(this.MovieTitle, _bold, new SolidBrush(Color.Black), new RectangleF(15 + 108, TitleY, (this.DrawnImage.Width - 200), TitleSize.Height));
                TitleY += ((int)TitleSize.Height - 25);
                int CurrX = 15 + 108;

                int intRating = (int)Math.Floor(this.Rating);
                for (int i = 0; i < intRating; i++)
                {
                    g.DrawImage(global::ImdbMobile.Properties.Resources.star_over, CurrX, TitleY + 30);
                    CurrX += 20;
                }
                for (int i = intRating; i < 10; i++)
                {
                    g.DrawImage(global::ImdbMobile.Properties.Resources.star_out, CurrX, TitleY + 30);
                    CurrX += 20;
                }
                g.DrawString("" + this.Rating, _bold, new SolidBrush(Color.Black), CurrX + 15, TitleY + 25);

                TitleY -= 25;
                g.DrawString(UI.Translations.GetTranslated("0083") + ":", _unbold, new SolidBrush(Color.Black), 15 + 108, (74 + TitleY));
                g.DrawString(this.Director, _bold, new SolidBrush(Color.Black), 15 + 108, (96 + TitleY));
                g.DrawString(UI.Translations.GetTranslated("0084") + ":", _unbold, new SolidBrush(Color.Black), 15 + 108, (123 + TitleY));
                g.DrawString(this.Writer, _bold, new SolidBrush(Color.Black), 15 + 108, (145 + TitleY));
                g.DrawString(UI.Translations.GetTranslated("0085") + ":", _unbold, new SolidBrush(Color.Black), 15 + 108, (171 + TitleY));
                g.DrawString(this.Genres, _bold, new SolidBrush(Color.Black), 15 + 108, (194 + TitleY));
                if (this.Certificate != null)
                {
                    g.DrawString(UI.Translations.GetTranslated("0086") + ":", _unbold, new SolidBrush(Color.Black), 15 + 108, (219 + TitleY));
                    g.DrawString(this.Certificate, _bold, new SolidBrush(Color.Black), 15 + 108, (242 + TitleY));
                }

            }
        }

        public void Render(Graphics g, Rectangle Bounds)
        {
            g.Clear(Color.LightGoldenrodYellow);
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
