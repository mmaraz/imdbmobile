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

        public UI.ActorHeader ah;
        System.Threading.Thread ImageThread;

        public ActorControl(ImdbActor actor)
        {
            this.CurrentActor = actor;
            InitializeComponent();

            this.Name = UI.Translations.GetTranslated("0008");
            this.Text = UI.Translations.GetTranslated("0009");

            this.ThreadList.Add(ImageThread);

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0001") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
            LoadActorData();
        }

        private void ShowError(string ErrorMessage)
        {
            UI.KListFunctions.ShowLoading(ErrorMessage, this.kListControl1);
        }

        private void LoadActorData()
        {
            IMDBData.ActorParser ap = new ImdbMobile.IMDBData.ActorParser(this.CurrentActor);
            ap.ParsingComplete += new EventHandler(ap_ParsingComplete);
            ap.Error += new EventHandler(ap_Error);
            ap.ParseDetails();
        }

        void ap_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            UI.KListFunctions.ShowError("Error: " + ae.EventData + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.kListControl1);
        }

        void ap_ParsingComplete(object sender, EventArgs e)
        {
            ActorParser ap = (ActorParser)sender;
            SetImdbInformation(ap.OriginalActor);
        }

        private void SetImdbInformation(ImdbActor actor)
        {
            this.kListControl1.Items.Clear();
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
            this.kListControl1.Items.Add(ah);

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
            // When text is longer then 12000 the control freaks out and the text isn't readable. so remove it.
            if (!string.IsNullOrEmpty(td.Text) && td.Text.Length > 12000)
            {
                td.Text = td.Text.Substring(0, 12000);
            }
            td.YIndex = 1;
            td.CalculateHeight();
            this.kListControl1.Items.Add(td);

            // Add Holder Control
            UI.ActionButton Filmography = new ImdbMobile.UI.ActionButton();
            Filmography.Icon = "Trailers";
            Filmography.Parent = this.kListControl1;
            Filmography.Text = UI.Translations.GetTranslated("0004");
            Filmography.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Filmography_MouseUp);
            Filmography.YIndex = 2;
            Filmography.CalculateHeight();
            this.kListControl1.Items.Add(Filmography);

            // Add Holder Control
            UI.ActionButton Trivia = new ImdbMobile.UI.ActionButton();
            Trivia.Icon = "Trivia";
            Trivia.Parent = this.kListControl1;
            Trivia.Text = UI.Translations.GetTranslated("0005");
            Trivia.YIndex = 3;
            Trivia.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(Trivia_MouseUp);
            Trivia.CalculateHeight();
            this.kListControl1.Items.Add(Trivia);
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
