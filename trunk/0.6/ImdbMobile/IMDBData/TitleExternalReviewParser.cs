using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleExternalReviewParser
    {
        public TitleExternalReviewParser()
        {

        }

        public List<ImdbExternalReview> ParseExternalReviews(ImdbTitle it)
        {
            API a = new API();
            string Response = a.GetExternalReviews(it.ImdbId);

            JObject Obj = JObject.Parse(Response);

            int Counter = 0;
            List<ImdbExternalReview> Reviews = new List<ImdbExternalReview>();

            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "reviews"))
                {
                    JToken reviews = data["reviews"];
                    foreach (JToken review in reviews)
                    {
                        Counter++;
                        ImdbExternalReview ier = new ImdbExternalReview();
                        ier.Author = (string)review["attr"];
                        ier.Label = (string)review["label"];
                        ier.URL = (string)review["url"];
                        Reviews.Add(ier);
                        // Limit this to 50 results.
                        // Would be nice to find a way to limit the amount of data the API
                        // brings back...
                        if (Counter == 50) { break; }
                    }
                }
            }

            return Reviews;
        }
    }
}