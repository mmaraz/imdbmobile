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
        private static int Index;

        private System.Threading.Thread ThumbThread;
        private System.Threading.Thread FullThread;

        private static Size SmallImageDimensions;
        private static Size LargeImageDimensions;

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

            this.ThreadList.Add(ThumbThread);
        }

        private void SetError(string Message)
        {
            try
            {
                UI.KListFunctions.ShowError(Message, this.LoadingList);
            }
            catch (ObjectDisposedException) { }
        }


        private void CreateHolders()
        {
            try
            {
                GetImages gi = delegate
                {
                    int YIndex = 0;
                    SmallImageDimensions = new Size((this.kListControl1.Width - 20) / 3, (this.kListControl1.Height - 20) / 3);
                    LargeImageDimensions = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

                    int ControlCount = (int)Math.Floor((double)CurrentTitle.Photos.Count / 3.0);
                    // Loop through each photo control
                    for (int i = 1; i <= ControlCount; i++)
                    {
                        for (int x = 1; x <= 3; x++)
                        {
                            YIndex++;
                            if ((i + x) < CurrentTitle.Photos.Count)
                            {
                                UI.PhotoDisplay pd = new ImdbMobile.UI.PhotoDisplay();
                                pd.MouseUp += new ImdbMobile.UI.PhotoDisplay.MouseEvent(pd_MouseUp);
                                pd.Parent = this.kListControl1;
                                pd.YIndex = YIndex;
                                pd.CalculateHeight(Screen.PrimaryScreen.WorkingArea.Width);
                                this.kListControl1.AddItem(pd);
                            }
                        }
                    }
                    this.LoadingList.Visible = false;
                };
                this.Invoke(gi);
            }
            catch (Exception e) { }
        }

        void pd_MouseUp(ImdbMobile.UI.PhotoDisplay Sender, int PhotoIndex)
        {
            Index = (Sender.YIndex * 3) + (PhotoIndex - 1);
            GetFullImage();
        }

        private void DownloadImages()
        {
            int ControlCount = (int)Math.Floor((double)CurrentTitle.Photos.Count / 3.0);
            // Loop through each photo control
            for (int i = 1; i <= ControlCount; i++)
            {
                for (int x = 1; x <= 3; x++)
                {
                    if ((i + x) < CurrentTitle.Photos.Count)
                    {
                        try
                        {
                            GetImages gi = delegate
                            {
                                this.LoadingList.Visible = false;
                                UI.PhotoDisplay pd = (UI.PhotoDisplay)this.kListControl1[(i - 1)];
                                ImdbPhoto CurrentPhoto = CurrentTitle.Photos[(pd.YIndex * 3) + (x - 1)];
                                DownloadImage(CurrentPhoto, true, (pd.YIndex * 3) + (x - 1));
                                switch (x)
                                {
                                    case 1: pd.Image1 = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ((pd.YIndex * 3) + (x - 1)) + "_thumb.jpg"); break;
                                    case 2: pd.Image2 = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ((pd.YIndex * 3) + (x - 1)) + "_thumb.jpg"); break;
                                    case 3: pd.Image3 = new Bitmap(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ((pd.YIndex * 3) + (x - 1)) + "_thumb.jpg"); break;
                                }
                                System.IO.File.Delete(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ((pd.YIndex * 3) + (x - 1)) + "_thumb.jpg");
                            };
                            this.Invoke(gi);
                        }
                        catch (Exception) { }
                    }
                }
            }
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

                CreateHolders();

                DownloadImages();

                try
                {
                    GetImages giFull = delegate
                    {
                        FullThread = new System.Threading.Thread(DownloadImages);
                        FullThread.Start();

                        this.ThreadList.Add(FullThread);
                    };
                    this.Invoke(giFull);
                }
                catch (Exception) { }
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
                    webReq = (WebRequest)HttpWebRequest.Create(ip.Image.URL.Replace(".jpg", "SX" + SmallImageDimensions.Width + "_SY" + SmallImageDimensions.Height + ".jpg"));
                    SavePath = SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + ImageIndex + "_thumb.jpg";
                }
                else
                {
                    webReq = (WebRequest)HttpWebRequest.Create(ip.Image.URL.Replace(".jpg", "SX" + LargeImageDimensions.Width + "_SY" + LargeImageDimensions.Height + ".jpg"));
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

        private void ShowLoadingImage()
        {
            ShowLoading sl = new ShowLoading(LoadingImage);
            this.Invoke(sl);
        }

        private void LoadingImage()
        {
            UI.KListFunctions.ShowLoading("Downloading Image", this.LoadingList);
            this.LoadingList.Visible = true;
        }

        private void GetFullImage()
        {
            ShowLoadingImage();
            DownloadImage(CurrentTitle.Photos[Index], false, Index);
            this.LoadingList.Visible = false;
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Index + ".jpg", null);

            System.Threading.Thread DownloadThread = new System.Threading.Thread(DeleteImage);
        }

        private void DeleteImage()
        {
            System.Threading.Thread.Sleep(20000);
            System.IO.File.Delete(SettingsWrapper.GlobalSettings.CachePath + "\\Photo" + Index + ".jpg");
            this.LoadingList.Visible = false;
        }

    }
}
