﻿using System;
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
        private string SearchQuery { get; set; }
        private List<ImdbSearchResult> CurrentResults;
        ImageDownloader id = new ImageDownloader();

        public MainControl()
        {
            InitializeComponent();

            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            this.kListControl1.Items.Clear();

            UI.ActionButton Top250 = new ImdbMobile.UI.ActionButton();
            Top250.Icon = "MoreInfo";
            Top250.Parent = this.kListControl1;
            Top250.Text = UI.Translations.GetTranslated("0028");
            Top250.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Top250_MouseUp);
            Top250.CalculateHeight();
            this.kListControl1.Items.Add(Top250);

            UI.ActionButton Bottom100 = new ImdbMobile.UI.ActionButton();
            Bottom100.Icon = "Trashcan";
            Bottom100.Parent = this.kListControl1;
            Bottom100.Text = UI.Translations.GetTranslated("0122");
            Bottom100.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Bottom100_MouseUp);
            Bottom100.CalculateHeight();
            this.kListControl1.Items.Add(Bottom100);

            UI.ActionButton ComingSoon = new ImdbMobile.UI.ActionButton();
            ComingSoon.Icon = "ComingSoon";
            ComingSoon.Parent = this.kListControl1;
            ComingSoon.Text = UI.Translations.GetTranslated("0029");
            ComingSoon.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ComingSoon_MouseUp);
            ComingSoon.CalculateHeight();
            this.kListControl1.Items.Add(ComingSoon);

            UI.ActionButton RecentSearches = new ImdbMobile.UI.ActionButton();
            RecentSearches.Icon = "Search";
            RecentSearches.Parent = this.kListControl1;
            RecentSearches.Text = UI.Translations.GetTranslated("0100");
            RecentSearches.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(RecentSearches_MouseUp);
            RecentSearches.CalculateHeight();
            this.kListControl1.Items.Add(RecentSearches);

            UI.ActionButton Settings = new ImdbMobile.UI.ActionButton();
            Settings.Icon = "Settings";
            Settings.Parent = this.kListControl1;
            Settings.Text = UI.Translations.GetTranslated("0030");
            Settings.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Settings_MouseUp);
            Settings.CalculateHeight();
            this.kListControl1.Items.Add(Settings);

            UI.ActionButton About = new ImdbMobile.UI.ActionButton();
            About.Icon = "about";
            About.Parent = this.kListControl1;
            About.Text = UI.Translations.GetTranslated("0089");
            About.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(About_MouseUp);
            About.CalculateHeight();
            this.kListControl1.Items.Add(About);

            UI.ActionButton Exit = new ImdbMobile.UI.ActionButton();
            Exit.Icon = "Close";
            Exit.Parent = this.kListControl1;
            Exit.Text = UI.Translations.GetTranslated("0031");
            Exit.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Exit_MouseUp);
            Exit.CalculateHeight();
            this.kListControl1.Items.Add(Exit);

            this.kListControl1.Visible = true;
        }

        void RecentSearches_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            RecentSearchControl rsc = new RecentSearchControl();
            UI.WindowHandler.OpenForm(rsc);
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

        void Bottom100_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            Bottom100Control t1 = new Bottom100Control();
            UI.WindowHandler.OpenForm(t1);
        }

        private void ShowError()
        {
            try
            {
                this.kListControl1.Items.Clear();
                UI.ErrorButton eb = new ImdbMobile.UI.ErrorButton();
                eb.Icon = "Close";
                eb.Parent = this.kListControl1;
                eb.Text = UI.Translations.GetTranslated("0021");
                eb.CalculateHeight();
                this.kListControl1.Items.Add(eb);
            }
            catch (ObjectDisposedException) { }
        }

        private void SearchIMDB()
        {
            Search s = new Search();
            s.ParsingComplete += new EventHandler(s_ParsingComplete);
            s.Error += new EventHandler(s_Error);
            s.QueryIMDB(SearchQuery);
        }

        void s_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            UI.KListFunctions.ShowError(ae.EventData, this.kListControl1);
        }

        void s_ParsingComplete(object sender, EventArgs e)
        {
            Search s = (Search)sender;
            ShowResults(s.Results);
        }

        private void ShowResults(List<ImdbSearchResult> isrList)
        {
            try
            {
                this.CurrentResults = isrList;
                this.kListControl1.Items.Clear();
                if (isrList.Count == 0)
                {
                    this.kListControl1.Items.Clear();
                    UI.ErrorButton eb = new ImdbMobile.UI.ErrorButton();
                    eb.Icon = "Close";
                    eb.Parent = this.kListControl1;
                    eb.Text = UI.Translations.GetTranslated("0022");
                    eb.CalculateHeight();
                    this.kListControl1.Items.Add(eb);
                    return;
                }

                foreach (ImdbSearchResult isr in isrList)
                {
                    MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
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
                    this.kListControl1.Items.Add(mpi);
                }


                id.DownloadImages(isrList, this.kListControl1, this.ParentForm);
            }
            catch (ObjectDisposedException) { }
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            MovieControl m = new MovieControl((ImdbTitle)this.CurrentResults[YIndex]);
            UI.WindowHandler.OpenForm(m);
        }

        void mpi_OnClick2(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = (MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender;
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ActorControl a = new ActorControl((ImdbActor)this.CurrentResults[YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

        public void DoSearch(string Text)
        {
            id.Kill();
            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0027"), this.kListControl1);

            SearchQuery = Text;
            if (SearchQuery != "")
            {
                SearchIMDB();
            }
        }
    }
}
