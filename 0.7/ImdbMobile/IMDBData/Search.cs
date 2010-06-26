using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class Search : APIParser
    {
        private string Query;
        public List<ImdbSearchResult> Results;
        public int ResultLimit { get; set; }

        public Search()
        {

        }

        public void QueryIMDB(string Query)
        {
            this.Results = new List<ImdbSearchResult>();
            this.Query = Query;

            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(a_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetSearch(Query);
        }

        void a_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);
            this.OnParsingData();
            ParseJSON(ae.EventData);
        }

        private void ParseJSON(string JSON)
        {
            JObject data = JObject.Parse(JSON);
            if (General.ContainsKey(data, "data") && General.ContainsKey(data["data"], "results"))
            {
                ReadResults(data["data"]["results"]);
            }
            this.OnParsingComplete();
        }


        private void ReadResults(JToken Results)
        {
            foreach (JToken Result in Results)
            {
                foreach (JToken Obj in Result["list"])
                {
                    ImdbSearchResult isr = null;
                    if (General.ContainsKey(Obj, "type"))
                    {
                        isr = ParseTitle(Obj);
                    }
                    else if (General.ContainsKey(Obj, "known_for"))
                    {
                        isr = ParseActor(Obj);
                    }
                    if (isr != null)
                    {
                        this.Results.Add(isr);
                        if (this.ResultLimit == this.Results.Count)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public ImdbTitle ParseTitle(JToken Obj)
        {
            ImdbTitle title = new ImdbTitle();
            title.ImdbId = (string)Obj["tconst"];
            string Type = (string)Obj["type"];
            switch (Type)
            {
                case "feature": title.Type = ImdbTitle.TitleType.FeatureMovie; break;
                case "tv_series": title.Type = ImdbTitle.TitleType.TVSeries; break;
                case "video_game": title.Type = ImdbTitle.TitleType.VideoGame; break;
                case "tv_episode": title.Type = ImdbTitle.TitleType.TVSeries; break;
            }
            title.Title = (string)Obj["title"];
            if (General.ContainsKey(Obj, "image"))
            {
                title.Cover.URL = (string)Obj["image"]["url"];
                title.Cover.Width = (int)Obj["image"]["width"];
                title.Cover.Height = (int)Obj["image"]["height"];
            }
            if (General.ContainsKey(Obj, "year"))
            {
                title.Year = (string)Obj["year"];
            }

            if (General.ContainsKey(Obj, "principals"))
            {
                JToken Actors = Obj["principals"];
                foreach (JToken jsd in Actors)
                {
                    title.Actors.Add(ParseActor(jsd));
                }
            }

            return title;
        }

        private ImdbActor ParseActor(JToken Obj)
        {
            ImdbActor actor = new ImdbActor();
            actor.ImdbId = (string)Obj["nconst"];
            if (General.ContainsKey(Obj, "name"))
            {
                actor.Name = (string)Obj["name"];
            }

            if (General.ContainsKey(Obj, "known_for"))
            {
                actor.KnownFor = (string)Obj["known_for"];
            }
            if (General.ContainsKey(Obj, "image"))
            {
                actor.Headshot.Height = (int)Obj["image"]["height"];
                actor.Headshot.Width = (int)Obj["image"]["width"];
                actor.Headshot.URL = (string)Obj["image"]["url"];
            }

            return actor;
        }

        
    }
}
