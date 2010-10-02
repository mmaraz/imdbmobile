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
    public partial class ParentalGuide : UI.SlidingList
    {
        private ImdbTitle CurrentTitle;

        public ParentalGuide(ImdbTitle title)
        {
            InitializeComponent();

            this.CurrentTitle = title;

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0093") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadParentalData();
        }

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0093") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void ShowError(string ErrorMessage)
        {
            this.LoadingList.Visible = false;
            UI.KListFunctions.ShowError(ErrorMessage, this.kListControl1);
        }

        private void LoadParentalData()
        {
            if (this.CurrentTitle.ParentalGuide != null && this.CurrentTitle.ParentalGuide.Count > 0)
            {
                SetImdbInformation(this.CurrentTitle);
            }
            else
            {
                IMDBData.TitleParentalGuideParser tgp = new TitleParentalGuideParser();
                tgp.Error += new EventHandler(tgp_Error);
                tgp.ParsingComplete += new EventHandler(tgp_ParsingComplete);
                tgp.ParseParentalGuide(this.CurrentTitle);
            }
        }

        void tgp_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.LoadingList.Dispose();
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void tgp_ParsingComplete(object sender, EventArgs e)
        {
            TitleParentalGuideParser tgp = (TitleParentalGuideParser)sender;
            SetImdbInformation(tgp.Title);
        }

        private void SetImdbInformation(ImdbTitle title)
        {
            if (title.ParentalGuide.Count == 0)
            {
                ShowError("There is no Parental Guide info for this title.");
                return;
            }
            this.kListControl1.Items.Clear();
            int Counter = 0;
            foreach (KeyValuePair<string, string> kvp in title.ParentalGuide)
            {
                UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
                td.Heading = Capitalise(kvp.Key);
                td.Parent = this.kListControl1;
                td.Text = kvp.Value;
                td.YIndex = Counter;
                td.CalculateHeight();
                this.kListControl1.Items.Add(td);
                Counter++;

                Update(td.YIndex + 1, title.ParentalGuide.Count);
            }
            this.LoadingList.Dispose();
        }

        private string Capitalise(string str)
        {
            if (str.Length > 0)
            {
                return ("" + str[0]).ToUpper() + str.Substring(1, str.Length - 1);
            }
            return str;
        }
    }
}
