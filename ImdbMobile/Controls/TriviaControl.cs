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
        private static List<string> Trivia;

        private delegate void ShowError(string Error);
        private delegate void AddTrivia(List<string> Trivia);
        private delegate ImdbTitle GetTitle();
        private delegate ImdbActor GetActor();

        System.Threading.Thread LoadingThread;

        private delegate void ClearList();
        private delegate void UpdateStatus(int Current, int Total);

        private static int CurrentPage = -1;
        private static int TotalPages = 1;

        private void Update(int Current, int Total)
        {
            ((UI.LoadingButton)this.LoadingList[0]).Text = UI.Translations.GetTranslated("0074") + ".\n(" + Current + " " + UI.Translations.GetTranslated("0050") + " " + Total + ")";
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

        private ImdbTitle GetTitleData()
        {
            try
            {
                return this.CurrentTitle;
            }
            catch (ObjectDisposedException) { }
            return null;
        }

        private ImdbActor GetActorData()
        {
            try
            {
                return this.CurrentActor;
            }
            catch (ObjectDisposedException) { }
            return null;
        }

        public TriviaControl(ImdbActor actor)
        {
            CurrentTitle = null;
            CurrentActor = actor;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0005");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0075") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        public TriviaControl(ImdbTitle title)
        {
            CurrentActor = null;
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0005");

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0076") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);

            LoadingThread = new System.Threading.Thread(LoadImdbInformation);
            LoadingThread.Start();
        }

        private void SetError(string Message)
        {
            try
            {
                UI.KListFunctions.ShowError(Message, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }

        private void AddTriviaItems(List<string> Trivia)
        {
            try
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
                    this.kListControl1.AddItem(tdList[i]);
                    Update(i, Trivia.Count);
                }

                try
                {
                    ClearList cl = new ClearList(Clear);
                    this.Invoke(cl);
                }
                catch (ObjectDisposedException) { }
            }
            catch (ObjectDisposedException) { }
        }


        private void ShowData()
        {
            AddTrivia at = new AddTrivia(AddTriviaItems);
            this.Invoke(at, new object[] { Trivia });

            if (Trivia.Count == 0)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    if (CurrentTitle == null)
                    {
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0077") });
                    }
                    else
                    {
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0078") });
                    }
                }
                catch (Exception) { }
            }
        }

        private void LoadImdbInformation()
        {
            try
            {
                ImdbActor CurrentActor = (ImdbActor)this.Invoke(new GetActor(GetActorData));
                ImdbTitle CurrentTitle = (ImdbTitle)this.Invoke(new GetTitle(GetTitleData));

                if (CurrentTitle == null)
                {
                    ActorTriviaParser atp = new ActorTriviaParser();
                    ImdbActor actor = atp.ParseTitleTrivia(CurrentActor);
                    Trivia = actor.Trivia;
                }
                else
                {
                    TitleTriviaParser ttp = new TitleTriviaParser();
                    ImdbTitle title = ttp.ParseTitleTrivia(CurrentTitle);
                    Trivia = title.Trivia;
                }

                ShowData();
            }
            catch (Exception e)
            {
                try
                {
                    ShowError se = new ShowError(SetError);
                    this.Invoke(se, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }
    }
}
