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
    public partial class QuotesControl : ImdbMobile.UI.SlidingList
    {
        private static ImdbTitle CurrentTitle;
        private static int ListWidth;
        private delegate void AddQuoteItem(UI.QuoteItem qi);
        private delegate void LoadingDone();
        private delegate void ShowError(string Error);
        System.Threading.Thread LoadingThread;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private static int CurrentPage = -1;
        private static int TotalPages = 1;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingKlist[0]).Text = UI.Translations.GetTranslated("0053") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingKlist.Invalidate();
        }

        private void Clear()
        {
            try
            {
                this.LoadingKlist.Visible = false;
            }
            catch (ObjectDisposedException) { }
        }

        public QuotesControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0046");
            this.ThreadList.Add(LoadingThread);

            ListWidth = this.kListControl1.Width;
            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0053") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingKlist);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        private void SetError(string Message)
        {
            try
            {
                UI.KListFunctions.ShowError(Message, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }

        private void AddQuote(UI.QuoteItem qi)
        {
            try
            {
                this.kListControl1.AddItem(qi);
            }
            catch (ObjectDisposedException) { }
        }

        private void NextPage()
        {
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage++;
                ShowData();
            }
        }

        private void PrevPage()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                ShowData();
            }
        }

        private void ShowData()
        {
            ClearList ShowLoading = delegate
            {
                try
                {
                    if (this.kListControl1[0].GetType() == typeof(UI.PagerDisplay))
                    {
                        UI.PagerDisplay pd = (UI.PagerDisplay)this.kListControl1[0];
                        pd.CurrentPage = CurrentPage + 1;
                        this.kListControl1.Clear();
                        this.kListControl1.AddItem(pd);
                    }
                }
                catch (Exception) { }
                UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0018") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingKlist);
            };
            this.Invoke(ShowLoading);

            int Start = CurrentPage * SettingsWrapper.GlobalSettings.NumToDisplay;
            int Take = SettingsWrapper.GlobalSettings.NumToDisplay;
            int Counter = 1;

            foreach (ImdbQuoteSection iqs in CurrentTitle.Quotes.Skip(Start).Take(Take))
            {
                UI.QuoteItem qi = new ImdbMobile.UI.QuoteItem();
                qi.QuoteSection = iqs;
                qi.YIndex = Counter;
                qi.Parent = this.kListControl1;
                qi.ListWidth = ListWidth;
                qi.CalculateHeight();

                AddQuoteItem aqi = new AddQuoteItem(AddQuote);
                this.Invoke(aqi, new object[] { qi });

                UpdateStatus us = new UpdateStatus(Update);
                this.Invoke(us, new object[] { Counter, Take });

                Counter++;
            }

            ClearList cl = new ClearList(Clear);
            this.Invoke(cl);

            if (CurrentTitle.Quotes.Count == 0)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0054") });
                }
                catch (Exception) { }
            }
        }

        private void LoadImdbInformation()
        {
            try
            {
                TitleQuoteParser tqp = new TitleQuoteParser();
                ImdbTitle title = tqp.ParseQuotes(CurrentTitle);

                if (CurrentTitle.Quotes.Count > SettingsWrapper.GlobalSettings.NumToDisplay)
                {
                    TotalPages = (int)Math.Ceiling((double)CurrentTitle.Quotes.Count / (double)SettingsWrapper.GlobalSettings.NumToDisplay);
                    ClearList Pager = delegate
                    {
                        UI.PagerDisplay pd = new ImdbMobile.UI.PagerDisplay();
                        pd.TotalPages = TotalPages;
                        pd.CurrentPage = 1;
                        pd.Parent = this.kListControl1;
                        pd.YIndex = 0;
                        pd.Next += new ImdbMobile.UI.PagerDisplay.MouseEvent(pd_Next);
                        pd.Previous += new ImdbMobile.UI.PagerDisplay.MouseEvent(pd_Previous);
                        pd.CalculateHeight();
                        this.kListControl1.AddItem(pd);
                    };
                    this.Invoke(Pager);
                }

                TotalPages = 1;
                CurrentPage = -1;
                NextPage();
            }
            catch (Exception e)
            {
                try
                {
                    ShowError se = new ShowError(SetError);
                    this.Invoke(se, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }

        void pd_Previous(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.PagerDisplay Sender)
        {
            PrevPage();
        }

        void pd_Next(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.PagerDisplay Sender)
        {
            NextPage();
        }
    }
}
