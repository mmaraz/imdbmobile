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
    public partial class TriviaControl : ImdbMobile.UI.SlidingList
    {
        private ImdbTitle CurrentTitle;
        private ImdbActor CurrentActor;
        private List<string> Trivia;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList.Items[0]).Text = UI.Translations.GetTranslated("0074") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
            this.LoadingList.Invalidate();
        }

        private void Clear()
        {
            this.LoadingList.Visible = false;
        }

        public TriviaControl(ImdbActor actor)
        {
            CurrentTitle = null;
            CurrentActor = actor;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0005");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0075") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadImdbInformation();
        }

        public TriviaControl(ImdbTitle title)
        {
            CurrentActor = null;
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0005");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0076") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadImdbInformation();
        }

        private void SetError(string Message)
        {
            UI.KListFunctions.ShowError(Message, this.kListControl1);
        }

        private void AddTriviaItems(List<string> Trivia)
        {
            List<UI.TextDisplay> tdList = new List<ImdbMobile.UI.TextDisplay>();
            for (int i = 0; i < Trivia.Count; i++)
            {
                UI.TextDisplay td = new ImdbMobile.UI.TextDisplay();
                td.Text = Trivia[i];
                td.YIndex = i;
                td.Parent = this.kListControl1;
                td.ShowSeparator = true;
                td.CalculateHeight();
                tdList.Add(td);
            }
            for (int i = 0; i < Trivia.Count; i++)
            {
                this.kListControl1.Items.Add(tdList[i]);
                Update(i, Trivia.Count);
            }

            Clear();
        }


        private void ShowData()
        {
            AddTriviaItems(Trivia);

            if (Trivia.Count == 0)
            {
                if (CurrentTitle == null)
                {
                    SetError(UI.Translations.GetTranslated("0077"));
                }
                else
                {
                    SetError(UI.Translations.GetTranslated("0078"));
                }
            }
        }

        private void LoadImdbInformation()
        {
            if (this.Trivia != null && this.Trivia.Count > 0)
            {
                ShowData();
            }
            else
            {
                if (CurrentTitle == null)
                {
                    ActorTriviaParser atp = new ActorTriviaParser();
                    atp.ParsingComplete += new EventHandler(atp_ParsingComplete);
                    atp.ParseTitleTrivia(CurrentActor);
                }
                else
                {
                    TitleTriviaParser ttp = new TitleTriviaParser();
                    ttp.ParsingComplete += new EventHandler(ttp_ParsingComplete);
                    ttp.ParseTitleTrivia(CurrentTitle);
                }
            }
        }

        void ttp_ParsingComplete(object sender, EventArgs e)
        {
            TitleTriviaParser ttp = (TitleTriviaParser)sender;
            this.Trivia = ttp.Title.Trivia;
            ShowData();
        }

        void atp_ParsingComplete(object sender, EventArgs e)
        {
            ActorTriviaParser atp = (ActorTriviaParser)sender;
            this.Trivia = atp.Actor.Trivia;
            ShowData();
        }
    }
}
