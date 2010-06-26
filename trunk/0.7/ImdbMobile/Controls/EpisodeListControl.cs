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
        private static ImdbSeason CurrentSeason;
        private static string ImdbConst;
        private System.Threading.Thread LoadingThread;

        private delegate void AddItem(int YIndex, string Text);
        private delegate void ShowList();

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
                mpi.YIndex = CurrentSeason.Episodes.IndexOf(ie);
                mpi.MainText = ie.Title;
                mpi.SecondaryText = UI.Translations.GetTranslated("0016") + ": " + ie.ReleaseDate.ToShortDateString();
                mpi.ShowSeparator = true;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);
                this.kListControl1.AddItem(mpi);
            }
        }

        void mpi_OnClick(object Sender)
        {
            /*MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            UI.KListFunctions.ShowLoading("Loading Episode Info.\nPlease Wait...", this.kListControl1);
            ImdbConst = CurrentSeason.Episodes[mpi.YIndex].ImdbId;

            System.Threading.Thread t = new System.Threading.Thread(DownloadTitle);
            t.Start();*/
        }

        private void DownloadTitle()
        {
            try
            {
                API a = new API();
                string Response = a.GetFullDetails(ImdbConst);

                JObject data = JObject.Parse(Response);
                if (General.ContainsKey(data, "data"))
                {
                    Search s = new Search();
                    ImdbTitle PartialTitle = s.ParseTitle(data["data"]);
                    MovieControl m = new MovieControl(PartialTitle);
                    UI.WindowHandler.OpenForm(m);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
