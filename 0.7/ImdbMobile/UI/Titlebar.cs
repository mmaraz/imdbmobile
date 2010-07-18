using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ImdbMobile.IMDBData;
using System.Windows.Forms;
using System.Drawing;

namespace ImdbMobile.UI
{
    public class Titlebar
    {
        PictureBox pbSearch;
        Panel holderPanel;
        public TextBox textBox1;
        Form ParentForm;
        Microsoft.WindowsCE.Forms.InputPanel sip;
        Bitmap DrawnBitmap;
        public bool ShowSearch { get; set; }

        public Titlebar(System.Windows.Forms.Form ParentForm)
        {
            this.ParentForm = ParentForm;
            sip = new Microsoft.WindowsCE.Forms.InputPanel();
            ParentForm.Resize += new EventHandler(ParentForm_Resize);
            this.ShowSearch = true;
        }

        void ParentForm_Resize(object sender, EventArgs e)
        {
            if (Screen.PrimaryScreen.Bounds.Height > 360)
            {
                this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 148);
                this.DrawVGA();
                // (W)VGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 148);

                if (this.ShowSearch)
                {
                    this.pbSearch.Image = Extensions.LoadBitmap("SearchButton_Large");
                    this.pbSearch.Size = new System.Drawing.Size(33, 32);
                }
            }
            else
            {
                this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 93);
                this.DrawQVGA();
                // (W)QVGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 93);

