using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class ActorTriviaParser
    {
        public ActorTriviaParser()
        {

        }

        public ImdbActor ParseTitleTrivia(ImdbActor actor)
        {
            API a = new API();
            actor.Trivia.Clear();
            string Response = a.GetActorTrivia(actor.ImdbId);

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "unspoilt"))
                {
                    JToken trivias = data["unspoilt"];
                    foreach (JToken trivia in trivias)
                    {
                        actor.Trivia.Add((string)trivia["text"]);
                    }
                }
                else if(General.ContainsKey(data, "trivia"))
                {
                    JToken trivias = data["trivia"];
                    foreach (JToken trivia in trivias)
                    {
                        actor.Trivia.Add((string)trivia["text"]);
                    }
                }
            }

            return actor;
        }
    }
}
