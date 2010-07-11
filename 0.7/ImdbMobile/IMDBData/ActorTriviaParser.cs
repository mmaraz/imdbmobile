using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class ActorTriviaParser : APIParser
    {
        public ImdbActor Actor;
        public ActorTriviaParser()
        {

        }

        private void GetResponse()
        {
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetActorTrivia(this.Actor.ImdbId);
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
            ParseTrivia(ae.EventData);
            this.OnParsingComplete();
        }

        private void ParseTrivia(string Response)
        {
            this.Actor.Trivia.Clear();
            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "unspoilt"))
                {
                    JToken trivias = data["unspoilt"];
                    foreach (JToken trivia in trivias)
                    {
                        this.Actor.Trivia.Add((string)trivia["text"]);
                    }
                }
                else if (General.ContainsKey(data, "trivia"))
                {
                    JToken trivias = data["trivia"];
                    foreach (JToken trivia in trivias)
                    {
                        this.Actor.Trivia.Add((string)trivia["text"]);
                    }
                }
            }
        }

        public void ParseTitleTrivia(ImdbActor actor)
        {
            this.Actor = actor;
            GetResponse();
        }
    }
}
