using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ImdbMobile.IMDBData;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace ImdbMobile.Controls
{
    public partial class TrailersControl : ImdbMobile.UI.SlidingList
    {
        private delegate void SetImage();
        private delegate void ShowErrorInfo(string Message);
        private delegate void ShowTrailerVideo(string URL);
        private static ImdbTitle CurrentTitle;
        private static ImdbEncoding SelectedEncoding;

        System.Threading.Thread LoadingThread;

        public TrailersControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0045");
            this.ThreadList.Add(LoadingThread);

            this.LoadingList.Visible = false;

            if (CurrentTitle.Trailer != null)
            {
                foreach (ImdbEncoding ie in CurrentTitle.Trailer.Encodings)
                {
                    UI.ActionButton mpi = new ImdbMobile.UI.ActionButton();
                    switch (ie.Type)
                    {
                        case ImdbEncoding.VideoType.ThreeG: mpi.Text = UI.Translations.GetTranslated("0071"); break;
                        case ImdbEncoding.VideoType.EDGE: mpi.Text = UI.Translations.GetTranslated("0070"); break;
                        case ImdbEncoding.VideoType.HD480p: mpi.Text = UI.Translations.GetTranslated("0096"); break;
                        case ImdbEncoding.VideoType.HD720p: mpi.Text = UI.Translations.GetTranslated("0097"); break;
                    }
                    mpi.Parent = this.kListControl1;
                    mpi.Icon = global::ImdbMobile.Properties.Resources.Trailers;
                    mpi.HoverIcon = global::ImdbMobile.Properties.Resources.Trailers_Over;
                    mpi.YIndex = CurrentTitle.Trailer.Encodings.IndexOf(ie);
                    mpi.MouseUp += new ImdbMobile.UI.ActionButton.MouseEvent(mpi_MouseUp);
                    mpi.CalculateHeight();
                    this.kListControl1.Items.Add(mpi);
                }
                if (CurrentTitle.Trailer.Encodings.Count == 0)
                {
                    try
                    {
                        ShowErrorInfo sr = new ShowErrorInfo(ShowError);
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0072") });
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                try
                {
                    ShowErrorInfo sr = new ShowErrorInfo(ShowError);
                    this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0072") });
                }
                catch (Exception) { }
            }
        }

        void mpi_MouseUp(int X, int Y, MichyPrima.ManilaDotNetSDK.KListControl Parent, UI.ActionButton Sender)
        {
            this.LoadingList.Visible = true;
            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0073") + ".\n" + UI.Translations.GetTranslated("0002") + "...", this.LoadingList);
            SelectedEncoding = CurrentTitle.Trailer.Encodings[Sender.YIndex];

            if (SelectedEncoding.VideoURL.ToLower().Contains("totaleclips"))
            {
                LoadingThread = new System.Threading.Thread(GetTrailerURL);
                LoadingThread.Start();
            }
            else
            {
                ShowTrailer(SelectedEncoding.VideoURL);
            }
        }

        private void ShowTrailer(string URL)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = SettingsWrapper.GlobalSettings.VideoPlayerPath;
                p.StartInfo.Arguments = SettingsWrapper.GlobalSettings.VideoPlayerArguments + " \"" + URL + "\"";
                p.Start();
                this.LoadingList.Visible = false;
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

        private void GetTrailerURL()
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(SelectedEncoding.VideoURL);
                using (WebResponse resp = hwr.GetResponse())
                {

                    string[] pathSplit = resp.ResponseUri.PathAndQuery.Split('/');

                    string MP4URL = "http://" + resp.ResponseUri.Host + "/" + pathSplit[1] + "/";

                    string str = "";
                    using (Stream responseStream = resp.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(responseStream))
                        {
                            str = sr.ReadToEnd();
                        }
                    }

                    Regex r = new Regex("(([a-zA-Z]|[0-9])[0-9]*_[0-9]*).mp4");
                    Match m = r.Match(str);

                    MP4URL += m.Groups[0].Value;

                    try
                    {
                        ShowTrailerVideo stv = new ShowTrailerVideo(ShowTrailer);
                        this.Invoke(stv, new object[] { MP4URL });
                    }
                    catch (ObjectDisposedException) { }
                }
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

        private void ShowError(string Message)
        {
            try
            {
                this.LoadingList.Visible = false;
                UI.KListFunctions.ShowError(Message, this.kListControl1);
            }
            catch (ObjectDisposedException) { }
        }
    }
}
