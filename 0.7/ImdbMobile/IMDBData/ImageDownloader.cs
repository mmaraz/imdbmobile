using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using ImdbMobile.Controls;

namespace ImdbMobile.IMDBData
{
    class DownloadCover
    {
        private string _url;
        private ImdbMobile.UI.SlidingList Parent;
        private delegate void UpdateIcon(System.Drawing.Image i);

        private void Update(System.Drawing.Image i)
        {
            if (Parent.GetType() == typeof(MovieControl))
            {
                ((MovieControl)Parent).mh.Icon = i;
            }
            else if (Parent.GetType() == typeof(ActorControl))
            {
                ((ActorControl)Parent).ah.Icon = i;
            }
        }

        public DownloadCover(string URL, ImdbMobile.UI.SlidingList Parent)
        {
            this.Parent = Parent;
            _url = URL;
        }
        public void Download()
        {
            string Path = ImageDownloader.DownloadImage(_url);
            if (Path != null)
            {
                System.Drawing.Image i = new System.Drawing.Bitmap(Path);
                System.IO.File.Delete(Path);
                try
                {
                    UpdateIcon ui = new UpdateIcon(Update);
                    Parent.Invoke(ui, new object[] { i });
                }
                catch (ObjectDisposedException) { }
            }
        }
    }

    public class ImageDownloader
    {
        public void Kill()
        {
            if (Worker != null)
            {
                try
                {
                    Worker.Abort();
                }
                catch (Exception) { }
            }
        }

        System.Threading.Thread Worker;

        public ImageDownloader()
        {

        }

        private static string GetMoviePoster(string URL)
        {
            return URL.Replace(".jpg", "SX108_SY154.jpg");
        }

        public static string DownloadImage(string URL)
        {
            try
            {
                if (!string.IsNullOrEmpty(URL))
                {
                    WebRequest webReq = (WebRequest)HttpWebRequest.Create(GetMoviePoster(URL));
                    using (HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse())
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            using (FileStream fs = File.Open(SettingsWrapper.GlobalSettings.CachePath + "\\tempimg.jpg", FileMode.Create, FileAccess.Write, FileShare.None))
                            {
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
                            }

                            return SettingsWrapper.GlobalSettings.CachePath + "\\tempimg.jpg";
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public void DownloadImages(List<ImdbSearchResult> isrList, MichyPrima.ManilaDotNetSDK.KListControl KList, System.Windows.Forms.Form ParentForm)
        {
            if (SettingsWrapper.GlobalSettings.DownloadThumbnails)
            {
                List<ImdbCover> tempList = new List<ImdbCover>();
                foreach (ImdbSearchResult isr in isrList)
                {
                    if (isr.ResultType == ImdbSearchResult.ResultTypeList.Actor)
                    {
                        tempList.Add(((ImdbActor)isr).Headshot);
                    }
                    else
                    {
                        tempList.Add(((ImdbTitle)isr).Cover);
                    }
                }
                DownloadImages(tempList, KList, ParentForm);
            }
        }

        public void DownloadImages(List<ImdbActor> actorList, MichyPrima.ManilaDotNetSDK.KListControl KList, System.Windows.Forms.Form ParentForm)
        {
            if (SettingsWrapper.GlobalSettings.DownloadThumbnails)
            {
                List<ImdbCover> tempList = new List<ImdbCover>();
                foreach (ImdbActor ia in actorList)
                {
                    tempList.Add(ia.Headshot);
                }
                DownloadImages(tempList, KList, ParentForm);
            }
        }

        public void DownloadImages(List<ImdbKnownFor> actorList, MichyPrima.ManilaDotNetSDK.KListControl KList, System.Windows.Forms.Form ParentForm)
        {
            if (SettingsWrapper.GlobalSettings.DownloadThumbnails)
            {
                List<ImdbCover> tempList = new List<ImdbCover>();
                foreach (ImdbKnownFor ia in actorList)
                {
                    tempList.Add(ia.Cover);
                }
                DownloadImages(tempList, KList, ParentForm);
            }
        }

        public void DownloadImages(List<ImdbCharacter> actorList, MichyPrima.ManilaDotNetSDK.KListControl KList, System.Windows.Forms.Form ParentForm)
        {
            if (SettingsWrapper.GlobalSettings.DownloadThumbnails)
            {
                List<ImdbCover> tempList = new List<ImdbCover>();
                foreach (ImdbCharacter ia in actorList)
                {
                    tempList.Add(ia.Headshot);
                }
                DownloadImages(tempList, KList, ParentForm);
            }
        }

        private void DownloadImages(List<ImdbCover> ImageList, MichyPrima.ManilaDotNetSDK.KListControl KList, System.Windows.Forms.Form ParentForm)
        {
            DownloadImageWrapper diw = new DownloadImageWrapper(ImageList, KList, ParentForm);
            this.Kill();
            Worker = new System.Threading.Thread(diw.DownloadImage);
            Worker.Start();
        }

    }

    class DownloadImageWrapper
    {
        private delegate void ChangeImage(string FileName, int Index);

        private List<ImdbCover> Images;
        private MichyPrima.ManilaDotNetSDK.KListControl ParentKList;
        private System.Windows.Forms.Form ParentFormControl;

        public DownloadImageWrapper(List<ImdbCover> Images, MichyPrima.ManilaDotNetSDK.KListControl ParentKList, System.Windows.Forms.Form ParentFormControl)
        {
            this.Images = Images;
            this.ParentKList = ParentKList;
            this.ParentFormControl = ParentFormControl;
        }

        public void DownloadImage()
        {
            foreach (ImdbCover ic in Images)
            {
                try
                {
                    string FileName = GetImageFromWeb(ic);
                    int Index = Images.IndexOf(ic);
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        if (System.IO.File.Exists(FileName))
                        {
                            try
                            {
                                ChangeImage ci = new ChangeImage(SetImage);
                                ParentFormControl.Invoke(ci, new object[] { FileName, Index });
                            }
                            catch (ObjectDisposedException)
                            {
                                // If the parent has been killed, exit the thread
                                break;
                            }
                        }
                        System.IO.File.Delete(FileName);
                        System.Threading.Thread.Sleep(10);
                    }
                }
                catch (Exception e) { }
            }
        }

        private void SetImage(string FileName, int Index)
        {
            try
            {
                MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi = ((MichyPrima.ManilaDotNetSDK.ManilaPanelItem)ParentKList.Items[Index]);
                System.Drawing.Bitmap b = new System.Drawing.Bitmap(FileName);
                int Width = (mpi.Height / 3) * 2;
                System.Drawing.Image i = Extensions.Resize(b, new System.Drawing.Size(Width, mpi.Height));
                mpi.Image = i;
                ParentKList.Refresh();
            }
            catch (Exception) { }
        }

        private string GetThumbnailURL(string URL)
        {
            return URL.Replace(".jpg", "SX49_SY70.jpg");
        }

        private string GetImageFromWeb(ImdbCover ic)
        {
            try
            {
                if (ic != null && !string.IsNullOrEmpty(ic.URL))
                {
                    string[] splitter = ic.URL.Split('/');
                    string fileName = splitter[splitter.Length - 1];
                    WebRequest webReq = (WebRequest)HttpWebRequest.Create(GetThumbnailURL(ic.URL));
                    using (HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse())
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            using (FileStream fs = File.Open(SettingsWrapper.GlobalSettings.CachePath + "\\" + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {

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
                            }
                        }
                    }

                    return SettingsWrapper.GlobalSettings.CachePath + "\\" + fileName;
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
