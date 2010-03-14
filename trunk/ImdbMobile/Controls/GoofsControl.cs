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

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0033") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            try
            {
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
            ParentWidth = this.kListControl1.Width;

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

        private void LoadImdbInformation()
        {
            try
            {
                TitleGoofParser tgp = new TitleGoofParser();
                CurrentTitle = tgp.ParseGoofs(CurrentTitle);

                foreach (ImdbGoof ig in CurrentTitle.Goofs)
                {
                    UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
                    td.YIndex = CurrentTitle.Goofs.IndexOf(ig);
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
                    this.Invoke(us, new object[] { td.YIndex+1, CurrentTitle.Goofs.Count });
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
