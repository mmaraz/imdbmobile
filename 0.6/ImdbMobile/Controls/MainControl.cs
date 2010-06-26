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
    public partial class MainControl : ImdbMobile.UI.SlidingList
    {

        private delegate void ResultsDelegate(List<ImdbSearchResult> isrList);
        private delegate void ErrorDelegate();
        private static string SearchQuery { get; set; }
        private List<ImdbSearchResult> CurrentResults;
        System.Threading.Thread SearchThread;
        ImageDownloader id = new ImageDownloader();

        public MainControl()
        {
            InitializeComponent();

            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            this.kListControl1.Clear();

            UI.ActionButton Top250 = new ImdbMobile.UI.ActionButton();
            Top250.Icon = global::ImdbMobile.Properties.Resources.MoreInfo;
            Top250.HoverIcon = global::ImdbMobile.Properties.Resources.MoreInfo_Over;
            Top250.Parent = this.kListControl1;
            Top250.Text = UI.Translations.GetTranslated("0028");
            Top250.YIndex = 0;
            Top250.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Top250_MouseUp);
            Top250.CalculateHeight();
            this.kListControl1.AddItem(Top250);

            UI.ActionButton ComingSoon = new ImdbMobile.UI.ActionButton();
            ComingSoon.Icon = global::ImdbMobile.Properties.Resources.ComingSoon;
            ComingSoon.HoverIcon = global::ImdbMobile.Properties.Resources.ComingSoon_Over;
            ComingSoon.Parent = this.kListControl1;
            ComingSoon.Text = UI.Translations.GetTranslated("0029");
            ComingSoon.YIndex = 1;
            ComingSoon.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ComingSoon_MouseUp);
            ComingSoon.CalculateHeight();
            this.kListControl1.AddItem(ComingSoon);

            UI.ActionButton Settings = new ImdbMobile.UI.ActionButton();
            Settings.Icon = global::ImdbMobile.Properties.Resources.Settings;
            Settings.HoverIcon = global::ImdbMobile.Properties.Resources.Settings_Over;
            Settings.Parent = this.kListControl1;
            Settings.Text = UI.Translations.GetTranslated("0030");
            Settings.YIndex = 2;
            Settings.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Settings_MouseUp);
            Settings.CalculateHeight();
            this.kListControl1.AddItem(Settings);

            UI.ActionButton About = new ImdbMobile.UI.ActionButton();
            About.Icon = global::ImdbMobile.Properties.Resources.about;
            About.HoverIcon = global::ImdbMobile.Properties.Resources.about_over;
            About.Parent = this.kListControl1;
            About.Text = UI.Translations.GetTranslated("0089");
            About.YIndex = 3;
            About.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(About_MouseUp);
            About.CalculateHeight();
            this.kListControl1.AddItem(About);

            UI.ActionButton Exit = new ImdbMobile.UI.ActionButton();
            Exit.Icon = global::ImdbMobile.Properties.Resources.Close;
            Exit.HoverIcon = global::ImdbMobile.Properties.Resources.Close_Over;
            Exit.Parent = this.kListControl1;
            Exit.Text = UI.Translations.GetTranslated("0031");
            Exit.YIndex = 4;
            Exit.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Exit_MouseUp);
            Exit.CalculateHeight();
            this.kListControl1.AddItem(Exit);

            this.kListControl1.Visible = true;
        }

        void Exit_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            Application.Exit();
        }

        void Settings_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            Settings s = new Settings();
            s.ShowDialog();
        }

        void About_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            AboutControl ac = new AboutControl();
            UI.WindowHandler.OpenForm(ac);
        }

        void ComingSoon_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            ComingSoonControl cs = new ComingSoonControl();
            UI.WindowHandler.OpenForm(cs);
        }

        void Top250_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            Top250Control t2 = new Top250Control();
            UI.WindowHandler.OpenForm(t2);
        }
        private void ShowError()
        {
            try
            {
                this.kListControl1.Clear();
                UI.ErrorButton eb = new ImdbMobile.UI.ErrorButton();
                eb.Icon = global::ImdbMobile.Properties.Resources.Close;
                eb.Parent = this.kListControl1;
                eb.Text = UI.Translations.GetTranslated("0021");
                eb.YIndex = 0;
                eb.CalculateHeight();
                this.kListControl1.AddItem(eb);
            }
            catch (ObjectDisposedException) { }
        }

        private void SearchIMDB()
        {
            List<ImdbSearchResult> isrList = null;
            try
            {
                Search s = new Search();
                isrList = s.QueryIMDB(SearchQuery);
            }
            catch (Exception e)
            {
                try
                {
                    ErrorDelegate ed = new ErrorDelegate(ShowError);
                    this.Invoke(ed);
                }
                catch (ObjectDisposedException) { }
            }

            if (isrList != null)
            {
                try
                {
                    ResultsDelegate rd = new ResultsDelegate(ShowResults);
                    this.Invoke(rd, new object[] { isrList });
                }
                catch (ObjectDisposedException) { }
            }
        }

        private void ShowResults(List<ImdbSearchResult> isrList)
        {
            try
            {
                this.CurrentResults = isrList;
                this.kListControl1.Clear();
                if (isrList.Count == 0)
                {
                    this.kListControl1.Clear();
                    UI.ErrorButton eb = new ImdbMobile.UI.ErrorButton();
                    eb.Icon = global::ImdbMobile.Properties.Resources.Close;
                    eb.Parent = this.kListControl1;
                    eb.Text = UI.Translations.GetTranslated("0022");
                    eb.YIndex = 0;
                    eb.CalculateHeight();
                    this.kListControl1.AddItem(eb);
                    return;
                }

                foreach (ImdbSearchResult isr in isrList)
                {
                    MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                    mpi.YIndex = isrList.IndexOf(isr);
                    if (isr.ResultType == ImdbSearchResult.ResultTypeList.Actor)
                    {
                        ImdbActor ia = (ImdbActor)isr;
                        mpi.MainText = "(" + UI.Translations.GetTranslated("0008") + ") " + ia.Name;
                        mpi.SecondaryText = UI.Translations.GetTranslated("0023") + ": " + ia.KnownFor;
                        mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick2);
                    }
                    if (isr.ResultType == ImdbSearchResult.ResultTypeList.Title)
                    {
                        ImdbTitle it = (ImdbTitle)isr;
                        switch (it.Type)
                        {
                            case ImdbTitle.TitleType.FeatureMovie: mpi.MainText = "(" + UI.Translations.GetTranslated("0024") + ") " + it.Title; break;
                            case ImdbTitle.TitleType.TVSeries: mpi.MainText = "(" + UI.Translations.GetTranslated("0025") + ") " + it.Title; break;
                            case ImdbTitle.TitleType.VideoGame: mpi.MainText = "(" + UI.Translations.GetTranslated("0006") + ") " + it.Title; break;
                        }
                        if (it.Year != null)
                        {
                            mpi.SecondaryText = it.Year;
                        }
                        mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);
                    }
                    this.kListControl1.AddItem(mpi);
                }


                id.DownloadImages(isrList, this.kListControl1, this.ParentForm);
            }
            catch (ObjectDisposedException) { }
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            MovieControl m = new MovieControl((ImdbTitle)this.CurrentResults[mpi.YIndex]);
            UI.WindowHandler.OpenForm(m);
        }

        void mpi_OnClick2(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            ActorControl a = new ActorControl((ImdbActor)this.CurrentResults[mpi.YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

        public void DoSearch(string Text)
        {
            id.Kill();
            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0027"), this.kListControl1);

            SearchQuery = Text;
            if (SearchQuery != "")
            {
                SearchThread = new System.Threading.Thread(SearchIMDB);
                SearchThread.Start();
            }
        }
    }
}
