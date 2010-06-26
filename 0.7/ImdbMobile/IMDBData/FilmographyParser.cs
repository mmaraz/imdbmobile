using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class FilmographyParser : APIParser
    {
        public ImdbActor CurrentActor;

        public FilmographyParser(ImdbActor actor)
        {
            this.CurrentActor = actor;
        }

        public void ParseDetails()
        {
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetActorFilmography(this.CurrentActor.ImdbId);
        }

        void APIWorker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);
            this.OnParsingData();
            ParseDetails(ae.EventData);
            this.OnParsingComplete();
        }

        private void ParseDetails(string Response)
        {
            ImdbActor actor = this.CurrentActor;

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                actor.KnownForFull = ParseFilmography(data);
            }
            this.CurrentActor = actor;
        }

        private List<ImdbKnownFor> ParseFilmography(JToken data)
        {
            List<ImdbKnownFor> retList = new List<ImdbKnownFor>();
            if (General.ContainsKey(data, "filmography"))
            {
                JToken films = data["filmography"];
                foreach (JToken film in films)
                {
                    string Label = (string)film["label"];
                    if (General.ContainsKey(film, "list"))
                    {
                        JToken filmList = film["list"];
                        foreach (JToken knownFor in filmList)
                        {
                            ImdbKnownFor kf = new ImdbKnownFor();
                            if (General.ContainsKey(knownFor, "char"))
                            {
                                kf.CharacterName = (string)knownFor["char"];
                                JToken title = knownFor["title"];
                                kf.ImdbId = (string)title["tconst"];
                                kf.Title = (string)title["title"];
                                switch ((string)title["type"])
                                {
                                    case "feature": kf.Type = ImdbTitle.TitleType.FeatureMovie; break;
                                    case "tv_series": kf.Type = ImdbTitle.TitleType.TVSeries; break;
                                    case "video_game": kf.Type = ImdbTitle.TitleType.VideoGame; break;
                                }
                                kf.Year = (string)title["year"];
                                kf.TitleAttribute = Label;
                                retList.Add(kf);
                            }
                        }
                    }
                }
            }
            return retList;
        }
    }
}
