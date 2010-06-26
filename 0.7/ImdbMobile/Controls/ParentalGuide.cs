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
            UI.KListFunctions.ShowLoading(ErrorMessage, this.kListControl1);
        }

        private void LoadParentalData()
        {
            IMDBData.TitleParentalGuideParser tgp = new TitleParentalGuideParser();
            tgp.ParsingComplete += new EventHandler(tgp_ParsingComplete);
            tgp.ParseParentalGuide(this.CurrentTitle);
        }

        void tgp_ParsingComplete(object sender, EventArgs e)
        {
            TitleParentalGuideParser tgp = (TitleParentalGuideParser)sender;
            SetImdbInformation(tgp.Title);
        }

        private void SetImdbInformation(ImdbTitle title)
        {
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
            this.LoadingList.Visible = false;
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
