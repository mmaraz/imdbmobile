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
        private delegate void AddQuoteItem();
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

        private void AddQuotes()
        {
            try
            {
                if (CurrentTitle.Quotes.Count == 0)
                {
                    try
                    {
                        ShowError sr = new ShowError(SetError);
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0054") });
                    }
                    catch (Exception) { }
                    return;
                }

                List<UI.QuoteItem> qList = new List<ImdbMobile.UI.QuoteItem>();
                foreach (ImdbQuoteSection iqs in CurrentTitle.Quotes)
                {
                    UI.QuoteItem qi = new ImdbMobile.UI.QuoteItem();
                    qi.QuoteSection = iqs;
                    qi.YIndex = CurrentTitle.Quotes.IndexOf(iqs);
                    qi.Parent = this.kListControl1;
                    qi.ListWidth = ListWidth;
                    qi.CalculateHeight();
                    qList.Add(qi);
                }
                foreach (UI.QuoteItem qi in qList)
                {
                    this.kListControl1.AddItem(qi);
                    Update(qList.IndexOf(qi), qList.Count);
                }

                try
                {
                    ClearList cl = new ClearList(Clear);
                    this.Invoke(cl);
                }
                catch (ObjectDisposedException) { }
            }
            catch (ObjectDisposedException) { }
        }

        private void ShowData()
        {

            try
            {
                AddQuoteItem aqi = new AddQuoteItem(AddQuotes);
                this.Invoke(aqi);
            }
            catch (ObjectDisposedException) { }

            

            
        }

        private void LoadImdbInformation()
        {
            try
            {
                TitleQuoteParser tqp = new TitleQuoteParser();
                ImdbTitle title = tqp.ParseQuotes(CurrentTitle);

                ShowData();
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
