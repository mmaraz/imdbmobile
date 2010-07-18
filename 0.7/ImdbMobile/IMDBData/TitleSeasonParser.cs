using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleEpisodeParser : APIParser
    {
        public ImdbTitle Title;

        public TitleEpisodeParser()
        {

        }

        public void ParseTitleEpisodes(ImdbTitle title)
        {
            this.Title = title;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetTitleEpisodes(title.ImdbId);
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
            ae = null;

            List<ImdbSeason> Seasons = new List<ImdbSeason>();
            JObject Obj = JObject.Parse(Response);
            Response = null;
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "seasons"))
                {
                    JToken seasons = data["seasons"];
                    foreach (JToken season in seasons)
                    {
                        ImdbSeason isea = new ImdbSeason();
                        isea.ShowTitle = this.Title.Title;
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
                                    string ReldateString = (string)episode["release_date"]["normal"];
                                    if (!string.IsNullOrEmpty(ReldateString))
                                    {
                                        ReldateString = ReldateString.Replace("-", "/");
                                    }
                                    try
                                    {
                                        ep.ReleaseDate = DateTime.Parse(ReldateString);
                                    }
                                    catch (Exception)
                                    {
                                        ep.ReleaseDate = null;
                                    }
                                }
                                ep.Title = (string)episode["title"];
                                try
                                {
                                    ep.Year = int.Parse((string)episode["year"]);
                                }
                                catch (Exception)
                                {
                                    ep.Year = null;
                                }
                                isea.Episodes.Add(ep);
                            }
                        }
                        Seasons.Add(isea);
                    }
                }
            }
            this.Title.Seasons = Seasons;
            this.OnParsingComplete();
        }
    }
}
