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
            this.LoadingList.Dispose();
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
            // Ensure the most relevant entries are at the top
            List<ImdbKnownForGroup> priority = this.CurrentActor.KnownForFull.Where(ikf => ikf.Label == "Actor" || ikf.Label == "Actress" || ikf.Label == "Director" || ikf.Label == "Producer").OrderBy(ikf => ikf.Label).ToList();
            List<ImdbKnownForGroup> nonpriority = this.CurrentActor.KnownForFull.Where(ikf => ikf.Label != "Actor" && ikf.Label != "Actress" && ikf.Label != "Director" && ikf.Label != "Producer").ToList();
            if (nonpriority != null && nonpriority.Count > 0)
            {
                priority.AddRange(nonpriority);
            }
            foreach (ImdbKnownForGroup ikg in priority)
            {
                UI.ActionButton ab = new ImdbMobile.UI.ActionButton();
                switch (ikg.Label)
                {
                    case "Producer": ab.Icon = "Producer"; break;
                    case "Director": ab.Icon = "Director"; break;
                    case "Actor": ab.Icon = "Actor"; break;
                    case "Actress": ab.Icon = "Actress"; break;
                    default: ab.Icon = "MiscCrew"; break;
                }

                switch (ikg.Label)
                {
                    case "Producer": ab.Text = UI.Translations.GetTranslated("0114"); break;
                    case "Director": ab.Text = UI.Translations.GetTranslated("0115"); break;
                    case "Actor": ab.Text = UI.Translations.GetTranslated("0116"); break;
                    case "Actress": ab.Text = UI.Translations.GetTranslated("0117"); break;
                    case "Writer": ab.Text = UI.Translations.GetTranslated("0118"); break;
                    case "Thanks": ab.Text = UI.Translations.GetTranslated("0119"); break;
                    case "Self": ab.Text = UI.Translations.GetTranslated("0120"); break;
                    case "Soundtrack": ab.Text = UI.Translations.GetTranslated("0121"); break;
                    case "Music Department": ab.Text = UI.Translations.GetTranslated("0123"); break;
                    case "Miscellaneous Crew": ab.Text = UI.Translations.GetTranslated("0124"); break;
                    default: ab.Text = ikg.Label; break;
                }
                ab.YIndex = this.CurrentActor.KnownForFull.IndexOf(ikg);
                ab.Parent = this.kListControl1;
                ab.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(ab_MouseUp);
                ab.CalculateHeight();
                this.kListControl1.Items.Add(ab);
            }
            this.LoadingList.Dispose();
        }

        void ab_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, ImdbMobile.UI.ActionButton Sender)
        {
            FilmographyListControl flc = new FilmographyListControl(this.CurrentActor.KnownForFull[Sender.YIndex].KnownForList);
            UI.WindowHandler.OpenForm(flc);
            flc.LoadImdbInformation();
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
            this.LoadingList.Dispose();
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
