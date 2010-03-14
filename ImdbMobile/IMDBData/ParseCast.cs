using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class ParseCast
    {
        private ImdbTitle CurrentTitle;

        public ParseCast()
        {

        }

        public ImdbTitle ParseFullCast(ImdbTitle title)
        {
            this.CurrentTitle = title;
            API a = new API();
            ParseFullCast(a.GetFullCast(title.ImdbId));
            return this.CurrentTitle;
        }

        private void ParseFullCast(string Response)
        {
            this.CurrentTitle.Cast = new List<ImdbCharacter>();
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
                                this.CurrentTitle.Cast.Add(ic);
                            }
                        }
                    }
                }
            }
        }
    }
}
