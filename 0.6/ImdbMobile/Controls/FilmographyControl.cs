﻿using System;
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
        private static ImdbActor CurrentActor;

        private delegate void SetInformation(ImdbActor actor);
        private delegate void AddMovieItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi);
        private delegate void ShowErrorInfo(string ErrorMessage);
        System.Threading.Thread LoadingThread;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private static int CurrentPage = -1;
        private static int TotalPages = 1;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0018") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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

        public FilmographyControl(ImdbActor actor)
        {
            CurrentActor = actor;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0020");
            this.ThreadList.Add(LoadingThread);

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0018") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        private void ShowError(string ErrorMessage)
        {
            try
            {
                UI.KListFunctions.ShowError(ErrorMessage, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
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
                mpi.YIndex = CurrentActor.KnownForFull.IndexOf(ikf);
                mpi.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(mpi_OnClick);
                try
                {
                    AddMovieItem ami = new AddMovieItem(AddItem);
                    this.Invoke(ami, new object[] { mpi });

                    UpdateStatus us = new UpdateStatus(Update);
                    this.Invoke(us, new object[] { CurrentActor.KnownForFull.IndexOf(ikf), CurrentActor.KnownForFull.Count });
                }
                catch (ObjectDisposedException) { }
            }
            try
            {
                ClearList cl = new ClearList(Clear);
                this.Invoke(cl);
            }
            catch (ObjectDisposedException) { }
        }

        private void SetImdbInformation(ImdbActor actor)
        {
            try
            {
                CurrentActor = actor;

                ShowData();
            }
            catch (ObjectDisposedException) { }
        }

        private void AddItem(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi)
        {
            try
            {
                this.kListControl1.AddItem(mpi);
            }
            catch (ObjectDisposedException) { }
        }

        void mpi_OnClick(object Sender)
        {
            ImdbTitle t = CurrentActor.KnownForFull[((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)Sender).YIndex];
            MovieControl m = new MovieControl(t);
            UI.WindowHandler.OpenForm(m);
        }

        private void LoadImdbInformation()
        {
            try
            {
                IMDBData.FilmographyParser fp = new ImdbMobile.IMDBData.FilmographyParser(CurrentActor);
                ImdbActor actor = fp.ParseDetails();

                try
                {
                    SetInformation si = new SetInformation(SetImdbInformation);
                    this.Invoke(si, new object[] { actor });
                }
                catch (ObjectDisposedException) { }
            }
            catch (Exception e)
            {
                try
                {
                    ShowErrorInfo se = new ShowErrorInfo(ShowError);
                    this.Invoke(se, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }
    }
}
