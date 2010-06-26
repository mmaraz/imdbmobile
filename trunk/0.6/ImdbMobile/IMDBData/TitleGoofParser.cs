using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleGoofParser
    {
        public TitleGoofParser() { }

        public ImdbTitle ParseGoofs(ImdbTitle title)
        {
            API a = new API();
            string Response = a.GetTitleGoofs(title.ImdbId);
            title.Goofs = new List<ImdbGoof>();

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
                        title.Goofs.Add(g);
                    }
                }
            }
            return title;
        }
    }
}
