using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ImdbMobile.IMDBData;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.Controls
{
    public partial class EpisodeListControl : ImdbMobile.UI.SlidingList
    {
        private ImdbSeason CurrentSeason;

        public EpisodeListControl(ImdbSeason season)
        {
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0017");

            CurrentSeason = season;
            ShowEpisodes();
        }

        private void ShowEpisodes()
        {
            foreach (ImdbEpisode ie in CurrentSeason.Episodes)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = ie.Title;
                if (ie.ReleaseDate == null)
                {
                    mpi.SecondaryText = UI.Translations.GetTranslated("0016") + ": Unknown";
                }
                else
                {
                    mpi.SecondaryText = UI.Translations.GetTranslated("0016") + ": " + ie.ReleaseDate.Value.ToShortDateString();
                }
                mpi.ShowSeparator = true;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);
                this.kListControl1.Items.Add(mpi);
            }
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            int EpisodeIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ImdbTitle it = new ImdbTitle();
            ImdbEpisode Episode = this.CurrentSeason.Episodes[EpisodeIndex];
            it.Title = this.CurrentSeason.ShowTitle + "\n";
            it.Title += this.CurrentSeason.Label + ", Episode " + (EpisodeIndex + 1) + "\n";
            it.Title += "\"" + Episode.Title + "\"";
            it.ImdbId = Episode.ImdbId;
            UI.WindowHandler.OpenForm(new Controls.MovieControl(it));
        }
    }
}
