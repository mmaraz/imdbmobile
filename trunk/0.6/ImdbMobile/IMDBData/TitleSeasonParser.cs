using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleEpisodeParser
    {
        public TitleEpisodeParser()
        {

        }

        public ImdbTitle ParseTitleEpisodes(ImdbTitle title)
        {
            API a = new API();
            string Response = a.GetTitleEpisodes(title.ImdbId);

            List<ImdbSeason> Seasons = new List<ImdbSeason>();
            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "seasons"))
                {
                    JToken seasons = data["seasons"];
                    foreach (JToken season in seasons)
                    {
                        ImdbSeason isea = new ImdbSeason();
                        isea.Label = (string)season["label"];
                        isea.Episodes = new List<ImdbEpisode>();
                        if (General.ContainsKey(season, "list"))
                        {
                            foreach (JToken episode in season["list"])
                            {
                                ImdbEpisode ep = new ImdbEpisode();
                                ep.ImdbId = (string)episode["tconst"];
                                if (General.ContainsKey(episode["release_date"], "normal"))
                                {
                                    ep.ReleaseDate = DateTime.Parse((string)episode["release_date"]["normal"]);
                                }
                                ep.Title = (string)episode["title"];
                                ep.Year = int.Parse((string)episode["year"]);
                                isea.Episodes.Add(ep);
                            }
                        }
                        Seasons.Add(isea);
                    }
                }
            }
            title.Seasons = Seasons;
            return title;
        }
    }
}
