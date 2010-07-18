using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    public class ActorHeader : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent);
        public event MouseEvent MouseUp;
        public event MouseEvent MouseDown;

        private string _text;
        private Image _cover;

        private Font _bold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold);
        private Font _unbold = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);

        public string Bio
        {
            get { return _text; }
            set
            {
                _text = value;
            }
        }

        public string Name { get; set; }
        public string Birthday { get; set; }
        public string RealName { get; set; }
        

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
                this.Parent.Refresh();
            }
        }
        
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }

        public ActorHeader()
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
                // Movie Title
                SizeF TitleSize = Extensions.MeasureStringExtended(g, this.Name, _bold, (UI.WindowHandler.ParentForm.Width - 200));
                this.Height = 100 + (int)TitleSize.Height + PaddingBottom + PaddingTop;
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
                g.FillRectangle(new SolidBrush(IMDBData.SettingsWrapper.GlobalSettings.CurrentSkin.BackgroundColour), Bounds);
            }
            if (this.Icon != null)
            {
                g.DrawImage(this.Icon, 5, Bounds.Y + PaddingTop);
            }
            else
            {
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(5, Bounds.Y + PaddingTop, 98, 140));
            }
            StringFormat sf = new StringFormat();
            int TitleY = Bounds.Y + PaddingTop;
            SizeF TitleSize = Extensions.MeasureStringExtended(g, this.Name, _bold, (this.Bounds.Width - 200));
            g.DrawString(this.Name, _bold, new SolidBrush(Color.Black), new RectangleF(15 + 108, TitleY, (this.Bounds.Width - 200), TitleSize.Height));
            TitleY += ((int)TitleSize.Height + 10);
            g.DrawString(UI.Translations.GetTranslated("0081") + ":", _unbold, new SolidBrush(Color.Black), 15 + 108, (TitleY));
            g.DrawString(this.RealName, _bold, new SolidBrush(Color.Black), 15 + 108, (22 + TitleY));
            g.DrawString(UI.Translations.GetTranslated("0082") + ":", _unbold, new SolidBrush(Color.Black), 15 + 108, (49 + TitleY));
            g.DrawString(this.Birthday, _bold, new SolidBrush(Color.Black), 15 + 108, (75 + TitleY));
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
