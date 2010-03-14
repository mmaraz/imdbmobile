using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImdbMobile.UI
{
    class PhotoDisplay : MichyPrima.ManilaDotNetSDK.KListControl.IKListItem
    {
        public delegate void MouseEvent(PhotoDisplay Sender, int PhotoIndex);
        public event MouseEvent MouseUp;
        public event MouseEvent MouseDown;

        public int YIndex { get; set; }
        public int Height { get; set; }
        public MichyPrima.ManilaDotNetSDK.KListControl Parent { get; set; }
        public Rectangle Bounds { get; set; }

        private Image _image1;
        private Image _image2;
        private Image _image3;

        public Image Image1
        {
            get { return _image1; }
            set
            {
                _image1 = value;
            }
        }

        public Image Image2
        {
            get { return _image2; }
            set
            {
                _image2 = value;
            }
        }

        public Image Image3
        {
            get { return _image3; }
            set
            {
                _image3 = value;
            }
        }
        
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public bool ShowSeparator { get; set; }

        public PhotoDisplay()
        {
            PaddingTop = 5;
            PaddingBottom = 5;
        }

        public void CalculateHeight(int ParentWidth)
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            this.Height += 154;
        }

        public void Render(Graphics g, Rectangle Bounds, bool Param)
        {
            if (this.Image1 == null)
            {
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(15, Bounds.Y + PaddingTop, 108, 72));
            }
            else
            {
                g.DrawImage(this.Image1, 15, Bounds.Y + PaddingTop);
            }
            if (this.Image2 == null)
            {
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(186, Bounds.Y + PaddingTop, 108, 72));
            }
            else
            {
                g.DrawImage(this.Image2, 186, Bounds.Y + PaddingTop);
            }
            if (this.Image3 == null)
            {
                g.DrawRectangle(new Pen(Color.Black), new Rectangle(357, Bounds.Y + PaddingTop, 108, 72));
            }
            else
            {
                g.DrawImage(this.Image3, 357, Bounds.Y + PaddingTop);
            }
        }

        public void OnMouseDown(int X, int Y, ref bool IsSamePoint)
        {
            if (this.MouseDown != null)
            {
                int gImage = GetImage(X, Y);
                if (gImage > -1)
                {
                    MouseDown(this, gImage);
                }
            }
        }

        private int GetImage(int X, int Y)
        {
            if ((X >= 15 & X <= 123) && (Y >= PaddingTop && Y <= PaddingTop + 72))
            {
                return 1;
            }
            if ((X >= 186 & X <= 294) && (Y >= PaddingTop && Y <= PaddingTop + 72))
            {
                return 2;
            }
            if ((X >= 357 & X <= 465) && (Y >= PaddingTop && Y <= PaddingTop + 72))
            {
                return 3;
            }
            return -1;
        }

        public void OnMouseUp(int X, int Y, bool IsSamePoint)
        {
            if (this.MouseUp != null)
            {
                int gImage = GetImage(X, Y);
                if (gImage > -1)
                {
                    MouseUp(this, gImage);
                }
            }
        }
    }
}
