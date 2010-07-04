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
    public partial class CastControl : ImdbMobile.UI.SlidingList
    {
        ImageDownloader id = new ImageDownloader();

        private int CurrentPage = -1;
        private int TotalPages = 1;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0010") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            this.LoadingList.Visible = false;
        }
        
        private ImdbTitle CurrentTitle;
        

        public CastControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.ImageDownloaderList.Add(id);

            this.Name = UI.Translations.GetTranslated("0012");
            this.Text = UI.Translations.GetTranslated("0012");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0010") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadImdbInformation();
        }

        private void SetError(string Message)
        {
            UI.KListFunctions.ShowError(Message, this.kListControl1);
        }

        private void SetStatus()
        {
            int Start = CurrentPage * SettingsWrapper.GlobalSettings.NumToDisplay;
            int Take = SettingsWrapper.GlobalSettings.NumToDisplay;
            id.DownloadImages(CurrentTitle.Cast.Skip(Start).Take(Take).ToList(), this.kListControl1, this.ParentForm);
        }

        private void AddCharacter(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            this.kListControl1.Items.Add(mpi);
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
            foreach (ImdbCharacter actor in CurrentTitle.Cast)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = actor.Name;
                string secondaryText = actor.CharacterName;
                if (actor.TitleAttribute != null)
                    secondaryText += " " + actor.TitleAttribute;
                mpi.SecondaryText = secondaryText;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                AddCharacter(mpi);
                int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
                Update(YIndex, CurrentTitle.Cast.Count);
            }

            if (!CurrentTitle.HasFullCast)
            {
                UI.ActionButton ab = new ImdbMobile.UI.ActionButton();
                ab.Icon = global::ImdbMobile.Properties.Resources.Cast;
                ab.Text = "Full Cast...";
                ab.Parent = this.kListControl1;
                ab.YIndex = CurrentTitle.Cast.Count;
                ab.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ab_MouseUp);
                ab.CalculateHeight();
                this.kListControl1.Items.Add(ab);
            }

            Clear();
            SetStatus();
            
            if (CurrentTitle.Cast.Count == 0)
            {
                SetError(UI.Translations.GetTranslated("0011"));
                return;
            }
        }

        void ab_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            GetFullCast();
        }

        private void LoadImdbInformation()
        {
            ShowData();
        }

        private void GetFullCast()
        {
            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0010") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
            this.LoadingList.Visible = true;
            this.kListControl1.Items.Clear();

            IMDBData.ParseCast pc = new ImdbMobile.IMDBData.ParseCast();
            pc.ParsingComplete += new EventHandler(pc_ParsingComplete);
            pc.ParseFullCast(CurrentTitle);
        }

        void pc_ParsingComplete(object sender, EventArgs e)
        {
            ParseCast pc = (ParseCast)sender;
            this.CurrentTitle = pc.Title;
            this.CurrentTitle.HasFullCast = true;
            ShowData();
        }

        void pd_Previous(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.PagerDisplay Sender)
        {
            PrevPage();
        }

        void pd_Next(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.PagerDisplay Sender)
        {
            NextPage();
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = ((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender); 
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ActorControl a = new ActorControl(CurrentTitle.Cast[YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

    }
}
