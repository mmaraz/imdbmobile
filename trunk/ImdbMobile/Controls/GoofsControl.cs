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
    public partial class GoofsControl : ImdbMobile.UI.SlidingList
    {
        private delegate void ShowError(string message);
        private delegate void ShowLoadingDone();
        private delegate void AddPanelItem(UI.TextDisplay td);
        System.Threading.Thread LoadingThread;
        private static int ParentWidth;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private static int CurrentPage = -1;
        private static int TotalPages = 1;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0033") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            try
            {
                this.kListControl1.Visible = true;
                this.LoadingList.Visible = false;
            }
            catch (ObjectDisposedException) { }
        }

        private static ImdbTitle CurrentTitle;

        public GoofsControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0042");

            this.ThreadList.Add(LoadingThread);

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0033") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
            ParentWidth = Screen.PrimaryScreen.WorkingArea.Width;

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
                    this.kListControl1.Visible = false;
                    UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0018") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
                };
                this.Invoke(ShowLoading);
            

            System.Threading.Thread t = new System.Threading.Thread(delegate
            {
            int Start = CurrentPage * SettingsWrapper.GlobalSettings.NumToDisplay;
            int Take = SettingsWrapper.GlobalSettings.NumToDisplay;
            int Counter = 1;

            foreach (ImdbGoof ig in CurrentTitle.Goofs.Skip(Start).Take(Take))
            {
                UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
                td.YIndex = Counter;
                td.Text = ig.Description;
                switch (ig.Type)
                {
                    case ImdbGoof.GoofType.Continuity: td.Heading = UI.Translations.GetTranslated("0034"); td.Icon = global::ImdbMobile.Properties.Resources.Continuity; break;
                    case ImdbGoof.GoofType.CrewOrEquipment: td.Heading = UI.Translations.GetTranslated("0035"); td.Icon = global::ImdbMobile.Properties.Resources.Cast; break;
                    case ImdbGoof.GoofType.FactualErrors: td.Heading = UI.Translations.GetTranslated("0036"); td.Icon = global::ImdbMobile.Properties.Resources.Trailers; break;
                    case ImdbGoof.GoofType.IncorrectlyRegarded: td.Heading = UI.Translations.GetTranslated("0037"); td.Icon = global::ImdbMobile.Properties.Resources.Close; break;
                    case ImdbGoof.GoofType.PlotHoles: td.Heading = UI.Translations.GetTranslated("0038"); td.Icon = global::ImdbMobile.Properties.Resources.Trailers; break;
                    case ImdbGoof.GoofType.RevealingMistakes: td.Heading = UI.Translations.GetTranslated("0039"); td.Icon = global::ImdbMobile.Properties.Resources.MoreInfo; break;
                    default: td.Heading = UI.Translations.GetTranslated("0040"); td.Icon = global::ImdbMobile.Properties.Resources.MoreInfo; break;
                }

                td.CalculateHeight(ParentWidth);

                AddPanelItem api = new AddPanelItem(AddItem);
                this.Invoke(api, new object[] { td });

                UpdateStatus us = new UpdateStatus(Update);
                this.Invoke(us, new object[] { Counter, Take });

                Counter++;
            }

            ClearList ci = new ClearList(Clear);
            this.Invoke(ci);

            if (CurrentTitle.Goofs.Count == 0)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0041") });
                }
                catch (Exception) { }
            }
            });
            t.Start();
        }

        private void LoadImdbInformation()
        {
            try
            {
                TitleGoofParser tgp = new TitleGoofParser();
                CurrentTitle = tgp.ParseGoofs(CurrentTitle);

                if (CurrentTitle.Goofs.Count > SettingsWrapper.GlobalSettings.NumToDisplay)
                {
                    TotalPages = (int)Math.Ceiling((double)CurrentTitle.Goofs.Count / (double)SettingsWrapper.GlobalSettings.NumToDisplay);
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

        private void AddItem(UI.TextDisplay td)
        {
            try
            {
                this.kListControl1.AddItem(td);
            }
            catch (ObjectDisposedException) { }
        }
    }
}
