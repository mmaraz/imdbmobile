using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class ComingSoonParser : APIParser
    {
        public List<ImdbSearchResult> Results;

        public ComingSoonParser() { }

        public void ParseComingSoon()
        {
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetComingSoon();
        }

        void APIWorker_Error(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnError(ae.EventData);
        }

        void APIWorker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            string Response = ae.EventData;
            this.OnDownloadComplete(Response);

            this.OnParsingData();
            List<ImdbSearchResult> isrList = new List<ImdbSearchResult>();

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "list"))
                {
                    JToken outerList = data["list"];
                    if (General.ContainsKey(outerList, "list"))
                    {
                        JToken innerList = outerList["list"];
                        foreach (JToken movie in innerList)
                        {
                            if (General.ContainsKey(movie, "list"))
                            {
                                JToken movieList = movie["list"];
                                foreach (JToken title in movieList)
                                {
                                    ImdbTitle it = new ImdbTitle();
                                    it.Title = (string)title["title"];
                                    it.ReleaseDate = (string)movie["label"];
                                    if (General.ContainsKey(title, "image"))
                                    {
                                        it.Cover.URL = (string)title["image"]["url"];
                                        it.Cover.Width = (int)title["image"]["width"];
                                        it.Cover.Height = (int)title["image"]["height"];
                                    }
                                    it.ImdbId = (string)title["tconst"];
                                    string Type = (string)title["type"];
                                    switch (Type)
                                    {
                                        case "feature": it.Type = ImdbTitle.TitleType.FeatureMovie; break;
                                        case "tv_series": it.Type = ImdbTitle.TitleType.TVSeries; break;
                                        case "video_game": it.Type = ImdbTitle.TitleType.VideoGame; break;
                                    }
                                    it.Year = (string)title["year"];
                                    isrList.Add(it);
                                }
                            }
                        }
                    }
                }
            }

            this.Results = isrList;
            this.OnParsingComplete();
        }
    }
}
