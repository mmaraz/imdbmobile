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
        private static int ParentWidth;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0033") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private ImdbTitle CurrentTitle;

        public GoofsControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0042");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0033") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
            ParentWidth = Screen.PrimaryScreen.WorkingArea.Width;

            LoadImdbInformation();
        }

        private void SetError(string Message)
        {
            this.LoadingList.Visible = false;
            UI.KListFunctions.ShowError(Message, this.kListControl1);
        }

        private void LoadImdbInformation()
        {
            if (this.CurrentTitle.Goofs != null && this.CurrentTitle.Goofs.Count > 0)
            {
                AddItems();
            }
            else
            {
                TitleGoofParser tgp = new TitleGoofParser();
                tgp.ParsingComplete += new EventHandler(tgp_ParsingComplete);
                tgp.ParseGoofs(CurrentTitle);
            }
        }

        void tgp_ParsingComplete(object sender, EventArgs e)
        {
            TitleGoofParser tgp = (TitleGoofParser)sender;
            CurrentTitle = tgp.Title;
            AddItems();
        }

        private void AddItems()
        {
            if (CurrentTitle.Goofs.Count == 0)
            {
                SetError(UI.Translations.GetTranslated("0041"));
                return;
            }

            List<UI.TextDisplay> tdList = new List<ImdbMobile.UI.TextDisplay>();
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
                td.Parent = this.kListControl1;

                td.CalculateHeight();
                tdList.Add(td);
            }
            foreach (UI.TextDisplay td in tdList)
            {
                this.kListControl1.Items.Add(td);
                Update(tdList.IndexOf(td), tdList.Count);
            }

            this.kListControl1.Visible = true;
            this.LoadingList.Visible = false;
        }
    }
}
