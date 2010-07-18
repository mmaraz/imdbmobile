using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ImdbMobile.IMDBData;

namespace ImdbMobile.Controls
{
    public partial class RecentSearchControl : ImdbMobile.UI.SlidingList
    {

        public RecentSearchControl()
        {
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0020");
            ShowData();
        }

        private void ShowError(string ErrorMessage)
        {
            UI.KListFunctions.ShowError(ErrorMessage, this.kListControl1);
        }


        private void ShowData()
        {
            foreach (string str in IMDBData.SettingsWrapper.GlobalSettings.RecentSearches)
            {
                UI.ActionButton ab = new ImdbMobile.UI.ActionButton();
                ab.Icon = "Search";
                ab.YIndex = IMDBData.SettingsWrapper.GlobalSettings.RecentSearches.IndexOf(str);
                ab.Text = str;
                ab.Parent = this.kListControl1;
                ab.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ab_MouseUp);
                ab.CalculateHeight();
                this.kListControl1.Items.Add(ab);
            }
        }

        void ab_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            UI.WindowHandler.Home();
            UI.WindowHandler.ParentForm.DoSearch(Sender.Text, false);
        }
    }
}
