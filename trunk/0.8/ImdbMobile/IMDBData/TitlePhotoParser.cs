using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitlePhotoParser : APIParser
    {
        public ImdbTitle Title;

        public TitlePhotoParser()
        {

        }

        public void ParsePhotos(ImdbTitle title)
        {
            this.Title = title;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetTitlePhotos(title.ImdbId);
        }

        void APIWorker_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnError(ae.EventData);
        }

        void APIWorker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);
            this.OnParsingData();

            string Response = ae.EventData;
            this.Title.Photos = new List<ImdbPhoto>();

            JObject Obj = JObject.Parse(Response);

            int PhotoCount = 0;

            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "photos"))
                {
                    JToken photos = data["photos"];
                    foreach (JToken photo in photos)
                    {
                        ImdbPhoto ip = new ImdbPhoto();
                        PhotoCount++;
                        if (General.ContainsKey(photo, "caption"))
                        {
                            ip.Caption = (string)photo["caption"];
                        }
                        else if (General.ContainsKey(photo, "copyright"))
                        {
                            ip.Caption = (string)photo["copyright"];
                        }

                        if (General.ContainsKey(photo, "image"))
                        {
                            ip.Image.URL = (string)photo["image"]["url"];
                            ip.Image.Height = (int)photo["image"]["height"];
                            ip.Image.Width = (int)photo["image"]["width"];
                        }
                        this.Title.Photos.Add(ip);
                        // Only download 50 photos
                        if (PhotoCount > 50)
                        {
                            break;
                        }
                    }
                }
            }

            this.OnParsingComplete();
        }
    }
}
