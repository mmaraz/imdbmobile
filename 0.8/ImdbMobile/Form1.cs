using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ImdbMobile.IMDBData;
using ImdbMobile.Controls;

namespace ImdbMobile
{
    public partial class Form1 : Form
    {
        public UI.Titlebar Titlebar;
        private MainControl Main;

        public Form1()
        {
            InitializeComponent();

            this.menuItem2.Text = UI.Translations.GetTranslated("0006");
            this.menuItem1.Text = UI.Translations.GetTranslated("0007");
            this.Text = UI.Translations.GetTranslated("0032");
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Titlebar = new ImdbMobile.UI.Titlebar(this);
            Titlebar.DrawTitlebar();

            Main = new MainControl();
            UI.WindowHandler.OpenForm(Main);

            if (System.IO.Directory.Exists(@"\Application Data\AppToDate\") || !System.IO.File.Exists(@"\Application Data\AppToDate\IMDbMobile.xml"))
            {
                try
                {
                    string ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                    System.IO.File.Copy(ApplicationPath + "\\IMDbMobile.xml", @"\Application Data\AppToDate\IMDbMobile.xml");
                    System.IO.File.Copy(ApplicationPath + "\\IMDbMobile.ico", @"\Application Data\AppToDate\IMDbMobile.ico");
                }
                catch (Exception ex) { }
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void DoSearch(string Text, bool AddToHistory)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                Main.DoSearch(Text);
                if (AddToHistory)
                {
                    if (SettingsWrapper.GlobalSettings.RecentSearches.Contains(Text))
                    {
                        SettingsWrapper.GlobalSettings.RecentSearches.RemoveAt(SettingsWrapper.GlobalSettings.RecentSearches.IndexOf(Text));
                    }

                    if (SettingsWrapper.GlobalSettings.RecentSearches.Count > 10)
                    {
                        SettingsWrapper.GlobalSettings.RecentSearches.RemoveAt(SettingsWrapper.GlobalSettings.RecentSearches.Count - 1);
                    }
                    SettingsWrapper.GlobalSettings.RecentSearches.Insert(0, Text);
                    SettingsWrapper.Save(SettingsWrapper.GlobalSettings);
                }
            }
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            UI.WindowHandler.Home();
        }

        private void menuItem1_Click_1(object sender, EventArgs e)
        {
            UI.WindowHandler.Back();
        }

        private void kListControl1_Click(object sender, EventArgs e)
        {

        }
    }
}