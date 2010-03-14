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
    public partial class SeasonListControl : ImdbMobile.UI.SlidingList
    {
        private static ImdbTitle CurrentTitle;
        private System.Threading.Thread LoadingThread;

        private delegate void AddItem(int YIndex, string Text);
        private delegate void ShowList();

        public SeasonListControl(ImdbTitle title)
        {
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0056");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0055") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
            CurrentTitle = title;

            this.ThreadList.Add(LoadingThread);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        private void ShowComplete()
        {
            try
            {
                this.LoadingList.Visible = false;
            }
            catch (ObjectDisposedException) { }
        }

        private void Add(int YIndex, string Label)
        {
            try
            {
                UI.ActionButton ab = new ImdbMobile.UI.ActionButton();
                ab.Text = Label;
                ab.Parent = this.kListControl1;
                ab.YIndex = YIndex;
                ab.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ab_MouseUp);
                ab.CalculateHeight();
                this.kListControl1.AddItem(ab);
            }
            catch (ObjectDisposedException) { }
        }

        private void LoadImdbInformation()
        {
            try
            {
                TitleEpisodeParser tep = new TitleEpisodeParser();
                CurrentTitle = tep.ParseTitleEpisodes(CurrentTitle);

                foreach (ImdbSeason isea in CurrentTitle.Seasons)
                {
                    AddItem ai = new AddItem(Add);
                    this.Invoke(ai, new object[] { CurrentTitle.Seasons.IndexOf(isea), isea.Label });
                }

                ShowList sl = new ShowList(ShowComplete);
                this.Invoke(sl);
            }
            catch (Exception e)
            {

            }
        }

        void ab_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            EpisodeListControl el = new EpisodeListControl(CurrentTitle.Seasons[Sender.YIndex]);
            UI.WindowHandler.OpenForm(el);
        }
    }
}
