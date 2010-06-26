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
    public partial class ExternalReviews : UI.SlidingList
    {
        private ImdbTitle CurrentTitle;
        private delegate void LoadImdbInformation(ImdbTitle title);
        private delegate void ShowErrorInfo(string ErrorMessage);
        private delegate void UpdateStatus(int Current, int Total);

        System.Threading.Thread LoadingThread;

        public ExternalReviews(ImdbTitle title)
        {
            InitializeComponent();

            this.ThreadList.Add(LoadingThread);

            this.CurrentTitle = title;

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0094") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadExternalData);
            LoadingThread.Start();
        }

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0094") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void ShowError(string ErrorMessage)
        {
            try
            {
                UI.KListFunctions.ShowLoading(ErrorMessage, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }

        private void LoadExternalData()
        {
            try
            {
                IMDBData.TitleExternalReviewParser erp = new TitleExternalReviewParser();
                this.CurrentTitle.ExternalReviews = erp.ParseExternalReviews(this.CurrentTitle);

                LoadImdbInformation li = new LoadImdbInformation(SetImdbInformation);
                this.Invoke(li, new object[] { this.CurrentTitle });
            }
            catch (Exception e)
            {
                try
                {
                    ShowErrorInfo si = new ShowErrorInfo(ShowError);
                    this.Invoke(si, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }

        private void SetImdbInformation(ImdbTitle title)
        {
            try
            {
                this.kListControl1.Clear();
                int Counter = 0;
                foreach (ImdbExternalReview ier in this.CurrentTitle.ExternalReviews)
                {
                    MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                    mpi.MainText = ier.Label;
                    mpi.SecondaryText = ier.Author;
                    mpi.YIndex = Counter;
                    mpi.Parent = this.kListControl1;
                    mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);
                    this.kListControl1.AddItem(mpi);
                    Counter++;

                    UpdateStatus us = new UpdateStatus(Update);
                    this.Invoke(us, new object[] { mpi.YIndex + 1, this.CurrentTitle.ExternalReviews.Count });
                }
                this.LoadingList.Visible = false;
            }
            catch (Exception) { }
        }

        void mpi_OnClick(object Sender)
        {
            ImdbExternalReview ier = this.CurrentTitle.ExternalReviews[((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender).YIndex];
            System.Diagnostics.Process.Start(ier.URL, "");
        }
    }
}
