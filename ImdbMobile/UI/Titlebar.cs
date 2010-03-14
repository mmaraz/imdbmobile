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

            this.pictureBox1 = new PictureBox();
            this.pictureBox1.Image = global::ImdbMobile.Properties.Resources.heading;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(480, 150);


            ParentForm.Controls.Add(pictureBox1);
            ParentForm.Controls.Add(this.pbSearch);
            ParentForm.Controls.Add(this.textBox1);
            this.textBox1.BringToFront();
            this.pbSearch.BringToFront();
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
