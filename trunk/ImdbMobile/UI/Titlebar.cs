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
        PictureBox pictureBox1;
        public TextBox textBox1;
        Form ParentForm;
        Microsoft.WindowsCE.Forms.InputPanel sip;

        public Titlebar(System.Windows.Forms.Form ParentForm)
        {
            this.ParentForm = ParentForm;
            sip = new Microsoft.WindowsCE.Forms.InputPanel();
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
            Panel holderPanel = new Panel();
            if (Screen.PrimaryScreen.Bounds.Height > 360)
            {
                // (W)VGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 148);
                holderPanel.Location = new Point(0, 0);
                holderPanel.Paint += new PaintEventHandler(holderPanel_Paint);
            }
            else
            {
                // (W)QVGA Devices
                holderPanel.Size = new Size(this.ParentForm.Width, 93);
            }
            this.textBox1 = new System.Windows.Forms.TextBox();
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

            this.pbSearch = new System.Windows.Forms.PictureBox();
            this.pbSearch.Image = global::ImdbMobile.Properties.Resources.search_btn;
            this.pbSearch.Location = new System.Drawing.Point(421, 91);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(47, 46);
            this.pbSearch.Click += new EventHandler(pbSearch_Click);

            ParentForm.Controls.Add(holderPanel);
            ParentForm.Controls.Add(this.pbSearch);
            ParentForm.Controls.Add(this.textBox1);
            this.textBox1.BringToFront();
            this.pbSearch.BringToFront();
        }

        // Draw (W)VGA
        void holderPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            Color TransparentColor = Color.FromArgb(255, 0, 220);
            System.Drawing.Imaging.ImageAttributes TransAtt = new System.Drawing.Imaging.ImageAttributes();
            TransAtt.SetColorKey(TransparentColor, TransparentColor);

            Rectangle DestRect = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width, 148);
            Bitmap image = global::ImdbMobile.Properties.Resources.HeadingTile_Large;
            e.Graphics.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

            image = global::ImdbMobile.Properties.Resources.IMDB_Logo_Large;
            int LogoX = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (image.Width / 2);
            DestRect = new Rectangle(LogoX, 5, image.Width, image.Height);
            e.Graphics.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

            int TextboxY = 5 + image.Height + 5;
            int TextboxX = 5;

            // Draw left edge
            image = global::ImdbMobile.Properties.Resources.TextInput_Large_Left;
            DestRect = new Rectangle(5, TextboxY, image.Width, image.Height);
            e.Graphics.DrawImage(image, DestRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, TransAtt);

            image = global::ImdbMobile.Properties.Resources.TextInput_Large_Right;
            DestRect = new Rectangle(TextboxX, TextboxY, image.Width, image.Height);
            TextboxX = Screen.PrimaryScreen.WorkingArea.Width - 5 - global::ImdbMobile.Properties.Resources.TextInput_Large_Right.Width;
            e.Graphics.DrawImage(global::ImdbMobile.Properties.Resources.TextInput_Large_Right, TextboxX, TextboxY);

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
