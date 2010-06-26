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
        private delegate void AddCharacterItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi);
        private delegate void HideStatus();
        private delegate void ShowError(string Message);
        ImageDownloader id = new ImageDownloader();
        System.Threading.Thread LoadingThread;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private static int CurrentPage = -1;
        private static int TotalPages = 1;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0010") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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
        

        public CastControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.ImageDownloaderList.Add(id);
            this.ThreadList.Add(LoadingThread);

            this.Name = UI.Translations.GetTranslated("0012");
            this.Text = UI.Translations.GetTranslated("0012");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0010") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            this.LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            this.LoadingThread.Start();
        }

        private void SetError(string Message)
        {
            try
            {
                UI.KListFunctions.ShowError(Message, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }

        private void SetStatus()
        {
            try
            {
                int Start = CurrentPage * SettingsWrapper.GlobalSettings.NumToDisplay;
                int Take = SettingsWrapper.GlobalSettings.NumToDisplay;
                id.DownloadImages(CurrentTitle.Cast.Skip(Start).Take(Take).ToList(), this.kListControl1, this.ParentForm);
            }
            catch (ObjectDisposedException)
            {

            }
        }

        private void AddCharacter(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            try
            {
                this.kListControl1.AddItem(mpi);
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
            foreach (ImdbCharacter actor in CurrentTitle.Cast)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = actor.Name;
                string secondaryText = actor.CharacterName;
                if (actor.TitleAttribute != null)
                    secondaryText += " " + actor.TitleAttribute;
                mpi.SecondaryText = secondaryText;
                mpi.YIndex = CurrentTitle.Cast.IndexOf(actor);
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                try
                {
                    AddCharacterItem aci = new AddCharacterItem(AddCharacter);
                    this.Invoke(aci, new object[] { mpi });

                    UpdateStatus us = new UpdateStatus(Update);
                    this.Invoke(us, new object[] { mpi.YIndex, CurrentTitle.Cast.Count });
                }
                catch (ObjectDisposedException) { }
            }

            ClearList cl = new ClearList(Clear);
            this.Invoke(cl);
            HideStatus hs = new HideStatus(SetStatus);
            this.Invoke(hs);

            if (CurrentTitle.Cast.Count == 0)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0011") });
                }
                catch (Exception) { }
            }
        }

        private void LoadImdbInformation()
        {
            try
            {
                
                IMDBData.ParseCast pc = new ImdbMobile.IMDBData.ParseCast();
                CurrentTitle = pc.ParseFullCast(CurrentTitle);

                ShowData();
            }
            catch (Exception e)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    this.Invoke(sr, new object[] { e.Message });
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

        void mpi_OnClick(object Sender)
        {
            ActorControl a = new ActorControl(CurrentTitle.Cast[((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender).YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

    }
}
