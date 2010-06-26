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

            this.menuItem1.Text = UI.Translations.GetTranslated("0060");
            this.menuItem2.Text = UI.Translations.GetTranslated("0061");
            this.label2.Text = UI.Translations.GetTranslated("0062") + ":";
            this.chkThumbnails.Text = UI.Translations.GetTranslated("0063");
            this.lblVideoPlayer.Text = UI.Translations.GetTranslated("0064") + ":";
            this.ddlVideoPlayer.Items.Insert(0, UI.Translations.GetTranslated("0065"));
            this.lblCustomVideo.Text = UI.Translations.GetTranslated("0065") + ":";
            this.lblCustomArgs.Text = UI.Translations.GetTranslated("0066") + ":";
            this.lblCachePath.Text = UI.Translations.GetTranslated("0067") + ":";
            this.Text = UI.Translations.GetTranslated("0030");
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            s = SettingsWrapper.GlobalSettings;
            ImdbLanguages ils = new ImdbLanguages();
            this.ddlLanguage.DataSource = ils.SupportedLanguages;
            this.ddlLanguage.DisplayMember = "Country";
            this.ddlLanguage.ValueMember = "Locale";

            this.comboBox1.Items.Clear();
            List<object> TranslationList = new List<object>();
            foreach(string str in System.IO.Directory.GetFiles(IMDBData.SettingsWrapper.ApplicationPath + "//Translations//", "*.xml"))
            {
                try
                {
                    System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                    xd.Load(str);
                    var TranslationInfo = new { EnglishName = xd.SelectSingleNode("/Translation/Name").Attributes["English"].InnerText, File = IMDBData.SettingsWrapper.ApplicationPath + "//Translations//" + System.IO.Path.GetFileName(str) };
                    TranslationList.Add(TranslationInfo);
                }
                catch (Exception) { }
            }
            this.comboBox1.DataSource = TranslationList;
            this.comboBox1.DisplayMember = "EnglishName";
            this.comboBox1.ValueMember = "File";
            this.comboBox1.SelectedValue = SettingsWrapper.GlobalSettings.UILanguage;

            try
            {
                this.comboBox1.SelectedValue = SettingsWrapper.GlobalSettings.UILanguage;
            }
            catch (Exception)
            {
                this.comboBox1.SelectedIndex = 0;
            }

            this.ddlVideoPlayer.SelectedIndex = 0;

            this.ddlVideoPlayer.SelectedIndex = 0;
            this.txtCustomVideo.Text = SettingsWrapper.GlobalSettings.VideoPlayerPath;
            this.txtCustomArgs.Text = SettingsWrapper.GlobalSettings.VideoPlayerArguments;
            this.txtCachePath.Text = SettingsWrapper.GlobalSettings.CachePath;
            this.chkThumbnails.Checked = SettingsWrapper.GlobalSettings.DownloadThumbnails;
            this.chkEnableAnimate.Checked = SettingsWrapper.GlobalSettings.UseAnimations;
        }

        private void Settings_Closing(object sender, CancelEventArgs e)
        {
            if (!Cancel)
            {
                if (this.ddlVideoPlayer.SelectedIndex == 0 && this.txtCustomVideo.Text == "")
                {
                    if (MessageBox.Show(UI.Translations.GetTranslated("0057"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                }
                else if (this.ddlVideoPlayer.SelectedIndex == 0 && !System.IO.File.Exists(this.txtCustomVideo.Text))
                {
                    if (MessageBox.Show(UI.Translations.GetTranslated("0008"), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
                SettingsWrapper.GlobalSettings.CachePath = this.txtCachePath.Text;
                SettingsWrapper.GlobalSettings.DownloadThumbnails = this.chkThumbnails.Checked;
                ImdbLanguages ils = new ImdbLanguages();
                foreach (ImdbLanguage il in ils.SupportedLanguages)
                {
                    if (il.Locale == (string)this.ddlLanguage.SelectedValue)
                    {
                        SettingsWrapper.GlobalSettings.Language = il;
                        break;
                    }
                }
                SettingsWrapper.GlobalSettings.UILanguage = (string)this.comboBox1.SelectedValue;
                SettingsWrapper.GlobalSettings.VideoPlayerPath = this.txtCustomVideo.Text;
                SettingsWrapper.GlobalSettings.VideoPlayerArguments = this.txtCustomArgs.Text;
                SettingsWrapper.GlobalSettings.UseAnimations = this.chkEnableAnimate.Checked;
                SettingsWrapper.Save(SettingsWrapper.GlobalSettings);
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ddlVideoPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlVideoPlayer.SelectedIndex > 0)
            {
                this.txtCustomArgs.Enabled = false;
                this.txtCustomVideo.Enabled = false;
                string StartupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string[] Splitted = StartupPath.Split('\\');
                string WindowsPath = Splitted[1];
                switch (this.ddlVideoPlayer.SelectedIndex)
                {
                    case 1: this.txtCustomVideo.Text = "\\" + WindowsPath + "\\wmplayer.exe"; break;
                    case 2: this.txtCustomVideo.Text = "\\" + WindowsPath + "\\StreamingPlayer.exe"; break;
                    case 3: if (CorePlayerPath() != null) { this.txtCustomVideo.Text = CorePlayerPath(); } break;
                }
            }
            else
            {
                this.txtCustomArgs.Enabled = true;
                this.txtCustomVideo.Enabled = true;
            }
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