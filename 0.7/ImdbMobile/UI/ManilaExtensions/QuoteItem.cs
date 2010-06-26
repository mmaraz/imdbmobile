using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    class QuoteItem : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent);
        public event MouseEvent MouseUp;
        public event MouseEvent MouseDown;


        private Font _bold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold);
        private Font _unbold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);

        public IMDBData.ImdbQuoteSection QuoteSection { get; set; }

        public int YIndex { get; set; }
        public int Height { get; set; }
        public MichyPrima.ManilaDotNetSDK.KListControl Parent { get; set; }
        public Rectangle Bounds { get; set; }
        public Image Icon { get; set; }
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public int ListWidth { get; set; }

        private Bitmap DrawnImage { get; set; }

        public QuoteItem()
        {
            PaddingTop = 20;
            PaddingBottom = 20;
        }

        public void CalculateHeight()
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            foreach (IMDBData.ImdbQuote iq in this.QuoteSection.Quotes)
            {
                using (Graphics g = Parent.CreateGraphics())
                {
                    SizeF textSize = Extensions.MeasureStringExtended(g, iq.Quote, _unbold, UI.WindowHandler.ParentForm.Width - 15);
                    this.Height += (int)textSize.Height;

                    textSize = Extensions.MeasureStringExtended(g, iq.Character.CharacterName, _bold, UI.WindowHandler.ParentForm.Width - 15);
                    this.Height += (int)textSize.Height;
                }

                
            }

            Bitmap b = new Bitmap(UI.WindowHandler.ParentForm.Width, this.Height);
            using (Graphics g = Graphics.FromImage(b))
            {

                Rectangle CurrentBounds = new Rectangle(0, 0, UI.WindowHandler.ParentForm.Width, this.Height);

                if (this.BackgroundColor != Color.Empty)
                {
                    g.FillRectangle(new SolidBrush(this.BackgroundColor), CurrentBounds);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.LightGoldenrodYellow), CurrentBounds);
                }
                if (this.Icon != null)
                {
                    g.DrawImage(this.Icon, 5, PaddingTop);
                }

                int CurrY = 0;
                foreach (IMDBData.ImdbQuote q in this.QuoteSection.Quotes)
                {
                    string CharText = "";
                    if (string.IsNullOrEmpty(q.Character.CharacterName))
                    {
                        CharText = "(" + UI.Translations.GetTranslated("0087") + "):";
                    }
                    else
                    {
                        CharText = q.Character.CharacterName + ":";
                    }
                    // Draw Character Name
                    SizeF charSize = Extensions.MeasureStringExtended(g, CharText, _bold, UI.WindowHandler.ParentForm.Width);
                    g.DrawString(CharText, _bold, new SolidBrush(Color.Black), new RectangleF(5, CurrY, UI.WindowHandler.ParentForm.Width, charSize.Height));
                    CurrY += (int)charSize.Height;

                    // Draw Quote
                    SizeF qSize = Extensions.MeasureStringExtended(g, q.Quote, _unbold, ListWidth);
                    g.DrawString(q.Quote, _unbold, new SolidBrush(Color.Black), new RectangleF(5, CurrY, UI.WindowHandler.ParentForm.Width, qSize.Height));
                    CurrY += (int)qSize.Height;
                }
                CurrY += PaddingBottom;
                // Draw Separator
                g.DrawLine(new Pen(Color.Black), 5, CurrY, UI.WindowHandler.ParentForm.Width - 5, CurrY);
                this.Height += 5;

                this.DrawnImage = b;

            }
        }

        public void Render(Graphics g, Rectangle Bounds, bool Param)
        {
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
    }
}
