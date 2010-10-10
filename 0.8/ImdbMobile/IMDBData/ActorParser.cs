using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class ActorParser : APIParser
    {
        public ImdbActor OriginalActor;

        public ActorParser(ImdbActor actor)
        {
            this.OriginalActor = actor;
        }

        public void ParseDetails()
        {
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(Worker_DataDownloaded);
            UI.WindowHandler.APIWorker.GetActorDetails(this.OriginalActor.ImdbId);
        }

        void APIWorker_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnError(ae.EventData);
        }

        void Worker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);
            this.OnParsingData();
            this.OriginalActor = ParseDetails(ae.EventData);
            this.OnParsingComplete();
        }

        public ImdbActor ParseDetails(string Response)
        {
            ImdbActor actor = this.OriginalActor;

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                actor.ImdbId = ParseImdbId(data);
                actor.Headshot = ParseHeadshot(data);
                actor.Photos = ParsePhotos(data);
                actor.Birthday = ParseBirthday(data);
                actor.RealName = ParseRealName(data);
                //actor.KnownForFull = ParseKnownFor(data);
                actor.Bio = ParseBio(data);
            }
            return actor;
        }

        private string ParseImdbId(JToken data)
        {
            return (string)data["nconst"];
        }

        private ImdbCover ParseHeadshot(JToken data)
        {
            ImdbCover ic = new ImdbCover();
            if (General.ContainsKey(data, "image"))
            {
                JToken image = data["image"];
                ic.URL = (string)image["url"];
                ic.Width = (int)image["width"];
                ic.Height = (int)image["height"];
                return ic;
            }
            return ic;
        }

        private string ParseBio(JToken data)
        {
            if (General.ContainsKey(data, "bio"))
            {
                return (string)data["bio"];
            }
            return null;
        }

        private List<ImdbKnownFor> ParseKnownFor(JToken data)
        {
            List<ImdbKnownFor> retList = new List<ImdbKnownFor>();
            if (General.ContainsKey(data, "known_for"))
            {
                JToken knownforlist = data["known_for"];
                foreach (JToken knownfor in knownforlist)
                {
                    ImdbKnownFor kf = new ImdbKnownFor();
                    kf.TitleAttribute = (string)knownfor["attr"];
                    if (General.ContainsKey(knownfor, "title"))
                    {
                        JToken title = knownfor["title"];
                        kf.Title = (string)title["title"];
                        kf.ImdbId = (string)title["tconst"];
                        switch ((string)title["type"])
                        {
                            case "feature": kf.Type = ImdbTitle.TitleType.FeatureMovie; break;
                            case "tv_series": kf.Type = ImdbTitle.TitleType.TVSeries; break;
                            case "video_game": kf.Type = ImdbTitle.TitleType.VideoGame; break;
                        }
                        kf.Year = (string)title["year"];
                    }
                    retList.Add(kf);
                    
                }
            }
            return retList;
        }

        private string ParseRealName(JToken data)
        {
            if (General.ContainsKey(data, "real_name"))
            {
                return (string)data["real_name"];
            }
            return null;
        }

        private string ParseBirthday(JToken data)
        {
            if (General.ContainsKey(data, "birth"))
            {
                JToken date = data["birth"];
                if (General.ContainsKey(date, "date"))
                {
                    JToken date2 = date["date"];
                    if (General.ContainsKey(date2, "normal"))
                    {
                        return (string)date2["normal"];
                    }
                }
            }
            return null;
        }

        private List<ImdbCover> ParsePhotos(JToken data)
        {
            List<ImdbCover> retList = new List<ImdbCover>();
            if (General.ContainsKey(data, "photos"))
            {
                JToken photos = data["photos"];
                foreach (JToken photo in photos)
                {
                    if (General.ContainsKey(photo, "image"))
                    {
                        JToken image = photo["image"];
                        ImdbCover cover = new ImdbCover();
                        cover.URL = (string)image["url"];
                        cover.Width = (int)image["width"];
                        cover.Height = (int)image["height"];
                        retList.Add(cover);
                    }
                }
            }
            return retList;
        }
    }
}
