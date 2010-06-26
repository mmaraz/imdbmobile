using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleTriviaParser
    {
        public TitleTriviaParser()
        {

        }

        public ImdbTitle ParseTitleTrivia(ImdbTitle title)
        {
            API a = new API();
            string Response = a.GetTitleTrivia(title.ImdbId);
            title.Trivia = new List<string>();

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "unspoilt"))
                {
                    JToken trivias = data["unspoilt"];
                    foreach (JToken trivia in trivias)
                    {
                        title.Trivia.Add((string)trivia["text"]);
                    }
                }
            }
            
            return title;
        }
    }
}
