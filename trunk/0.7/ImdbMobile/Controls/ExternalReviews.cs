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

        public ExternalReviews(ImdbTitle title)
        {
            InitializeComponent();

            this.CurrentTitle = title;

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0094") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadExternalData();
        }

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0094") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void ShowError(string ErrorMessage)
        {
            UI.KListFunctions.ShowLoading(ErrorMessage, this.kListControl1);
        }

        private void LoadExternalData()
        {
            if (this.CurrentTitle.ExternalReviews != null && this.CurrentTitle.ExternalReviews.Count > 0)
            {
                SetImdbInformation(this.CurrentTitle);
            }
            else
            {
                IMDBData.TitleExternalReviewParser erp = new TitleExternalReviewParser();
                erp.ParsingComplete += new EventHandler(erp_ParsingComplete);
                erp.ParseExternalReviews(this.CurrentTitle);
            }
        }

        void erp_ParsingComplete(object sender, EventArgs e)
        {
            TitleExternalReviewParser erp = (TitleExternalReviewParser)sender;
            SetImdbInformation(erp.Title);
        }

        private void SetImdbInformation(ImdbTitle title)
        {
            this.kListControl1.Items.Clear();
            int Counter = 0;
            foreach (ImdbExternalReview ier in this.CurrentTitle.ExternalReviews)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = ier.Label;
                mpi.SecondaryText = ier.Author;
                mpi.Parent = this.kListControl1;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);
                this.kListControl1.Items.Add(mpi);
                Counter++;

                Update(this.CurrentTitle.ExternalReviews.IndexOf(ier) + 1, this.CurrentTitle.ExternalReviews.Count);
            }
            this.LoadingList.Visible = false;
        }

        void mpi_OnClick(object Sender)
        {

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = ((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender);
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ImdbExternalReview ier = this.CurrentTitle.ExternalReviews[YIndex];
            System.Diagnostics.Process.Start(ier.URL, "");
        }
    }
}
