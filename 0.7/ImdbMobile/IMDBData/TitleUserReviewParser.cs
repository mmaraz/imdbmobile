using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleUserReviewParser : APIParser
    {
        public ImdbTitle Title;

        public TitleUserReviewParser()
        {

        }

        public void ParseUserReviews(ImdbTitle it)
        {
            this.Title = it;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetUserReviews(it.ImdbId);
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
                        if (General.ContainsKey(comment, "date"))
                        {
                            iur.Date = DateTime.Parse((string)comment["date"]);
                        }
                        else
                        {
                            iur.Date = DateTime.Now;
                        }
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

            this.Title.UserReviews = Reviews;
            this.OnParsingComplete();
        }
    }
}
