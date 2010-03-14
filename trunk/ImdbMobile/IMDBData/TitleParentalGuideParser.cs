using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleParentalGuideParser
    {
        public TitleParentalGuideParser()
        {

        }

        public Dictionary<string, string> ParseParentalGuide(ImdbTitle it)
        {
            API a = new API();
            string Response = a.GetParentalGuide(it.ImdbId);

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
            return Guides;
        }
    }
}
