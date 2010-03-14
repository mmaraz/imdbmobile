using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleQuoteParser
    {

        public TitleQuoteParser()
        {

        }

        public ImdbTitle ParseQuotes(ImdbTitle title)
        {
            API a = new API();
            string Response = a.GetTitleQuotes(title.ImdbId);
            title.Quotes = new List<ImdbQuoteSection>();

            JObject Obj = JObject.Parse(Response);
            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "quotes"))
                {
                    JToken quotes = data["quotes"];
                    foreach (JToken quoteSection in quotes)
                    {
                        ImdbQuoteSection iqs = new ImdbQuoteSection();
                        foreach (JToken singleQuote in quoteSection["lines"])
                        {
                            ImdbQuote iq = new ImdbQuote();
                            if (General.ContainsKey(singleQuote, "chars"))
                            {
                                JToken chars = singleQuote["chars"];
                                foreach (JToken singleChar in chars)
                                {
                                    iq.Character.CharacterName = (string)singleChar["char"];
                                    iq.Character.ImdbId = (string)singleChar["nconst"];
                                    break;
                                }
                                iq.Quote = (string)singleQuote["quote"];
                            }
                            else
                            {
                                iq.Quote = (string)singleQuote["stage"];
                            }
                            iqs.Quotes.Add(iq);
                            
                        }
                        title.Quotes.Add(iqs);
                    }
                }
            }
            return title;
        }
    }
}
