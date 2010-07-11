using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class ParseCast : APIParser
    {
        public ImdbTitle Title;

        public ParseCast()
        {

        }

        public void ParseFullCast(ImdbTitle title)
        {
            this.Title = title;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetFullCast(title.ImdbId);
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
            ParseFullCast(ae.EventData);
            this.OnParsingComplete();
            this.Title.HasFullCast = true;
        }

        private void ParseFullCast(string Response)
        {
            this.Title.Cast = new List<ImdbCharacter>();
            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "credits"))
                {
                    JToken credits = data["credits"];
                    foreach (JToken castSection in credits)
                    {
                        if ((string)castSection["label"] == "Cast" ||
                            (string)castSection["label"] == "Series Cast")
                        {
                            JToken castList = castSection["list"];
                            foreach (JToken character in castList)
                            {
                                ImdbCharacter ic = new ImdbCharacter();
                                ic.CharacterName = (string)character["char"];
                                JToken actorName = character["name"];
                                ic.ImdbId = (string)actorName["nconst"];
                                ic.Name = (string)actorName["name"];
                                if (General.ContainsKey(actorName, "image"))
                                {
                                    JToken headshot = actorName["image"];
                                    ic.Headshot.URL = (string)headshot["url"];
                                    ic.Headshot.Width = (int)headshot["width"];
                                    ic.Headshot.Height = (int)headshot["height"];
                                }
                                ic.TitleAttribute = (string)character["attr"];
                                this.Title.Cast.Add(ic);
                            }
                        }
                    }
                }
            }
        }
    }
}
