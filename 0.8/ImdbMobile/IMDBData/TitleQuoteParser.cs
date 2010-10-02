using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleQuoteParser : APIParser
    {
        public ImdbTitle Title;

        public TitleQuoteParser()
        {

        }

        public void ParseQuotes(ImdbTitle title)
        {
            this.Title = title;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetTitleQuotes(title.ImdbId);
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
            this.Title.Quotes = new List<ImdbQuoteSection>();

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
                        this.Title.Quotes.Add(iqs);
                    }
                }
            }

            this.OnParsingComplete();
        }
    }
}
