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
        private delegate void LoadImdbInformation(ImdbTitle title);
        private delegate void ShowErrorInfo(string ErrorMessage);
        private delegate void UpdateStatus(int Current, int Total);
        System.Threading.Thread LoadingThread;

        public UserReviews(ImdbTitle title)
        {
            InitializeComponent();

            this.ThreadList.Add(LoadingThread);

            this.CurrentTitle = title;

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0095") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadUserData);
            LoadingThread.Start();
        }

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0095") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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

        private void LoadUserData()
        {
            try
            {
                IMDBData.TitleUserReviewParser tup = new TitleUserReviewParser();
                this.CurrentTitle.UserReviews = tup.ParseUserReviews(this.CurrentTitle);

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
                foreach (ImdbUserReview iur in title.UserReviews)
                {
                    UI.ReviewDisplay rd = new ImdbMobile.UI.ReviewDisplay();
                    rd.Heading = iur.Summary;
                    rd.Parent = this.kListControl1;
                    rd.Rating = iur.UserRating;
                    rd.Text = iur.Username + "\n" + iur.UserLocation;
                    rd.YIndex = Counter;
                    rd.ShowSeparator = true;
                    rd.MouseUp += new ImdbMobile.UI.ReviewDisplay.MouseEvent(rd_MouseUp);
                    rd.CalculateHeight(this.kListControl1.Width);
                    this.kListControl1.AddItem(rd);
                    Counter++;

                    UpdateStatus us = new UpdateStatus(Update);
                    this.Invoke(us, new object[] { rd.YIndex + 1, title.UserReviews.Count });
                }
                this.LoadingList.Visible = false;
            }
            catch (Exception) { }
        }

        void rd_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ReviewDisplay Sender)
        {
            ViewReview vr = new ViewReview(this.CurrentTitle.UserReviews[Sender.YIndex]);
            UI.WindowHandler.OpenForm(vr);
        }
    }
}
