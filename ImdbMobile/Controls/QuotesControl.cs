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

            ListWidth = this.LoadingKlist.Width;
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

        private void LoadImdbInformation()
        {
            try
            {
                TitleQuoteParser tqp = new TitleQuoteParser();
                ImdbTitle title = tqp.ParseQuotes(CurrentTitle);

                foreach (ImdbQuoteSection iqs in title.Quotes)
                {
                    UI.QuoteItem qi = new ImdbMobile.UI.QuoteItem();
                    qi.QuoteSection = iqs;
                    qi.YIndex = title.Quotes.IndexOf(iqs);
                    qi.Parent = this.kListControl1;
                    qi.ListWidth = ListWidth;
                    qi.CalculateHeight();

                    AddQuoteItem aqi = new AddQuoteItem(AddQuote);
                    this.Invoke(aqi, new object[] { qi });

                    UpdateStatus us = new UpdateStatus(Update);
                    this.Invoke(us, new object[] { qi.YIndex + 1, title.Quotes.Count });
                }

                ClearList cl = new ClearList(Clear);
                this.Invoke(cl);

                if (title.Quotes.Count == 0)
                {
                    try
                    {
                        ShowError sr = new ShowError(SetError);
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0054") });
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
                catch (Exception) { }
            }
        }
    }
}
