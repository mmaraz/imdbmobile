using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleUserReviewParser
    {
        public TitleUserReviewParser()
        {

        }

        public List<ImdbUserReview> ParseUserReviews(ImdbTitle it)
        {
            API a = new API();
            string Response = a.GetUserReviews(it.ImdbId);

            JObject Obj = JObject.Parse(Response);

            List<ImdbUserReview> Reviews = new List<ImdbUserReview>();

            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "user_comments"))
                {
                    JToken comments = data["user_comments"];
                    foreach (JToken comment in comments)
                    {
                        ImdbUserReview iur = new ImdbUserReview();
                        iur.Date = DateTime.Parse((string)comment["date"]);
                        iur.FullText = (string)comment["text"];
                        iur.Status = (string)comment["status"];
                        iur.Summary = (string)comment["summary"];
                        iur.UserLocation = (string)comment["user_location"];
                        iur.Username = (string)comment["user_name"];
                        if (General.ContainsKey(comment, "user_rating"))
                        {
                            iur.UserRating = (int)comment["user_rating"] * 1.0;
                        }
                        else
                        {
                            iur.UserRating = 0.0;
                        }
                        Reviews.Add(iur);
                    }
                }
            }

            return Reviews;
        }
    }
}
