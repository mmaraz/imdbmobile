using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using ImdbMobile.Controls;
using System.Security.Cryptography;

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

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }


        public DownloadCover(string URL, ImdbMobile.UI.SlidingList Parent)
        {
            this.Parent = Parent;
            _url = URL;
        }
        public void Download()
        {
            if (!string.IsNullOrEmpty(_url))
            {
                // image is available
                string CoverFileName = "";
                string MD5Hash = "";
                string[] splitter = ImageDownloader.GetMoviePoster(_url).Split('/');
                MD5Hash = CalculateMD5Hash(splitter[splitter.Length - 1]);
                if (SettingsWrapper.GlobalSettings.UseCaching && MD5Hash.Length > 5)
                {
                    StringBuilder path = new StringBuilder();
                    path.Append(SettingsWrapper.GlobalSettings.CachePath);
                    path.Append("\\");
                    path.Append(MD5Hash.Substring(0,1));
                    path.Append("\\");
                    path.Append(MD5Hash.Substring(1, 2));
                    path.Append("\\");
                    path.Append(splitter[splitter.Length - 1]);
                    CoverFileName = path.ToString();
                }
                else
                {
                    CoverFileName = SettingsWrapper.GlobalSettings.CachePath + "\\" + splitter[splitter.Length -1];
                }

                

                if (System.IO.File.Exists(CoverFileName))
                {
                    // Image is already downloaded and exists in the cachefolder.
                    System.Drawing.Image i = new System.Drawing.Bitmap(CoverFileName);
                    try
                    {
                        UpdateIcon ui = new UpdateIcon(Update);
                        Parent.Invoke(ui, new object[] { i });
                    }
                    catch (ObjectDisposedException) { }
                }
                else
                {
                    // Download image
                    string Path = ImageDownloader.DownloadImage(_url);
                    System.Drawing.Image i = new System.Drawing.Bitmap(Path);
                    if (!SettingsWrapper.GlobalSettings.UseCaching)
                    {
                        System.IO.File.Delete(Path);
                    }
                    try
                    {
                        UpdateIcon ui = new UpdateIcon(Update);
                        Parent.Invoke(ui, new object[] { i });
                    }
                    catch (ObjectDisposedException) { }
                }
            }
            else
            {
                // No image available
                string imagePath = "";
                if (SettingsWrapper.GlobalSettings.UseBigImages)
                {
                    if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Cache\\no_image_big.png"))
                    {
                        imagePath = ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Cache\\no_image_big.png";
                    }
                }
                else
                {
                    if (System.IO.File.Exists(ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Cache\\no_image_small.png"))
                    {
                        imagePath = ImdbMobile.IMDBData.SettingsWrapper.ApplicationPath + "\\Cache\\no_image_small.png";
                    }
                }
                if (!string.IsNullOrEmpty(imagePath))
                {
                    System.Drawing.Image i = new System.Drawing.Bitmap(imagePath);
                    try
                    {
                        UpdateIcon ui = new UpdateIcon(Update);
                        Parent.Invoke(ui, new object[] { i });
                    }
                    catch (ObjectDisposedException) { }
                }
            }
        }
    }

    public class ImageDownloader
    {
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void Kill()
        {
            if (diw != null)
            {
                try
                {
                    diw.Abort();
                }
                catch (Exception) { }
            }
            if (webReq != null)
            {
                try
                {
                    webReq.Abort();
                }
                catch (Exception) { }
            }
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
        private static HttpWebRequest webReq;
        DownloadImageWrapper diw;


        public ImageDownloader()
        {

        }

        public static string GetMoviePoster(string URL)
        {
            if (SettingsWrapper.GlobalSettings.UseBigImages)
            {
                return URL.Replace(".jpg", "SX189_SY270.jpg");
            }
            else
            {
                return URL.Replace(".jpg", "SX108_SY154.jpg");
            }
            
        }

        public static string DownloadImage(string URL)
        {
            try
            {
                if (!string.IsNullOrEmpty(URL))
                {
                    webReq = (HttpWebRequest)WebRequest.Create(GetMoviePoster(URL));
                    using (HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse())
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            string MD5Hash = "";
                            string CoverFileName = "";
                            string[] splitter = ImageDownloader.GetMoviePoster(URL).Split('/');

                            MD5Hash = CalculateMD5Hash(splitter[splitter.Length - 1]);
                            if (SettingsWrapper.GlobalSettings.UseCaching && MD5Hash.Length > 5)
                            {
                                StringBuilder path = new StringBuilder();
                                path.Append(SettingsWrapper.GlobalSettings.CachePath);
                                path.Append("\\");
                                path.Append(MD5Hash.Substring(0, 1));
                                path.Append("\\");
                                path.Append(MD5Hash.Substring(1, 2));
                                path.Append("\\");

                                if (!Directory.Exists(path.ToString()))
                                {
                                    Directory.CreateDirectory(path.ToString());
                                }

                                path.Append(splitter[splitter.Length - 1]);
                                CoverFileName = path.ToString();

                            }
                            else
                            {
                                CoverFileName = SettingsWrapper.GlobalSettings.CachePath + "\\" + splitter[splitter.Length - 1];
                            }

                            using (FileStream fs = File.Open(CoverFileName, FileMode.Create, FileAccess.Write, FileShare.None))
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

                            return CoverFileName;
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
            diw = new DownloadImageWrapper(ImageList, KList, ParentForm);
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
        private HttpWebRequest webReq;

        public DownloadImageWrapper(List<ImdbCover> Images, MichyPrima.ManilaDotNetSDK.KListControl ParentKList, System.Windows.Forms.Form ParentFormControl)
        {
            this.Images = Images;
            this.ParentKList = ParentKList;
            this.ParentFormControl = ParentFormControl;
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void Abort()
        {
            if (webReq != null)
            {
                try
                {
                    webReq.Abort();
                }
                catch (Exception) { }
            }
        }

        public void DownloadImage()
        {
            // Download the thumbnail images
            foreach (ImdbCover ic in Images)
            {
                try
                {
                    string CoverFileName = "";
                    if (ic != null && !string.IsNullOrEmpty(ic.URL))
                    {
                        string MD5Hash = "";
                        string[] splitter = ic.URL.Split('/');
                        MD5Hash = CalculateMD5Hash(splitter[splitter.Length - 1]);
                        if (SettingsWrapper.GlobalSettings.UseCaching && MD5Hash.Length > 5)
                        {
                            StringBuilder path = new StringBuilder();
                            path.Append(SettingsWrapper.GlobalSettings.CachePath);
                            path.Append("\\");
                            path.Append(MD5Hash.Substring(0, 1));
                            path.Append("\\");
                            path.Append(MD5Hash.Substring(1, 2));
                            path.Append("\\");
                            path.Append(splitter[splitter.Length - 1]);
                            CoverFileName = path.ToString();
                        }
                        else
                        {
                            CoverFileName = SettingsWrapper.GlobalSettings.CachePath + "\\" + splitter[splitter.Length - 1];
                        }
                    }

                    if (!string.IsNullOrEmpty(CoverFileName))
                    {
                        if (System.IO.File.Exists(CoverFileName))
                        {
                            try
                            {
                                int Index = Images.IndexOf(ic);
                                ChangeImage ci = new ChangeImage(SetImage);
                                ParentFormControl.Invoke(ci, new object[] { CoverFileName, Index });
                            }
                            catch (ObjectDisposedException)
                            {
                                // If the parent has been killed, exit the thread
                                break;
                            }
                        }
                        else
                        {
                            string FileName = GetImageFromWeb(ic);
                            int Index = Images.IndexOf(ic);
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
                            if (!SettingsWrapper.GlobalSettings.UseCaching)
                            {
                                System.IO.File.Delete(FileName);
                            }
                        }
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
                b.Dispose();
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
                    string MD5Hash = "";
                    MD5Hash = CalculateMD5Hash(splitter[splitter.Length - 1]);
                    if (SettingsWrapper.GlobalSettings.UseCaching && MD5Hash.Length > 5)
                    {
                        StringBuilder path = new StringBuilder();
                        path.Append(SettingsWrapper.GlobalSettings.CachePath);
                        path.Append("\\");
                        path.Append(MD5Hash.Substring(0, 1));
                        path.Append("\\");
                        path.Append(MD5Hash.Substring(1, 2));
                        path.Append("\\");

                        if (!Directory.Exists(path.ToString()))
                        {
                            Directory.CreateDirectory(path.ToString());
                        }

                        path.Append(splitter[splitter.Length - 1]);
                        fileName = path.ToString();

                    }
                    else
                    {
                        fileName = SettingsWrapper.GlobalSettings.CachePath + "\\" + splitter[splitter.Length - 1];
                    }

                    webReq = (HttpWebRequest)WebRequest.Create(GetThumbnailURL(ic.URL));
                    using (HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse())
                    {
                        using (Stream s = resp.GetResponseStream())
                        {
                            using (FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
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

                    return fileName;
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}
