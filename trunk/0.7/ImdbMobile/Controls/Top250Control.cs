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
        private delegate void AddItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi);
        private delegate void ShowError(string Message);
        private delegate void ShowComplete();
        ImageDownloader id = new ImageDownloader();
        System.Threading.Thread LoadingThread;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0068") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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

        public Top250Control()
        {
            InitializeComponent();

            this.ImageDownloaderList.Add(id);
            this.ThreadList.Add(LoadingThread);

            this.Text = UI.Translations.GetTranslated("0028");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0068") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        private void AddPanelItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            try
            {
                this.kListControl1.AddItem(mpi);
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

        private void SetComplete()
        {
            try
            {
                //this.lblStatus.Visible = false;
                id.DownloadImages(SearchResults, this.kListControl1, this.ParentForm);
            }
            catch (ObjectDisposedException) { }
        }

        private void LoadImdbInformation()
        {
            try
            {
                Top250Parser t2p = new Top250Parser();
                SearchResults = t2p.ParseTop250();

                
                foreach (ImdbSearchResult isr in SearchResults)
                {
                    int Index = SearchResults.IndexOf(isr);
                    ImdbTitle title = (ImdbTitle)isr;
                    MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                    mpi.MainText = (Index + 1) + ". " + title.Title;
                    mpi.SecondaryText = title.Year + " - " + title.Rating + "/10 - " + title.NumberOfVotes + " " + UI.Translations.GetTranslated("0069");
                    mpi.YIndex = Index;
                    mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                    try
                    {
                        AddItem ai = new AddItem(AddPanelItem);
                        this.Invoke(ai, new object[] { mpi });

                        UpdateStatus us = new UpdateStatus(Update);
                        this.Invoke(us, new object[] { mpi.YIndex + 1, SearchResults.Count });
                    }
                    catch (ObjectDisposedException) { }
                }

                try
                {
                    ClearList cl = new ClearList(Clear);
                    this.Invoke(cl);

                    ShowComplete sc = new ShowComplete(SetComplete);
                    this.Invoke(sc);
                }
                catch (ObjectDisposedException) { }
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
