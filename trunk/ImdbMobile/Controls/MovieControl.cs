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
    public partial class MovieControl : ImdbMobile.UI.SlidingList
    {
        private ImdbTitle CurrentTitle;
        private delegate void LoadImdbInformation(ImdbTitle title);
        private delegate void ShowErrorInfo(string ErrorMessage);
        public UI.MovieHeader mh;
        System.Threading.Thread ImageThread;
        System.Threading.Thread LoadingThread;

        public MovieControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0024");

            this.ThreadList.Add(ImageThread);
            this.ThreadList.Add(LoadingThread);

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0088") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);

            LoadingThread = new System.Threading.Thread(LoadMovieData);
            LoadingThread.Start();
        }

        private void ShowError(string ErrorMessage)
        {
            UI.KListFunctions.ShowError(ErrorMessage, this.kListControl1);
        }

        public void SetImdbInformation(ImdbTitle title)
        {
            this.kListControl1.Clear();
            mh = new ImdbMobile.UI.MovieHeader();
            mh.Director = "";
            if (title.Directors.Count > 0)
            {
                mh.Director = title.Directors[0].Name;
            }
            mh.Genres = "";
            foreach (string str in this.CurrentTitle.Genres)
            {
                mh.Genres += str + ", ";
            }
            if (mh.Genres != "")
            {
                mh.Genres = mh.Genres.Trim().TrimEnd(new char[] { ',' });
            }
            mh.MovieTitle = title.Title;
            mh.Certificate = title.Certificate;
            mh.Writer = "";
            if (title.Writers.Count > 0)
            {
                mh.Writer = title.Writers[0].Name;
            }
            mh.YIndex = 0;
            mh.Parent = this.kListControl1;
            double Stars = 0;
            if (CurrentTitle.Rating != null)
            {
                try
                {
                    Stars = double.Parse(CurrentTitle.Rating);
                }
                catch (Exception) { }
            }
            mh.Rating = Stars;
            mh.CalculateHeight();
            this.kListControl1.AddItem(mh);

            DownloadCover dc = new DownloadCover(title.Cover.URL, this);
            ImageThread = new System.Threading.Thread(dc.Download);
            ImageThread.Start();

            UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
            td.Heading = UI.Translations.GetTranslated("0043") + ":";
            td.Text = title.Plot;
            td.YIndex = 1;
            td.Parent = this.kListControl1;
            td.CalculateHeight(this.kListControl1.Width);
            this.kListControl1.AddItem(td);

            UI.ActionButton CastButton = new ImdbMobile.UI.ActionButton();
            CastButton.Icon = global::ImdbMobile.Properties.Resources.Cast;
            CastButton.HoverIcon = global::ImdbMobile.Properties.Resources.Cast_Over;
            CastButton.Parent = this.kListControl1;
            CastButton.Text = UI.Translations.GetTranslated("0012");
            CastButton.YIndex = 2;
            CastButton.CalculateHeight();
            CastButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(CastButton_MouseUp);
            this.kListControl1.AddItem(CastButton);

            UI.ActionButton UserReviewButton = new ImdbMobile.UI.ActionButton();
            UserReviewButton.Icon = global::ImdbMobile.Properties.Resources.UserReview;
            UserReviewButton.HoverIcon = global::ImdbMobile.Properties.Resources.UserReview_Over;
            UserReviewButton.Parent = this.kListControl1;
            UserReviewButton.Text = UI.Translations.GetTranslated("0090");
            UserReviewButton.YIndex = 3;
            UserReviewButton.CalculateHeight();
            UserReviewButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(UserReviewButton_MouseUp);
            this.kListControl1.AddItem(UserReviewButton);

            UI.ActionButton ExternalReviewButton = new ImdbMobile.UI.ActionButton();
            ExternalReviewButton.Icon = global::ImdbMobile.Properties.Resources.ExternalReview;
            ExternalReviewButton.HoverIcon = global::ImdbMobile.Properties.Resources.ExternalReview_Over;
            ExternalReviewButton.Parent = this.kListControl1;
            ExternalReviewButton.Text = UI.Translations.GetTranslated("0091");
            ExternalReviewButton.YIndex = 4;
            ExternalReviewButton.CalculateHeight();
            ExternalReviewButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ExternalReviewButton_MouseUp);
            this.kListControl1.AddItem(ExternalReviewButton);

            UI.ActionButton PhotoButton = new ImdbMobile.UI.ActionButton();
            PhotoButton.Icon = global::ImdbMobile.Properties.Resources.Photos;
            PhotoButton.HoverIcon = global::ImdbMobile.Properties.Resources.Photos_Over;
            PhotoButton.Parent = this.kListControl1;
            PhotoButton.Text = UI.Translations.GetTranslated("0044");
            PhotoButton.YIndex = 5;
            PhotoButton.CalculateHeight();
            PhotoButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(PhotoButton_MouseUp);
            this.kListControl1.AddItem(PhotoButton);

            UI.ActionButton TrailerButton = new ImdbMobile.UI.ActionButton();
            TrailerButton.Icon = global::ImdbMobile.Properties.Resources.Trailers;
            TrailerButton.HoverIcon = global::ImdbMobile.Properties.Resources.Trailers_Over;
            TrailerButton.Parent = this.kListControl1;
            TrailerButton.Text = UI.Translations.GetTranslated("0045");
            TrailerButton.YIndex = 6;
            TrailerButton.CalculateHeight();
            TrailerButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(TrailerButton_MouseUp);
            this.kListControl1.AddItem(TrailerButton);

            UI.ActionButton QuoteButton = new ImdbMobile.UI.ActionButton();
            QuoteButton.Icon = global::ImdbMobile.Properties.Resources.Quote;
            QuoteButton.HoverIcon = global::ImdbMobile.Properties.Resources.Quote_Over;
            QuoteButton.Parent = this.kListControl1;
            QuoteButton.Text = UI.Translations.GetTranslated("0046");
            QuoteButton.YIndex = 7;
            QuoteButton.CalculateHeight();
            QuoteButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(QuoteButton_MouseUp);
            this.kListControl1.AddItem(QuoteButton);

            UI.ActionButton TriviaButton = new ImdbMobile.UI.ActionButton();
            TriviaButton.Icon = global::ImdbMobile.Properties.Resources.Trivia;
            TriviaButton.HoverIcon = global::ImdbMobile.Properties.Resources.Trivia_Over;
            TriviaButton.Parent = this.kListControl1;
            TriviaButton.Text = UI.Translations.GetTranslated("0005");
            TriviaButton.YIndex = 8;
            TriviaButton.CalculateHeight();
            TriviaButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(TriviaButton_MouseUp);
            this.kListControl1.AddItem(TriviaButton);

            UI.ActionButton GoofButton = new ImdbMobile.UI.ActionButton();
            GoofButton.Icon = global::ImdbMobile.Properties.Resources.MoreInfo;
            GoofButton.HoverIcon = global::ImdbMobile.Properties.Resources.MoreInfo_Over;
            GoofButton.Parent = this.kListControl1;
            GoofButton.Text = UI.Translations.GetTranslated("0042");
            GoofButton.YIndex = 9;
            GoofButton.CalculateHeight();
            GoofButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(GoofButton_MouseUp);
            this.kListControl1.AddItem(GoofButton);

            UI.ActionButton ParentalButton = new ImdbMobile.UI.ActionButton();
            ParentalButton.Icon = global::ImdbMobile.Properties.Resources.ParentalGuide;
            ParentalButton.HoverIcon = global::ImdbMobile.Properties.Resources.ParentalGuide_Over;
            ParentalButton.Parent = this.kListControl1;
            ParentalButton.Text = UI.Translations.GetTranslated("0092");
            ParentalButton.YIndex = 10;
            ParentalButton.CalculateHeight();
            ParentalButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ParentalButton_MouseUp);
            this.kListControl1.AddItem(ParentalButton);

            if (CurrentTitle.Type == ImdbTitle.TitleType.TVSeries)
            {
                UI.ActionButton EpisodeButton = new ImdbMobile.UI.ActionButton();
                EpisodeButton.Icon = global::ImdbMobile.Properties.Resources.episodes;
                EpisodeButton.Parent = this.kListControl1;
                EpisodeButton.Text = UI.Translations.GetTranslated("0047");
                EpisodeButton.YIndex = 11;
                EpisodeButton.CalculateHeight();
                EpisodeButton.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(EpisodeButton_MouseUp);
                this.kListControl1.AddItem(EpisodeButton);
            }

        }

        void ParentalButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            ParentalGuide pg = new ParentalGuide(CurrentTitle);
            UI.WindowHandler.OpenForm(pg);
        }

        void ExternalReviewButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            ExternalReviews er = new ExternalReviews(CurrentTitle);
            UI.WindowHandler.OpenForm(er);
        }

        void UserReviewButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            UserReviews ur = new UserReviews(CurrentTitle);
            UI.WindowHandler.OpenForm(ur);
        }

        void EpisodeButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            SeasonListControl sl = new SeasonListControl(CurrentTitle);
            UI.WindowHandler.OpenForm(sl);
        }

        void GoofButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            GoofsControl g = new GoofsControl(CurrentTitle);
            UI.WindowHandler.OpenForm(g);
        }

        void TriviaButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            TriviaControl t = new TriviaControl(CurrentTitle);
            UI.WindowHandler.OpenForm(t);
        }

        void QuoteButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            QuotesControl q = new QuotesControl(CurrentTitle);
            UI.WindowHandler.OpenForm(q);
        }

        void TrailerButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            TrailersControl t = new TrailersControl(CurrentTitle);
            UI.WindowHandler.OpenForm(t);
        }

        void PhotoButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            PhotoViewerControl pv = new PhotoViewerControl(CurrentTitle);
            UI.WindowHandler.OpenForm(pv);
        }

        void CastButton_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            CastControl c = new CastControl(CurrentTitle);
            UI.WindowHandler.OpenForm(c);
        }
        public void LoadMovieData()
        {
            try
            {
                IMDBData.TitleParser tp = new ImdbMobile.IMDBData.TitleParser();
                ImdbTitle title = tp.ParseFullDetails(CurrentTitle);
                if (title != null)
                {
                    LoadImdbInformation li = new LoadImdbInformation(SetImdbInformation);
                    this.Invoke(li, new object[] { title });
                }
            }
            catch (Exception e)
            {
                try
                {
                    ShowErrorInfo se = new ShowErrorInfo(ShowError);
                    this.Invoke(se, new object[] { e.Message });
                }
                catch (ObjectDisposedException) { }
            }
        }
    }
}
