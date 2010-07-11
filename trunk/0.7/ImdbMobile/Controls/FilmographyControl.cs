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
    public partial class FilmographyControl : ImdbMobile.UI.SlidingList
    {
        private ImdbActor CurrentActor;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0018") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            this.LoadingList.Visible = false;
        }

        public FilmographyControl(ImdbActor actor)
        {
            CurrentActor = actor;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0020");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0018") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadImdbInformation();
        }

        private void ShowError(string ErrorMessage)
        {
            UI.KListFunctions.ShowError(ErrorMessage, this.kListControl1);
        }


        private void ShowData()
        {
            foreach (ImdbKnownFor ikf in CurrentActor.KnownForFull)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = "(" + ikf.TitleAttribute + ") " + ikf.Title;
                if (ikf.CharacterName != null)
                {
                    mpi.SecondaryText = UI.Translations.GetTranslated("0019") + " " + ikf.CharacterName + " - " + ikf.Year;
                }
                else
                {
                    mpi.SecondaryText = ikf.Year;
                }
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);

                AddItem(mpi);
                Update(CurrentActor.KnownForFull.IndexOf(ikf), CurrentActor.KnownForFull.Count);
            }
            Clear();
        }

        private void AddItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            this.kListControl1.Items.Add(mpi);
        }

        void mpi_OnClick(object Sender)
        {
            MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = ((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender);
            int YIndex = UI.KListFunctions.GetIndexOf(mpi, this.kListControl1);
            ImdbTitle t = CurrentActor.KnownForFull[YIndex];
            MovieControl m = new MovieControl(t);
            UI.WindowHandler.OpenForm(m);
        }

        private void LoadImdbInformation()
        {
            IMDBData.FilmographyParser fp = new ImdbMobile.IMDBData.FilmographyParser(CurrentActor);
            fp.Error += new EventHandler(fp_Error);
            fp.ParsingComplete += new EventHandler(fp_ParsingComplete);
            fp.ParseDetails();
        }

        void fp_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.LoadingList.Visible = false;
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void fp_ParsingComplete(object sender, EventArgs e)
        {
            FilmographyParser fp = (FilmographyParser)sender;
            this.CurrentActor = fp.CurrentActor;
            ShowData();
        }
    }
}
