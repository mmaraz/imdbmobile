using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleGoofParser : APIParser
    {
        public ImdbTitle Title;

        public TitleGoofParser() { }

        public void ParseGoofs(ImdbTitle title)
        {
            this.Title = title;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetTitleGoofs(title.ImdbId);
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
            this.Title.Goofs = new List<ImdbGoof>();

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "unspoilt"))
                {
                    JToken goofs = data["unspoilt"];
                    foreach (JToken goof in goofs)
                    {
                        ImdbGoof g = new ImdbGoof();
                        g.Description = (string)goof["text"];
                        switch ((string)goof["type"])
                        {
                            case "GOOF-FAKE": g.Type = ImdbGoof.GoofType.RevealingMistakes; break;
                            case "GOOF-CONT": g.Type = ImdbGoof.GoofType.Continuity; break;
                            case "GOOF-CREW": g.Type = ImdbGoof.GoofType.CrewOrEquipment; break;
                            case "GOOF-FAIR": g.Type = ImdbGoof.GoofType.IncorrectlyRegarded; break;
                            case "GOOF-PLOT": g.Type = ImdbGoof.GoofType.PlotHoles; break;
                        }
                        this.Title.Goofs.Add(g);
                    }
                }
            }
            this.OnParsingComplete();
        }
    }
}
