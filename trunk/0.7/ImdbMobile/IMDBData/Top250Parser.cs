using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class Top250Parser : APIParser
    {
        public List<ImdbSearchResult> Results;

        public Top250Parser()
        {

        }

        public void ParseTop250()
        {
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetTop250();
        }

        void APIWorker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);

            this.OnParsingData();
            string Response = ae.EventData;

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
                            ImdbTitle title = new ImdbTitle();
                            title.Title = (string)movie["title"];
                            title.Year = (string)movie["year"];
                            title.NumberOfVotes = (int)movie["num_votes"];
                            title.Rating = ParseRating(movie); ;
                            title.ImdbId = (string)movie["tconst"];
                            title.Cover.URL = (string)movie["image"]["url"];
                            title.Cover.Width = (int)movie["image"]["width"];
                            title.Cover.Height = (int)movie["image"]["height"];
                            isrList.Add(title);
                        }
                    }
                }
            }

            this.Results = isrList;
            this.OnParsingComplete();
        }

        private string ParseRating(JToken data)
        {
            if (General.ContainsKey(data, "rating"))
            {
                JValue ratingValue = (JValue)data["rating"];

                if (ratingValue.Value.GetType() == typeof(Int32))
                {
                    return "" + (Int32)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(Int64))
                {
                    return "" + (Int64)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(string))
                {
                    return "" + (string)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(decimal))
                {
                    return "" + (decimal)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(double))
                {
                    return "" + (double)ratingValue.Value;
                }

                return null;
            }
            return null;
        }
    }
}
