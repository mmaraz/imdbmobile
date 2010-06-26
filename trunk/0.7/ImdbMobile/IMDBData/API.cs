using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using OpenNETCF.Security.Cryptography;

namespace ImdbMobile.IMDBData
{
    public class API
    {
        string api = "v1";
        string appid = "iphone1";
        string locale = SettingsWrapper.GlobalSettings.Language.Locale;
        string sig = "app1";
        string apiKey = "2wex6aeu6a8q9e49k7sfvufd6rhh0n";

        public API() {}

        public string GetShowtimes(string Location, DateTime Date)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("location", Location);
            n.Add("date", Date.Year + "-" + FormatInt(Date.Month) + "-" + FormatInt(Date.Day));
            return GetResponse("showtimes/location", n);
        }

        private string FormatInt(int Integer)
        {
            if (Integer < 10)
            {
                return "0" + Integer;
            }
            return "" + Integer;
        }

        public string GetParentalGuide(string tconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("tconst", tconst);
            return GetResponse("title/parentalguide", n);
        }

        public string GetUserReviews(string tconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("tconst", tconst);
            return GetResponse("title/usercomments", n);
        }

        public string GetExternalReviews(string tconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("tconst", tconst);
            return GetResponse("title/external_reviews", n);
        }

        public string GetComingSoon()
        {
            return GetResponse("feature/comingsoon");
        }

        public string GetTop250()
        {
            return GetResponse("chart/top");
        }

        public string GetActorQuotes(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            return GetResponse("name/quotes", n);
        }

        public string GetActorTrivia(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            return GetResponse("name/trivia", n);
        }

        public string GetActorFilmography(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            return GetResponse("name/filmography", n);
        }

        public string GetActorDetails(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            return GetResponse("name/maindetails", n);
        }

        public string GetTitleEpisodes(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/episodes", t);
        }

        public string GetTitleGoofs(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/goofs", t);
        }

        public string GetTitleQuotes(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/quotes", t);
        }

        public string GetTitleTrivia(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/trivia", t);
        }

        public string GetTitlePhotos(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/photos", t);
        }

        public string GetFullDetails(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/maindetails", t);
        }

        public string GetFullCast(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            return GetResponse("title/fullcredits", t);
        }

        public string GetSearch(string Query)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("q", URLEncode(Query));
            return GetResponse("find", d);
        }

        public string GetResponse(string Function)
        {
            return GetResponse(Function, new Dictionary<string, string>());
        }

        public string GetResponse(string Function, Dictionary<string, string> Arguments)
        {
            try
            {
                string BaseURL = "http://app.imdb.com/" + Function + "?api=" + api + "&appid=" + appid + "&locale=" + locale + "&timestamp=" + GetTimestamp() + "&";
                if (Arguments.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in Arguments)
                    {
                        BaseURL += kvp.Key + "=" + kvp.Value + "&";
                    }
                }
                BaseURL = BaseURL.TrimEnd(new char[] { '&' });
                BaseURL = BaseURL.TrimEnd(new char[] { '&' });
                string RequestURL = BaseURL + "&sig=" + sig;
                WebRequest webReq = (WebRequest)HttpWebRequest.Create(RequestURL + "-" + EncodeURL(RequestURL));

                string Result = "";
                using (HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse())
                {
                    using (Stream s = resp.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            Result = sr.ReadToEnd();
                        }
                    }
                }

                return Result;
            }
            catch (ObjectDisposedException) { }
            return "";
        }

        private string GetTimestamp()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));

            return "" + (int)t.TotalSeconds;
        }

        private string EncodeURL(string URL)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(apiKey);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = encoding.GetBytes(URL);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
            return ByteToString(hashmessage);

        }

        private string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        private string URLEncode(string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in str)
            {
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);
                else
                    sb.AppendFormat("%{0:X02}", (int)c);
            }
            return sb.ToString();
        }
    }
}