                if (this.ShowSearch)
                {
                    this.pbSearch.Image = Extensions.LoadBitmap("SearchButton_Small");
                    this.pbSearch.Size = new System.Drawing.Size(24, 23);
                }
            }
            ParentForm.Invalidate();
            foreach (SlidingList sl in WindowHandler.ControlList)
            {
                ParentForm.Controls.Remove(sl);
                sl.Initialise();
            }
        }

        void pbSearch_Click(object sender, EventArgs e)
        {
            this.sip.Enabled = false;
            string SearchText = this.textBox1.Text;
            UI.WindowHandler.Home();
            ((Form1)UI.WindowHandler.ParentForm).Titlebar.textBox1.Text = SearchText;
            ((Form1)UI.WindowHandler.ParentForm).DoSearch(SearchText, true);
        }

        public void DrawTitlebar()
        {
            holderPanel = new Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();

            this.pbSearch = new System.Windows.Forms.PictureBox();
            if (Screen.PrimaryScreen.Bounds.Height > 360)
            {
                this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 148);
                this.DrawVGA();
                // (W)VGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 148);

                if (this.ShowSearch)
                {
                    this.pbSearch.Image = Extensions.LoadBitmap("SearchButton_Large");
                    this.pbSearch.Size = new System.Drawing.Size(33, 32);
                }
            }
            else
            {
                this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 93);
                this.DrawQVGA();
                // (W)QVGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 93);

                if (this.ShowSearch)
                {
                    this.pbSearch.Image = Extensions.LoadBitmap("SearchButton_Small");
                    this.pbSearch.Size = new System.Drawing.Size(24, 23);
                }
            }

            holderPanel.Location = new Point(0, 0);
            holderPanel.Paint += new PaintEventHandler(holderPanel_Paint);
            ParentForm.Controls.Add(holderPanel);

            if (this.ShowSearch)
            {
                this.pbSearch.Name = "pbSearch";
                this.pbSearch.Click += new EventHandler(pbSearch_Click);

                this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.textBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Italic);
                this.textBox1.ForeColor = System.Drawing.Color.LightGray;
                this.textBox1.Location = new System.Drawing.Point(27, 97);
                this.textBox1.Name = "textBox1";
                this.textBox1.Size = new System.Drawing.Size(388, 40);
                this.textBox1.TabIndex = 11;
                this.textBox1.Text = UI.Translations.GetTranslated("0080") + "...";
                this.textBox1.GotFocus += new System.EventHandler(this.textBox1_GotFocus);
                this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
                this.textBox1.LostFocus += new System.EventHandler(this.textBox1_LostFocus);

                ParentForm.Controls.Add(this.pbSearch);
                ParentForm.Controls.Add(this.textBox1);
                this.textBox1.BringToFront();
                this.pbSearch.BringToFront();
            }
        }

        private void DrawVGA()
        {
            using (Graphics g = Graphics.FromImage(this.DrawnBitmap))
            {
                g.Clear(Color.Black);
                
                Color TransparentColor = Color.FromArgb(255, 0, 220);
                System.Drawing.Imaging.ImageAttributes TransAtt = new System.Drawing.Imaging.ImageAttributes();
                TransAtt.SetColorKey(TransparentColor, TransparentColor);

                Rectangle DestRect = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width, 148);
                Color start = Color.FromArgb(52, 60, 67);
                Color stop = Color.FromArgb(27, 31, 29);
                GradientFill.Fill(g, DestRect, start, stop, GradientFill.FillDirection.TopToBottom);

                Size LogoSize = Extensions.GetBitmapDimensions("IMDB_Logo_Large");
                int LogoX = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (LogoSize.Width / 2);
                DestRect = new Rectangle(LogoX, 5, LogoSize.Width, LogoSize.Height);
                Extensions.DrawBitmap(g, DestRect, "IMDB_Logo_Large");
                
                /*Bitmap image = global::ImdbMobile.Properties.Resources.IMDB_Logo_Large;
                int LogoX = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (image.Width / 2);
                DestRect = new Rectangle(LogoX, 5, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);*/

                if (this.ShowSearch)
                {
                    int TextboxY = 5 + LogoSize.Height + 5;
                    int TextboxX = 5;

                    // Draw left edge
                    Size s = Extensions.GetBitmapDimensions("TextInput_Large_Left");
                    DestRect = new Rectangle(5, TextboxY, s.Width, s.Height);
                    Extensions.DrawBitmap(g, DestRect, "TextInput_Large_Left");

                    s = Extensions.GetBitmapDimensions("TextInput_Large_Right");
                    TextboxX = Screen.PrimaryScreen.WorkingArea.Width - 5 - s.Width;
                    DestRect = new Rectangle(TextboxX, TextboxY, s.Width, s.Height);
                    Extensions.DrawBitmap(g, DestRect, "TextInput_Large_Right");

                    s = Extensions.GetBitmapDimensions("TextInput_Large_Center");
                    DestRect = new Rectangle(5 + s.Width, TextboxY, TextboxX - s.Width, s.Height);
                    Extensions.DrawBitmap(g, DestRect, "TextInput_Large_Center");

                    s = Extensions.GetBitmapDimensions("TextInput_Large_Right");
                    int PBX = Screen.PrimaryScreen.WorkingArea.Width - 5 - s.Width - 15;
                    this.pbSearch.Location = new System.Drawing.Point(PBX, 96);

                    this.textBox1.Size = new System.Drawing.Size(PBX - 5, 40);
                }
            }
        }

        private void DrawQVGA()
        {
            using (Graphics g = Graphics.FromImage(this.DrawnBitmap))
            {
                g.Clear(Color.Black);
                Color TransparentColor = Color.FromArgb(255, 0, 220);
                System.Drawing.Imaging.ImageAttributes TransAtt = new System.Drawing.Imaging.ImageAttributes();
                TransAtt.SetColorKey(TransparentColor, TransparentColor);

                Rectangle DestRect = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width, 93);
                Color start = Color.FromArgb(52, 60, 67);
                Color stop = Color.FromArgb(27, 31, 29);
                GradientFill.Fill(g, DestRect, start, stop, GradientFill.FillDirection.TopToBottom);

                Size s = Extensions.GetBitmapDimensions("IMDB_Logo_Small");
                int LogoX = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (s.Width / 2);
                DestRect = new Rectangle(LogoX, 2, s.Width, s.Height);
                Extensions.DrawBitmap(g, DestRect, "IMDB_Logo_Small");

                if (this.ShowSearch)
                {
                    int TextboxY = 2 + s.Height + 2;
                    int TextboxX = 5;


                    // Draw left edge
                    Size sLeft = Extensions.GetBitmapDimensions("TextInput_Small_Left");
                    DestRect = new Rectangle(5, TextboxY, s.Width, s.Height);
                    Extensions.DrawBitmap(g, DestRect, "TextInput_Small_Left");


                    Size sRight = Extensions.GetBitmapDimensions("TextInput_Small_Right");
                    DestRect = new Rectangle(TextboxX, TextboxY, s.Width, s.Height);
                    Extensions.DrawBitmap(g, DestRect, "TextInput_Small_Right");

                    Size sCenter = Extensions.GetBitmapDimensions("TextInput_Small_Center");
                    DestRect = new Rectangle(5 + sLeft.Width, TextboxY, TextboxX - sLeft.Width, sCenter.Height);
                    Extensions.DrawBitmap(g, DestRect, "TextInput_Small_Center");

                    int PBX = Screen.PrimaryScreen.WorkingArea.Width - 5 - sRight.Width - 15;
                    this.pbSearch.Location = new System.Drawing.Point(PBX, 54);

                    this.textBox1.Size = new System.Drawing.Size(PBX - this.pbSearch.Width - 2, 32);
                    this.textBox1.Location = new System.Drawing.Point(22, 54);
                }
            }
        }

        void holderPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.DrawnBitmap, 0, 0);
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            Font focus = new Font("Arial", 8f, FontStyle.Regular);
            if (this.textBox1.Text == UI.Translations.GetTranslated("0080") + "...")
            {
                this.textBox1.Font = focus;
                this.textBox1.ForeColor = Color.Black;
                this.textBox1.Text = "";
            }
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            Font original = new Font("Arial", 8f, FontStyle.Italic);
            if (this.textBox1.Text == "")
            {
                this.textBox1.Text = UI.Translations.GetTranslated("0080") + "...";
                this.textBox1.Font = original;
                this.textBox1.ForeColor = Color.LightGray;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pbSearch_Click(null, null);
            }
        }
    }
}
