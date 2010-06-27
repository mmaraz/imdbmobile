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
    public partial class SeasonListControl : ImdbMobile.UI.SlidingList
    {
        private ImdbTitle CurrentTitle;

        public SeasonListControl(ImdbTitle title)
        {
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0056");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0055") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
            CurrentTitle = title;

            LoadImdbInformation();
        }

        private void ShowComplete()
        {
            this.LoadingList.Visible = false;
        }

        private void Add(int YIndex, string Label)
        {
            UI.ActionButton ab = new ImdbMobile.UI.ActionButton();
            ab.Text = Label;
            ab.Parent = this.kListControl1;
            ab.YIndex = YIndex;
            ab.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ab_MouseUp);
            ab.CalculateHeight();
            this.kListControl1.Items.Add(ab);
        }

        private void LoadImdbInformation()
        {
            if (this.CurrentTitle.Seasons != null && this.CurrentTitle.Seasons.Count > 0)
            {
                foreach (ImdbSeason isea in this.CurrentTitle.Seasons)
                {
                    Add(CurrentTitle.Seasons.IndexOf(isea), isea.Label);
                }
            }
            else
            {
                TitleEpisodeParser tep = new TitleEpisodeParser();
                tep.ParsingComplete += new EventHandler(tep_ParsingComplete);
                tep.ParseTitleEpisodes(CurrentTitle);
            }
        }

        void tep_ParsingComplete(object sender, EventArgs e)
        {
            TitleEpisodeParser tep = (TitleEpisodeParser)sender;
            foreach (ImdbSeason isea in tep.Title.Seasons)
            {
                Add(CurrentTitle.Seasons.IndexOf(isea), isea.Label);
            }
        }

        void ab_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            EpisodeListControl el = new EpisodeListControl(CurrentTitle.Seasons[Sender.YIndex]);
            UI.WindowHandler.OpenForm(el);
        }
    }
}
