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
        private delegate void ShowComplete();
        private delegate void ShowError(string Message);
        private delegate void AddItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi);
        System.Threading.Thread LoadingThread;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0013") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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

        private static List<ImdbSearchResult> SearchResults;
        ImageDownloader id = new ImageDownloader();

        public ComingSoonControl()
        {
            InitializeComponent();

            this.ImageDownloaderList.Add(id);
            this.ThreadList.Add(LoadingThread);

            this.Text = UI.Translations.GetTranslated("0015");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0013") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        private void SetComplete()
        {
            try
            {
                id.DownloadImages(SearchResults, this.kListControl1, this.ParentForm);
            }
            catch (ObjectDisposedException) { }
        }

        private void SetError(string Message)
        {
            try
            {
                UI.KListFunctions.ShowError(Message, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }

        private void AddPanelItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            try
            {
                this.kListControl1.AddItem(mpi);
            }
            catch (ObjectDisposedException) { }
        }

        private void LoadImdbInformation()
        {
            try
            {
                ComingSoonParser csp = new ComingSoonParser();
                SearchResults = csp.ParseComingSoon();
                
                foreach (ImdbSearchResult isr in SearchResults)
                {
                    ImdbTitle title = (ImdbTitle)isr;

                    MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                    mpi.YIndex = SearchResults.IndexOf(isr);
                    mpi.MainText = title.Title;
                    mpi.SecondaryText = title.ReleaseDate;
                    mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                    AddItem ai = new AddItem(AddPanelItem);
                    this.Invoke(ai, new object[] { mpi });

                    UpdateStatus us = new UpdateStatus(Update);
                    this.Invoke(us, new object[] { mpi.YIndex+1, SearchResults.Count });
                }

                ClearList cl = new ClearList(Clear);
                this.Invoke(cl);

                ShowComplete sc = new ShowComplete(SetComplete);
                this.Invoke(sc);

                if (SearchResults.Count == 0)
                {
                    try
                    {
                        ShowError sr = new ShowError(SetError);
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0014") });
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception e)
            {
                try
                {
                    ShowError se = new ShowError(SetError);
                    this.Invoke(se, new object[] { e.Message });
                }
                catch (ObjectDisposedException) { }
            }
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            ImdbTitle title = (ImdbTitle)SearchResults[mpi.YIndex];
            MovieControl m = new MovieControl(title);
            UI.WindowHandler.OpenForm(m);
        }
    }
}
