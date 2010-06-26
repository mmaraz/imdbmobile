using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleParentalGuideParser : APIParser
    {
        public ImdbTitle Title;

        public TitleParentalGuideParser()
        {

        }

        public void ParseParentalGuide(ImdbTitle it)
        {
            this.Title = it;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetParentalGuide(it.ImdbId);
        }

        void APIWorker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);

            this.OnParsingData();
            string Response = ae.EventData;

            JObject Obj = JObject.Parse(Response);

            Dictionary<string, string> Guides = new Dictionary<string, string>();
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "parental_guide"))
                {
                    JToken guide = data["parental_guide"];
                    foreach (JToken guideInfo in guide)
                    {
                        Guides.Add((string)guideInfo["label"], (string)guideInfo["text"]);
                    }
                }
            }
            this.Title.ParentalGuide = Guides;
            this.OnParsingComplete();
        }
    }
}
