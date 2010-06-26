using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class FilmographyParser
    {
        private ImdbActor CurrentActor;

        public FilmographyParser(ImdbActor actor)
        {
            this.CurrentActor = actor;
        }

        public ImdbActor ParseDetails()
        {
            string Response = GetResponse();
            return ParseDetails(Response);
        }

        private ImdbActor ParseDetails(string Response)
        {
            ImdbActor actor = this.CurrentActor;

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                actor.KnownForFull = ParseFilmography(data);
            }
            return actor;
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

        private string GetResponse()
        {
            API a = new API();
            return a.GetActorFilmography(this.CurrentActor.ImdbId);
        }
    }
}
