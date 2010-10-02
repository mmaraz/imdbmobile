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
    public partial class Top250Control : ImdbMobile.UI.SlidingList
    {
        ImageDownloader id = new ImageDownloader();

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0068") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            this.LoadingList.Dispose();
        }

        private List<ImdbSearchResult> SearchResults;

        public Top250Control()
        {
            InitializeComponent();

            this.ImageDownloaderList.Add(id);

            this.Text = UI.Translations.GetTranslated("0028");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0068") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadImdbInformation();
        }

        private void AddPanelItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            this.kListControl1.Items.Add(mpi);
        }

        private void SetError(string Message)
        {
            UI.KListFunctions.ShowError(Message, this.kListControl1);
        }

        private void SetComplete()
        {
            id.DownloadImages(SearchResults, this.kListControl1, this.ParentForm);
        }

        private void LoadImdbInformation()
        {
            Top250Parser t2p = new Top250Parser();
            t2p.Error += new EventHandler(t2p_Error);
            t2p.ParsingComplete += new EventHandler(t2p_ParsingComplete);
            t2p.ParseTop250();
        }

        void t2p_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.LoadingList.Dispose();
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void t2p_ParsingComplete(object sender, EventArgs e)
        {
            Top250Parser t2p = (Top250Parser)sender;
            this.SearchResults = t2p.Results;

            foreach (ImdbSearchResult isr in SearchResults)
            {
                int Index = SearchResults.IndexOf(isr);
                ImdbTitle title = (ImdbTitle)isr;
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = (Index + 1) + ". " + title.Title;
                mpi.SecondaryText = title.Year + " - " + title.Rating + "/10 - " + title.NumberOfVotes + " " + UI.Translations.GetTranslated("0069");
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                AddPanelItem(mpi);
                Update(Index + 1, SearchResults.Count);
            }

            Clear();
            SetComplete();
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ImdbTitle title = (ImdbTitle)SearchResults[YIndex];

            MovieControl m = new MovieControl(title);
            UI.WindowHandler.OpenForm(m);
        }
    }
}
