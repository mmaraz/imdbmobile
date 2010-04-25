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
        private delegate void LoadImdbInformation(ImdbTitle title);
        private delegate void ShowErrorInfo(string ErrorMessage);
        private delegate void UpdateStatus(int Current, int Total);
        System.Threading.Thread LoadingThread;

        public ParentalGuide(ImdbTitle title)
        {
            InitializeComponent();

            this.ThreadList.Add(LoadingThread);

            this.CurrentTitle = title;

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0093") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadParentalData);
            LoadingThread.Start();
        }

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0093") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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

        private void LoadParentalData()
        {
            try
            {
                IMDBData.TitleParentalGuideParser tgp = new TitleParentalGuideParser();
                Dictionary<string, string> ParentalGuide = tgp.ParseParentalGuide(this.CurrentTitle);
                this.CurrentTitle.ParentalGuide = ParentalGuide;

                try
                {
                    LoadImdbInformation li = new LoadImdbInformation(SetImdbInformation);
                    this.Invoke(li, new object[] { this.CurrentTitle });
                }
                catch (ObjectDisposedException) { }
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
                foreach (KeyValuePair<string, string> kvp in title.ParentalGuide)
                {
                    UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
                    td.Heading = Capitalise(kvp.Key);
                    td.Parent = this.kListControl1;
                    td.Text = kvp.Value;
                    td.YIndex = Counter;
                    td.CalculateHeight();
                    this.kListControl1.AddItem(td);
                    Counter++;

                    try
                    {
                        UpdateStatus us = new UpdateStatus(Update);
                        this.Invoke(us, new object[] { td.YIndex + 1, title.ParentalGuide.Count });
                    }
                    catch (ObjectDisposedException) { }
                }
                this.LoadingList.Visible = false;
            }
            catch (Exception) { }
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
