using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitlePhotoParser
    {
        public TitlePhotoParser()
        {

        }

        public ImdbTitle ParsePhotos(ImdbTitle title)
        {
            API a = new API();
            string Response = a.GetTitlePhotos(title.ImdbId);
            title.Photos = new List<ImdbPhoto>();

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
                        title.Photos.Add(ip);
                        // Only download 50 photos
                        if (PhotoCount > 50)
                        {
                            break;
                        }
                    }
                }
            }

            return title;
        }
    }
}
