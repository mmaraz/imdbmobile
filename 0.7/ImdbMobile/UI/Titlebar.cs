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

        public Titlebar(System.Windows.Forms.Form ParentForm)
        {
            this.ParentForm = ParentForm;
            sip = new Microsoft.WindowsCE.Forms.InputPanel();
            ParentForm.Resize += new EventHandler(ParentForm_Resize);
        }

        void ParentForm_Resize(object sender, EventArgs e)
        {
            try
            {
                if (Screen.PrimaryScreen.Bounds.Height > 360)
                {
                    this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 148);
                    this.DrawVGA();
                    // (W)VGA Devices
                    holderPanel.Size = new Size(this.ParentForm.Width, 148);

                    this.pbSearch.Image = global::ImdbMobile.Properties.Resources.SearchButton_Large;
                    this.pbSearch.Size = new System.Drawing.Size(33, 32);
                }
                else
                {
                    this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 93);
                    this.DrawQVGA();
                    // (W)QVGA Devices
                    holderPanel.Size = new Size(this.ParentForm.Width, 93);

                    this.pbSearch.Image = global::ImdbMobile.Properties.Resources.SearchButton_Small;
                    this.pbSearch.Size = new System.Drawing.Size(24, 23);
                }
                ParentForm.Invalidate();
                foreach (SlidingList sl in WindowHandler.ControlList)
                {
                    ParentForm.Controls.Remove(sl);
                    sl.Initialise();
                }
            }
            catch (ObjectDisposedException) { }
        }

        void pbSearch_Click(object sender, EventArgs e)
        {
            this.sip.Enabled = false;
            string SearchText = this.textBox1.Text;
            UI.WindowHandler.Home();
            ((Form1)UI.WindowHandler.ParentForm).Titlebar.textBox1.Text = SearchText;
            ((Form1)UI.WindowHandler.ParentForm).DoSearch(SearchText);
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

                this.pbSearch.Image = global::ImdbMobile.Properties.Resources.SearchButton_Large;
                this.pbSearch.Size = new System.Drawing.Size(33, 32);
            }
            else
            {
                this.DrawnBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, 93);
                this.DrawQVGA();
                // (W)QVGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 93);

                this.pbSearch.Image = global::ImdbMobile.Properties.Resources.SearchButton_Small;
                this.pbSearch.Size = new System.Drawing.Size(24, 23);
            }

            holderPanel.Location = new Point(0, 0);
            holderPanel.Paint += new PaintEventHandler(holderPanel_Paint);

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

            ParentForm.Controls.Add(holderPanel);
            ParentForm.Controls.Add(this.pbSearch);
            ParentForm.Controls.Add(this.textBox1);
            this.textBox1.BringToFront();
            this.pbSearch.BringToFront();
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
                Bitmap image = global::ImdbMobile.Properties.Resources.HeadingTile_Large;
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                image = global::ImdbMobile.Properties.Resources.IMDB_Logo_Large;
                int LogoX = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (image.Width / 2);
                DestRect = new Rectangle(LogoX, 5, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                int TextboxY = 5 + image.Height + 5;
                int TextboxX = 5;

                // Draw left edge
                image = global::ImdbMobile.Properties.Resources.TextInput_Large_Left;
                DestRect = new Rectangle(5, TextboxY, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);


                TextboxX = Screen.PrimaryScreen.WorkingArea.Width - 5 - global::ImdbMobile.Properties.Resources.TextInput_Large_Right.Width;
                image = global::ImdbMobile.Properties.Resources.TextInput_Large_Right;
                DestRect = new Rectangle(TextboxX, TextboxY, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                image = global::ImdbMobile.Properties.Resources.TextInput_Large_Center;
                DestRect = new Rectangle(5 + global::ImdbMobile.Properties.Resources.TextInput_Large_Left.Width, TextboxY, TextboxX - global::ImdbMobile.Properties.Resources.TextInput_Large_Left.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                int PBX = Screen.PrimaryScreen.WorkingArea.Width - 5 - global::ImdbMobile.Properties.Resources.TextInput_Large_Right.Width - 15;
                this.pbSearch.Location = new System.Drawing.Point(PBX, 96);

                this.textBox1.Size = new System.Drawing.Size(PBX - 5, 40);
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
                Bitmap image = global::ImdbMobile.Properties.Resources.HeadingTile_Small;
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height - 20, GraphicsUnit.Pixel, TransAtt);

                image = global::ImdbMobile.Properties.Resources.IMDB_Logo_Small;
                int LogoX = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (image.Width / 2);
                DestRect = new Rectangle(LogoX, 2, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                int TextboxY = 2 + image.Height + 2;
                int TextboxX = 5;

                // Draw left edge
                image = global::ImdbMobile.Properties.Resources.TextInput_Small_Left;
                DestRect = new Rectangle(5, TextboxY, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);


                TextboxX = Screen.PrimaryScreen.WorkingArea.Width - 5 - global::ImdbMobile.Properties.Resources.TextInput_Small_Right.Width;
                image = global::ImdbMobile.Properties.Resources.TextInput_Small_Right;
                DestRect = new Rectangle(TextboxX, TextboxY, image.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                image = global::ImdbMobile.Properties.Resources.TextInput_Small_Center;
                DestRect = new Rectangle(5 + global::ImdbMobile.Properties.Resources.TextInput_Small_Left.Width, TextboxY, TextboxX - global::ImdbMobile.Properties.Resources.TextInput_Small_Left.Width, image.Height);
                g.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

                int PBX = Screen.PrimaryScreen.WorkingArea.Width - 5 - global::ImdbMobile.Properties.Resources.TextInput_Small_Right.Width - 15;
                this.pbSearch.Location = new System.Drawing.Point(PBX, 54);

                this.textBox1.Size = new System.Drawing.Size(PBX - this.pbSearch.Width - 2, 32);
                this.textBox1.Location = new System.Drawing.Point(22, 54);
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
            sip.Enabled = true;
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
            sip.Enabled = false;
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
