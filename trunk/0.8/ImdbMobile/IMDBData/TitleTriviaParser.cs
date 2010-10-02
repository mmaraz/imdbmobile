using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleTriviaParser : APIParser
    {
        public ImdbTitle Title;

        public TitleTriviaParser()
        {

        }

        public void ParseTitleTrivia(ImdbTitle title)
        {
            this.Title = title;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetTitleTrivia(title.ImdbId);
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
            this.Title.Trivia = new List<string>();

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "unspoilt"))
                {
                    JToken trivias = data["unspoilt"];
                    foreach (JToken trivia in trivias)
                    {
                        this.Title.Trivia.Add((string)trivia["text"]);
                    }
                }
            }

            this.OnParsingComplete();
        }
    }
}
