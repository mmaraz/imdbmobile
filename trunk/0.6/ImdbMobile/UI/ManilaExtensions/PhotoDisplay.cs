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

        public Rectangle Image1Rect { get; set; }
        public Rectangle Image2Rect { get; set; }
        public Rectangle Image3Rect { get; set; }

        public Image Image1
        {
            get { return _image1; }
            set
            {
                _image1 = value;
                Redraw();
            }
        }

        public Image Image2
        {
            get { return _image2; }
            set
            {
                _image2 = value;
                Redraw();
            }
        }

        public Image Image3
        {
            get { return _image3; }
            set
            {
                _image3 = value;
                Redraw();
            }
        }
        
        public int PaddingTop;
        public int PaddingBottom;
        public Color BackgroundColor { get; set; }
        public bool ShowSeparator { get; set; }

        private Bitmap DrawnBitmap;

        public PhotoDisplay()
        {
            PaddingTop = 5;
            PaddingBottom = 5;
        }

        private void Redraw()
        {
            if (DrawnBitmap == null)
            {
                DrawnBitmap = new Bitmap(this.Parent.Width, this.Height);
                using (Graphics g = Graphics.FromImage(DrawnBitmap))
                {
                    g.Clear(Color.LightGoldenrodYellow);
                }
            }
            using (Graphics g = Graphics.FromImage(DrawnBitmap))
            {
                // List width - 5px gap to left and right of image
                int ImageWidth = (this.Parent.Width - 20) / 3;
                int ImageHeight = (this.Parent.Height - 20) / 3;

                if (this.Image1 == null)
                {
                    this.Image1Rect = new Rectangle(5, PaddingTop, ImageWidth, ImageHeight);
                    g.DrawRectangle(new Pen(Color.Black), Image1Rect);
                }
                else
                {
                    g.DrawImage(this.Image1, 5, PaddingTop);
                }
                if (this.Image2 == null)
                {
                    this.Image2Rect = new Rectangle(10 + ImageWidth, PaddingTop, ImageWidth, ImageHeight);
                    g.DrawRectangle(new Pen(Color.Black), this.Image2Rect);
                }
                else
                {
                    g.DrawImage(this.Image2, 10 + ImageWidth, PaddingTop);
                }
                if (this.Image3 == null)
                {
                    this.Image3Rect = new Rectangle(15 + (ImageWidth * 2), PaddingTop, ImageWidth, ImageHeight);
                    g.DrawRectangle(new Pen(Color.Black), this.Image3Rect);
                }
                else
                {
                    g.DrawImage(this.Image3, 15 + (ImageWidth * 2), PaddingTop);
                }
            }
        }

        public void CalculateHeight(int ParentWidth)
        {
            this.Height = 0;
            this.Height += (PaddingBottom + PaddingTop);

            this.Height += ((this.Parent.Height) / 3);
        }

         public void Render(Graphics g, Rectangle Bounds, bool Param)
        {
            if (this.DrawnBitmap != null)
            {
                g.DrawImage(this.DrawnBitmap, 0, Bounds.Y);
            }
            else
            {
                Redraw();
                g.DrawImage(this.DrawnBitmap, 0, Bounds.Y);
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

        private bool IntersectsRect(int X, int Y, Rectangle Rect)
        {
            Y = Y + this.Bounds.Y;
            if (X > Rect.X && X < (Rect.X + Rect.Width))
            {
                if (Y > Rect.Y && Y < (Rect.Y + Rect.Height))
                {
                    return true;
                }
            }
            return false;
        }

        private int GetImage(int X, int Y)
        {
            if (IntersectsRect(X, Y, this.Image1Rect))
            {
                return 1;
            }
            if (IntersectsRect(X, Y, this.Image2Rect))
            {
                return 2;
            }
            if (IntersectsRect(X, Y, this.Image3Rect))
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
