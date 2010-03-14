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
    public partial class ActorControl : ImdbMobile.UI.SlidingList
    {
        private ImdbActor CurrentActor;
        private delegate void SetImage();
        private delegate void LoadImdbInformation(ImdbActor actor);
        private delegate void ShowErrorInfo(string ErrorMessage);
        public UI.ActorHeader ah;
        System.Threading.Thread ImageThread;
        System.Threading.Thread LoadingThread;

        public ActorControl(ImdbActor actor)
        {
            this.CurrentActor = actor;
            InitializeComponent();

            this.Name = UI.Translations.GetTranslated("0008");
            this.Text = UI.Translations.GetTranslated("0009");

            this.ThreadList.Add(ImageThread);
            this.ThreadList.Add(LoadingThread);

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0001") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);

            LoadingThread = new System.Threading.Thread(LoadActorData);
            LoadingThread.Start();
        }

        private void ShowError(string ErrorMessage)
        {
            try
            {
                UI.KListFunctions.ShowLoading(ErrorMessage, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }

        private void LoadActorData()
        {
            try
            {
                IMDBData.ActorParser ap = new ImdbMobile.IMDBData.ActorParser(this.CurrentActor);
                ImdbActor actor = ap.ParseDetails();
                CurrentActor = actor;

                LoadImdbInformation li = new LoadImdbInformation(SetImdbInformation);
                this.Invoke(li, new object[] { actor });
            }
            catch (Exception e)
            {
                try
                {
                    ShowErrorInfo si = new ShowErrorInfo(ShowError);
                    this.Invoke(si, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }

        private void SetImdbInformation(ImdbActor actor)
        {
            try
            {
                this.kListControl1.Clear();
                ah = new ImdbMobile.UI.ActorHeader();
                if (string.IsNullOrEmpty(CurrentActor.Bio))
                {
                    ah.Bio = "N/A";
                }
                else
                {
                    ah.Bio = CurrentActor.Bio;
                }
                if (!string.IsNullOrEmpty(CurrentActor.Birthday))
                {
                    ah.Birthday = CurrentActor.Birthday;
                }
                if (!string.IsNullOrEmpty(CurrentActor.Name))
                {
                    ah.Name = CurrentActor.Name;
                }
                if (string.IsNullOrEmpty(CurrentActor.RealName))
                {
                    ah.RealName = CurrentActor.Name;
                }
                else
                {
                    ah.RealName = CurrentActor.RealName;
                    if (string.IsNullOrEmpty(CurrentActor.Name))
                    {
                        ah.Name = CurrentActor.RealName;
                    }
                }
                ah.YIndex = 0;
                ah.Parent = this.kListControl1;
                ah.CalculateHeight();
                this.kListControl1.AddItem(ah);

                if (CurrentActor.Headshot != null)
                {
                    DownloadCover dc = new DownloadCover(CurrentActor.Headshot.URL, this);
                    ImageThread = new System.Threading.Thread(dc.Download);
                    ImageThread.Start();
                }

                UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
                td.Heading = UI.Translations.GetTranslated("0003") + ":";
                td.Parent = this.kListControl1;
                td.Text = CurrentActor.Bio;
                td.YIndex = 1;
                td.CalculateHeight(this.kListControl1.Width);
                this.kListControl1.AddItem(td);

                // Add Holder Control
                UI.ActionButton Filmography = new ImdbMobile.UI.ActionButton();
                Filmography.Icon = global::ImdbMobile.Properties.Resources.Trailers;
                Filmography.HoverIcon = global::ImdbMobile.Properties.Resources.Trailers_Over;
                Filmography.Parent = this.kListControl1;
                Filmography.Text = UI.Translations.GetTranslated("0004");
                Filmography.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Filmography_MouseUp);
                Filmography.YIndex = 2;
                Filmography.CalculateHeight();
                this.kListControl1.AddItem(Filmography);

                // Add Holder Control
                UI.ActionButton Trivia = new ImdbMobile.UI.ActionButton();
                Trivia.Icon = global::ImdbMobile.Properties.Resources.Trivia;
                Trivia.HoverIcon = global::ImdbMobile.Properties.Resources.Trivia_Over;
                Trivia.Parent = this.kListControl1;
                Trivia.Text = UI.Translations.GetTranslated("0005");
                Trivia.YIndex = 3;
                Trivia.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Trivia_MouseUp);
                Trivia.CalculateHeight();
                this.kListControl1.AddItem(Trivia);

            }
            catch (ObjectDisposedException) { }
        }

        void Trivia_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            TriviaControl t = new TriviaControl(CurrentActor);
            UI.WindowHandler.OpenForm(t);
        }

        void Filmography_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            FilmographyControl f = new FilmographyControl(CurrentActor);
            UI.WindowHandler.OpenForm(f);
        }
    }
}
