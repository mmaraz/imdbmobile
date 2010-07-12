﻿using System;
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

        public Settings()
        {
            InitializeComponent();

        }

        private void Settings_Load(object sender, EventArgs e)
        {
            UI.Titlebar t = new ImdbMobile.UI.Titlebar(this);
            t.ShowSearch = false;
            t.DrawTitlebar();

            this.ddlVideoPlayer.SelectedIndexChanged += new EventHandler(ddlVideoPlayer_SelectedIndexChanged);

            ImdbLanguages ils = new ImdbLanguages();
            this.ddlImdbLocale.DataSource = ils.SupportedLanguages;
            this.ddlImdbLocale.DisplayMember = "Country";
            this.ddlImdbLocale.ValueMember = "Locale";
            this.ddlImdbLocale.SelectedValue = SettingsWrapper.GlobalSettings.Language.Locale;

            this.ddlUILang.Items.Clear();
            List<object> TranslationList = new List<object>();
            foreach (string str in System.IO.Directory.GetFiles(IMDBData.SettingsWrapper.ApplicationPath + "//Translations//", "*.xml"))
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

            this.ddlUILang.DataSource = TranslationList;
            this.ddlUILang.DisplayMember = "EnglishName";
            this.ddlUILang.ValueMember = "File";
            this.ddlUILang.SelectedValue = SettingsWrapper.GlobalSettings.UILanguage;

            try
            {
                this.ddlUILang.SelectedValue = SettingsWrapper.GlobalSettings.UILanguage;
            }
            catch (Exception)
            {
                this.ddlUILang.SelectedIndex = 0;
            }

            this.ddlVideoPlayer.SelectedIndex = 0;

            this.ddlVideoPlayer.SelectedIndex = 0;
            this.txtCustomVideo.Text = SettingsWrapper.GlobalSettings.VideoPlayerPath;
            this.txtThumbnailCache.Text = SettingsWrapper.GlobalSettings.CachePath;
            this.chkDownloadThumbs.Checked = SettingsWrapper.GlobalSettings.DownloadThumbnails;
            this.chkUseAnimations.Checked = SettingsWrapper.GlobalSettings.UseAnimations;
            this.chkEnableGZip.Checked = SettingsWrapper.GlobalSettings.UseCompression;
        }

        void ddlVideoPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlVideoPlayer.SelectedIndex > 0)
            {
                this.txtCustomVideo.ReadOnly = true;
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
                this.txtCustomVideo.ReadOnly = false;
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

        private void menuItem1_Click(object sender, EventArgs e)
        {
            SettingsWrapper.GlobalSettings.CachePath = this.txtThumbnailCache.Text;
            SettingsWrapper.GlobalSettings.DownloadThumbnails = this.chkDownloadThumbs.Checked;
            ImdbLanguages ils = new ImdbLanguages();
            foreach (ImdbLanguage il in ils.SupportedLanguages)
            {
                if (il.Locale == (string)this.ddlImdbLocale.SelectedValue)
                {
                    SettingsWrapper.GlobalSettings.Language = il;
                    break;
                }
            }
            SettingsWrapper.GlobalSettings.UILanguage = (string)this.ddlUILang.SelectedValue;
            SettingsWrapper.GlobalSettings.VideoPlayerPath = this.txtCustomVideo.Text;
            SettingsWrapper.GlobalSettings.UseAnimations = this.chkUseAnimations.Checked;
            SettingsWrapper.GlobalSettings.UseCompression = this.chkEnableGZip.Checked;
            SettingsWrapper.Save(SettingsWrapper.GlobalSettings);

            this.Close();
        }
    }
}