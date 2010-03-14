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

namespace ImdbMobile.Controls
{
    public partial class PhotoViewerControl : ImdbMobile.UI.SlidingList
    {
        private delegate void ShowError(string Error);
        private delegate void GetImages();
        private delegate void ShowLoading();

        private static ImdbTitle CurrentTitle;
        private static int StartIndex;
        private static int Pages;
        private static int CurrentPage = 0;

        private System.Threading.Thread ThumbThread;
        private System.Threading.Thread FullThread;

        public PhotoViewerControl(ImdbTitle title)
        {
            CurrentTitle = title;
            InitializeComponent();

            this.Text = UI.Translations.GetTranslated("0052");

            this.ThreadList.Add(ThumbThread);
            this.ThreadList.Add(FullThread);

            UI.KListFunctions.ShowLoading(UI.Translations.GetTranslated("0048") + "...\n" + UI.Translations.GetTranslated("0002") + ".", this.LoadingList);

            ThumbThread = new System.Threading.Thread(LoadImdbInformation);
            ThumbThread.Start();
        }

        private void SetError(string Message)
        {
            try
            {
                UI.KListFunctions.ShowError(Message, this.LoadingList);
            }
            catch (ObjectDisposedException) { }
        }


        private void NextPage()
        {
            if (CurrentPage < Pages)
            {
                CurrentPage++;
                StartIndex = CurrentPage * 6;
                GetImages gi = new GetImages(DownloadImages);
                this.Invoke(gi);
            }
        }

        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                StartIndex = CurrentPage * 6;
                GetImages gi = new GetImages(DownloadImages);
                this.Invoke(gi);
            }
        }

        private void DownloadImages()
        {
            this.label1.Text = UI.Translations.GetTranslated("0049") + " " + CurrentPage + " " + UI.Translations.GetTranslated("0050") + " " + Pages;
            int Counter = 1;
            for (int i = StartIndex; i < StartIndex + 6; i++)
            {
                if (i < CurrentTitle.Photos.Count)
                {
                    DownloadImage(CurrentTitle.Photos[i], true, Counter);
                    if (Counter == 1)
                    {
                        pb1.Image = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Counter + "_thumb.jpg");
                    }
                    else if (Counter == 2)
                    {
                        pb2.Image = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Counter + "_thumb.jpg");
                    }
                    else if (Counter == 3)
                    {
                        pb3.Image = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Counter + "_thumb.jpg");
                    }
                    else if (Counter == 4)
                    {
                        pb4.Image = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Counter + "_thumb.jpg");
                    }
                    else if (Counter == 5)
                    {
                        pb5.Image = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Counter + "_thumb.jpg");
                    }
                    else if (Counter == 6)
                    {
                        pb6.Image = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Counter + "_thumb.jpg");
                    }
                }
                else
                {
                    break;
                }
                Counter++;
            }
            this.LoadingList.Visible = false;
        }

        private void LoadImdbInformation()
        {
            try
            {
                TitlePhotoParser tpp = new TitlePhotoParser();
                CurrentTitle = tpp.ParsePhotos(CurrentTitle);

                if (CurrentTitle.Photos.Count == 0)
                {
                    try
                    {
                        ShowError sr = new ShowError(SetError);
                        this.Invoke(sr, new object[] { UI.Translations.GetTranslated("0051") });
                    }
                    catch (Exception) { }
                    return;
                }

                Pages = (int)Math.Ceiling((double)CurrentTitle.Photos.Count / 6d);

                NextPage();
            }
            catch (Exception e)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    this.Invoke(sr, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }


        private void DownloadImage(ImdbPhoto ip, bool thumb, int ImageIndex)
        {
            try
            {
                WebRequest webReq = null;
                string SavePath = "";
                if (thumb)
                {
                    webReq = (WebRequest)HttpWebRequest.Create(ip.Image.URL.Replace(".jpg", "SX160_SY106.jpg"));
                    SavePath = SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ImageIndex + "_thumb.jpg";
                }
                else
                {
                    webReq = (WebRequest)HttpWebRequest.Create(ip.Image.URL.Replace(".jpg", "SX360_SY600.jpg"));
                    SavePath = SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ImageIndex + ".jpg";
                }
                HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse();

                Stream s = resp.GetResponseStream();
                FileStream fs = File.Open(SavePath, FileMode.Create, FileAccess.Write, FileShare.None);

                int maxRead = 10240;
                byte[] buffer = new byte[maxRead];
                int bytesRead = 0;
                int totalBytesRead = 0;

                while ((bytesRead = s.Read(buffer, 0, maxRead)) > 0)
                {
                    totalBytesRead += bytesRead;
                    fs.Write(buffer, 0, bytesRead);
                }
                fs.Flush();
                fs.Close();
                resp.Close();
            }
            catch (Exception e)
            {
                try
                {
                    ShowError sr = new ShowError(SetError);
                    this.Invoke(sr, new object[] { e.Message });
                }
                catch (Exception) { }
            }
        }

        private void pb1_Click(object sender, EventArgs e)
        {
            GetFullImage(1);
        }

        private void ShowLoadingImage()
        {
            ShowLoading sl = new ShowLoading(LoadingImage);
            this.Invoke(sl);
        }

        private void LoadingImage()
        {
            this.LoadingList.Visible = true;
        }

        private void GetFullImage(int Index)
        {
            ShowLoadingImage();
            DownloadImage(CurrentTitle.Photos[StartIndex], false, Index);
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Index + ".jpg", null);
            p.WaitForExit(2000);
            System.IO.File.Delete(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Index + ".jpg");
            try
            {
                FullThread.Abort();
            }
            catch (Exception) { }
            this.LoadingList.Visible = false;
        }

        private void pb2_Click(object sender, EventArgs e)
        {
            GetFullImage(2);
        }

        private void pb3_Click(object sender, EventArgs e)
        {
            GetFullImage(3);
        }

        private void pb4_Click(object sender, EventArgs e)
        {
            GetFullImage(4);
        }

        private void pb5_Click(object sender, EventArgs e)
        {
            GetFullImage(5);
        }

        private void pb6_Click(object sender, EventArgs e)
        {
            GetFullImage(6);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.LoadingList.Visible = true;
            ThumbThread = new System.Threading.Thread(PreviousPage);
            ThumbThread.Start();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.LoadingList.Visible = true;
            ThumbThread = new System.Threading.Thread(NextPage);
            ThumbThread.Start();
        }
    }
}
