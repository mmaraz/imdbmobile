using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ImdbMobile.IMDBData;

namespace ImdbMobile
{
    public partial class Settings : Form
    {
        ImdbMobile.IMDBData.Settings s;
        private bool Cancel;

        public Settings()
        {
            InitializeComponent();

        }

        private void Settings_Load(object sender, EventArgs e)
        {
            
        }

        

        private string CorePlayerPath()
        {
            if (System.IO.File.Exists("\\Program Files\\CorePlayer\\player.exe"))
            {
                return "\\Program Files\\CorePlayer\\player.exe";
            }
            else if (System.IO.File.Exists("\\Storage Card\\Program Files\\CorePlayer\\player.exe"))
            {
                return "\\Storage Card\\Program Files\\CorePlayer\\player.exe";
            }
            else
            {
                MessageBox.Show(UI.Translations.GetTranslated("0059"));
                this.ddlVideoPlayer.SelectedIndex = 0;
                return null;
            }
        }
    }
}