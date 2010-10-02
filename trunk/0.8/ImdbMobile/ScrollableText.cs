using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MichyPrima.ManilaDotNetSDK;
using System.Drawing;
using OpenNETCF.Drawing;

namespace ImdbMobile
{
    class ScrollableText : KListControl.IKListItem
    {
        private Rectangle _bounds = Rectangle.Empty;
        private KListControl _parent = new KListControl();
        private int _yindex = 0;
        private int _height = 25;

        public ScrollableText() { }

        public Rectangle Bounds
        {
            get
            {
                return this.Parent.Bounds;
            }
            set
            {
                this.Parent.Bounds = value;
            }
        }

        public KListControl Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public int YIndex
        {
            get { return _yindex; }
            set { _yindex = value; }
        }

        public int Height
        {
            get { return this.Parent.Height; }
            set { this.Parent.Height = value; }
        }

        public string Text
        {
            get;
            set;
        }

        public Font Font
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public void Render(Graphics g, Rectangle r, bool b)
        {
            GraphicsEx gex = GraphicsEx.FromGraphics(g);
            FontEx fex = new FontEx(this.Font.Name, this.Font.Size, this.Font.Style);
            gex.DrawString(this.Text, fex, this.Color, new Rectangle(0, 0, this.Bounds.Width, (int)gex.MeasureString(this.Text, fex).Height));
            gex.Dispose();
        }

        public void OnMouseUp(int x, int y, bool b)
        {

        }

        public void OnMouseDown(int x, int y, ref bool b)
        {

        }
    }
}
