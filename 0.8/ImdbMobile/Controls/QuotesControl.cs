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
        private ImdbTitle CurrentTitle;
        private int ListWidth;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingKlist.Items[0]).Text = UI.Translations.GetTranslated("0053") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingKlist.Invalidate();
        }

        private void Clear()
        {
            this.LoadingKlist.Visible = false;
        }

        public QuotesControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0046");

            ListWidth = this.kListControl1.Width;
            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0053") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingKlist);

            LoadImdbInformation();
        }

        private void SetError(string Message)
        {
            UI.KListFunctions.ShowError(Message, this.kListControl1);
            this.LoadingKlist.Dispose();
        }

        private void AddQuotes()
        {
            if (CurrentTitle.Quotes.Count == 0)
            {
                SetError(UI.Translations.GetTranslated("0054"));
                return;
            }

            List<UI.QuoteItem> qList = new List<ImdbMobile.UI.QuoteItem>();
            foreach (ImdbQuoteSection iqs in CurrentTitle.Quotes)
            {
                UI.QuoteItem qi = new ImdbMobile.UI.QuoteItem();
                qi.QuoteSection = iqs;
                qi.YIndex = CurrentTitle.Quotes.IndexOf(iqs);
                qi.Parent = this.kListControl1;
                qi.CalculateHeight();
                qList.Add(qi);
            }
            foreach (UI.QuoteItem qi in qList)
            {
                this.kListControl1.Items.Add(qi);
                Update(qList.IndexOf(qi), qList.Count);
            }

            Clear();
        }

        private void LoadImdbInformation()
        {
            if (this.CurrentTitle.Quotes != null && this.CurrentTitle.Quotes.Count > 0)
            {
                AddQuotes();
            }
            else
            {
                TitleQuoteParser tqp = new TitleQuoteParser();
                tqp.Error += new EventHandler(tqp_Error);
                tqp.ParsingComplete += new EventHandler(tqp_ParsingComplete);
                tqp.ParseQuotes(this.CurrentTitle);
            }
        }

        void tqp_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void tqp_ParsingComplete(object sender, EventArgs e)
        {
            TitleQuoteParser tqp = (TitleQuoteParser)sender;
            this.CurrentTitle = tqp.Title;
            AddQuotes();
        }
    }
}
