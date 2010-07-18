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
    public partial class FilmographyListControl : ImdbMobile.UI.SlidingList
    {
        private List<ImdbKnownFor> KnownForList;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0018") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            this.LoadingList.Dispose();
        }

        public FilmographyListControl(List<ImdbKnownFor> KnownForList)
        {
            this.KnownForList = KnownForList;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0020");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0018") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            ShowData();
        }


        private void ShowData()
        {
            foreach (ImdbKnownFor ikf in this.KnownForList)
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
                mpi.MainText = ikf.Title;
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
                Update(this.KnownForList.IndexOf(ikf), this.KnownForList.Count);
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
            ImdbTitle t = this.KnownForList[YIndex];
            MovieControl m = new MovieControl(t);
            UI.WindowHandler.OpenForm(m);
        }
    }
}
