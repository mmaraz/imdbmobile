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
    public partial class UserReviews : UI.SlidingList
    {
        private ImdbTitle CurrentTitle;

        public UserReviews(ImdbTitle title)
        {
            InitializeComponent();

            this.CurrentTitle = title;

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0095") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadUserData();
        }

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0095") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void ShowError(string ErrorMessage)
        {
            UI.KListFunctions.ShowLoading(ErrorMessage, this.kListControl1);
        }

        private void LoadUserData()
        {
            if (this.CurrentTitle.UserReviews != null && this.CurrentTitle.UserReviews.Count > 0)
            {
                SetImdbInformation(this.CurrentTitle);
            }
            else
            {
                TitleUserReviewParser tup = new TitleUserReviewParser();
                tup.Error += new EventHandler(tup_Error);
                tup.ParsingComplete += new EventHandler(tup_ParsingComplete);
                tup.ParseUserReviews(this.CurrentTitle);
            }
        }

        void tup_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.LoadingList.Visible = false;
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void tup_ParsingComplete(object sender, EventArgs e)
        {
            TitleUserReviewParser tup = (TitleUserReviewParser)sender;
            SetImdbInformation(tup.Title);
        }

        private void SetImdbInformation(ImdbTitle title)
        {
            this.kListControl1.Items.Clear();

            if (title.UserReviews.Count == 0)
            {
                ShowError(UI.Translations.GetTranslated("0098"));
                return;
            }

            List<UI.ReviewDisplay> rdList = new List<ImdbMobile.UI.ReviewDisplay>();
            foreach (ImdbUserReview iur in title.UserReviews)
            {
                UI.ReviewDisplay rd = new ImdbMobile.UI.ReviewDisplay();
                rd.Heading = iur.Summary;
                rd.Parent = this.kListControl1;
                rd.Rating = iur.UserRating;
                rd.Text = iur.Username + "\n" + iur.UserLocation;
                rd.YIndex = title.UserReviews.IndexOf(iur);
                rd.ShowSeparator = true;
                rd.MouseUp += new ImdbMobile.UI.ReviewDisplay.MouseEvent(rd_MouseUp);
                rd.CalculateHeight(this.kListControl1.Width);
                rdList.Add(rd);                
            }
            foreach (UI.ReviewDisplay rd in rdList)
            {
                this.kListControl1.Items.Add(rd);
                Update(rd.YIndex + 1, title.UserReviews.Count);
            }
            

            this.LoadingList.Visible = false;
        }

        void rd_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ReviewDisplay Sender)
        {
            ViewReview vr = new ViewReview(this.CurrentTitle.UserReviews[Sender.YIndex]);
            UI.WindowHandler.OpenForm(vr);
        }
    }
}
