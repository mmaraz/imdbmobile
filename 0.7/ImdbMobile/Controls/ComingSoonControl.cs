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
    public partial class ComingSoonControl : ImdbMobile.UI.SlidingList
    {

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0013") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            try
            {
                this.LoadingList.Visible = false;
            }
            catch (ObjectDisposedException) { }
        }

        private List<ImdbSearchResult> SearchResults;
        ImageDownloader id = new ImageDownloader();

        public ComingSoonControl()
        {
            InitializeComponent();

            this.ImageDownloaderList.Add(id);

            this.Text = UI.Translations.GetTranslated("0015");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0013") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadImdbInformation();
        }

        private void SetComplete()
        {
            id.DownloadImages(SearchResults, this.kListControl1, this.ParentForm);
        }

        private void SetError(string Message)
        {
            UI.KListFunctions.ShowError(Message, this.kListControl1);
        }

        private void AddPanelItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            this.kListControl1.Items.Add(mpi);
        }

        private void LoadImdbInformation()
        {
            ComingSoonParser csp = new ComingSoonParser();
            csp.Error += new EventHandler(csp_Error);
            csp.ParsingComplete += new EventHandler(csp_ParsingComplete);
            csp.ParseComingSoon();
        }

        void csp_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.LoadingList.Visible = false;
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void csp_ParsingComplete(object sender, EventArgs e)
        {
            ComingSoonParser csp = (ComingSoonParser)sender;
            this.SearchResults = csp.Results;

            foreach (ImdbSearchResult isr in SearchResults)
            {
                ImdbTitle title = (ImdbTitle)isr;

                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = title.Title;
                mpi.SecondaryText = title.ReleaseDate;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                AddPanelItem(mpi);
                int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
                Update(YIndex + 1, SearchResults.Count);
            }

            Clear();
            SetComplete();

            if (SearchResults.Count == 0)
            {
                SetError(UI.Translations.GetTranslated("0014"));
            }
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
