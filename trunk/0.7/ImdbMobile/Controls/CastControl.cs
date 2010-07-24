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
            List<ImdbActor> FullList = new List<ImdbActor>();
            FullList.AddRange(CurrentTitle.Directors);
            FullList.AddRange(CurrentTitle.Writers.Select(w => w as ImdbActor).ToList());
            FullList.AddRange(CurrentTitle.Cast.Select(c => c as ImdbActor).ToList());
            id.DownloadImages(FullList, this.kListControl1, this.ParentForm);
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
            int YIndex = 0;
            foreach (ImdbActor ia in CurrentTitle.Directors)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = ia.Name;
                string secondaryText = "(Director)";
                mpi.SecondaryText = secondaryText;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpiDir_OnClick);

                AddCharacter(mpi);
                Update(YIndex, CurrentTitle.Cast.Count + CurrentTitle.Directors.Count + CurrentTitle.Writers.Count);

                YIndex++;
            }
            foreach (ImdbWriter ia in CurrentTitle.Writers)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = ia.Name;
                string secondaryText = "(Writer)";
                mpi.SecondaryText = secondaryText;
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpiWri_OnClick);

                AddCharacter(mpi);
                Update(YIndex, CurrentTitle.Cast.Count + CurrentTitle.Directors.Count + CurrentTitle.Writers.Count);

                YIndex++;
            }
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
                Update(YIndex, CurrentTitle.Cast.Count + CurrentTitle.Directors.Count + CurrentTitle.Writers.Count);
            }

            if (!CurrentTitle.HasFullCast)
            {
                UI.ActionButton ab = new ImdbMobile.UI.ActionButton();
                ab.Icon = "Cast";
                ab.Text = UI.Translations.GetTranslated("0099") + "...";
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
            pc.Error += new EventHandler(pc_Error);
            pc.ParsingComplete += new EventHandler(pc_ParsingComplete);
            pc.ParseFullCast(CurrentTitle);
        }

        void pc_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.LoadingList.Visible = false;
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
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
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1) - CurrentTitle.Directors.Count - CurrentTitle.Writers.Count;
            ActorControl a = new ActorControl(CurrentTitle.Cast[YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

        void mpiDir_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = ((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender);
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ActorControl a = new ActorControl(CurrentTitle.Directors[YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

        void mpiWri_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = ((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender);
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1) - CurrentTitle.Directors.Count;
            ActorControl a = new ActorControl(CurrentTitle.Writers[YIndex]);
            UI.WindowHandler.OpenForm(a);
        }

    }
}
