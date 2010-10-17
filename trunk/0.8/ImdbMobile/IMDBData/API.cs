using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using OpenNETCF.Security.Cryptography;
using System.IO.Compression;

namespace ImdbMobile.IMDBData
{
    public class API
    {
        public delegate void EventCallback();

        public event EventHandler ResponseReceived;
        public event EventHandler DataDownloaded;
        public event EventHandler Error;

        private RequestState CurrentState;

        string api = "v1";
        string appid = "iphone1";
        string locale = SettingsWrapper.GlobalSettings.Language.Locale;
        string sig = "app1";
        string apiKey = "2wex6aeu6a8q9e49k7sfvufd6rhh0n";

        class RequestState
        {
            const int BufferSize = 1024;
            public StringBuilder RequestData;
            public byte[] BufferRead;
            public WebRequest Request;
            public Stream ResponseStream;
            public GZipStream CompressedStream;

            public RequestState()
            {
                BufferRead = new byte[BufferSize];
                RequestData = new StringBuilder(String.Empty);
                Request = null;
                ResponseStream = null;
            }
        }

        public API()
        {
            
        }

        public void GetShowtimes(string Location, DateTime Date)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("location", Location);
            n.Add("date", Date.Year + "-" + FormatInt(Date.Month) + "-" + FormatInt(Date.Day));
            GetResponse("showtimes/location", n);
        }

        private string FormatInt(int Integer)
        {
            if (Integer < 10)
            {
                return "0" + Integer;
            }
            return "" + Integer;
        }

        public void GetParentalGuide(string tconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("tconst", tconst);
            GetResponse("title/parentalguide", n);
        }

        public void GetUserReviews(string tconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("tconst", tconst);
            GetResponse("title/usercomments", n);
        }

        public void GetExternalReviews(string tconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("tconst", tconst);
            GetResponse("title/external_reviews", n);
        }

        public void GetComingSoon()
        {
            GetResponse("feature/comingsoon");
        }

        public void GetTop250()
        {
            GetResponse("chart/top");
        }

        public void GetBottom100()
        {
            GetResponse("chart/bottom");
        }

        public void GetActorQuotes(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            GetResponse("name/quotes", n);
        }

        public void GetActorTrivia(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            GetResponse("name/trivia", n);
        }

        public void GetActorFilmography(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            GetResponse("name/filmography", n);
        }

        public void GetActorDetails(string nconst)
        {
            Dictionary<string, string> n = new Dictionary<string, string>();
            n.Add("nconst", nconst);
            GetResponse("name/maindetails", n);
        }

        public void GetTitleEpisodes(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/episodes", t);
        }

        public void GetTitleGoofs(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/goofs", t);
        }

        public void GetTitleQuotes(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/quotes", t);
        }

        public void GetTitleTrivia(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/trivia", t);
        }

        public void GetTitlePhotos(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/photos", t);
        }

        public void GetFullDetails(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/maindetails", t);
        }

        public void GetFullCast(string tconst)
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t.Add("tconst", tconst);
            GetResponse("title/fullcredits", t);
        }

        public void GetSearch(string Query)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("q", URLEncode(Query));
            GetResponse("find", d);
        }

        public void GetResponse(string Function)
        {
            GetResponse(Function, new Dictionary<string, string>());
        }

        public void GetResponse(string Function, Dictionary<string, string> Arguments)
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

                System.Net.HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(BaseURL);
                if (SettingsWrapper.GlobalSettings.UseCompression)
                {
                    hwr.Headers.Add("Accept-Encoding: gzip,deflate");
                }
                this.CurrentState = new RequestState();
                this.CurrentState.Request = hwr;

                hwr.BeginGetResponse(new AsyncCallback(OnHTTPOK), this.CurrentState);
            }
            catch (Exception ex)
            {
                if (this.Error != null)
                {
                    UI.WindowHandler.ParentForm.Invoke(
                        new EventCallback(
                            delegate()
                            {
                                APIEvent ae = new APIEvent(ex.Message);
                                this.Error(null, ae);
                            }
                        )
                        );
                }
            }
        }

        public void Abort()
        {
            if (this.CurrentState != null)
            {
                if (this.CurrentState.CompressedStream != null)
                {
                    this.CurrentState.CompressedStream.Dispose();
                }
                if (this.CurrentState.Request != null)
                {
                    this.CurrentState.Request.Abort();
                }
                if (this.CurrentState.ResponseStream != null)
                {
                    this.CurrentState.ResponseStream.Dispose();
                }
            }
        }

        private void OnHTTPOK(IAsyncResult result)
        {
            try
            {
                RequestState rs = (RequestState)result.AsyncState;
                WebResponse resp = rs.Request.EndGetResponse(result);
                rs.ResponseStream = resp.GetResponseStream();
                if (!resp.Headers.ToString().Contains("\r\nContent-Encoding: gzip\r\n"))
                {
                    // Server hasn't returned a GZip compressed page,
                    // switch off compression
                    IMDBData.SettingsWrapper.GlobalSettings.UseCompression = false;
                }

                if (this.ResponseReceived != null)
                {
                    UI.WindowHandler.ParentForm.Invoke(
                            new EventCallback(
                                delegate()
                                {
                                    this.ResponseReceived(this, null);
                                }
                            )
                            );
                }

                if (IMDBData.SettingsWrapper.GlobalSettings.UseCompression)
                {
                    GZipStream gs = new GZipStream(rs.ResponseStream, CompressionMode.Decompress);
                    rs.CompressedStream = gs;
                    gs.BeginRead(rs.BufferRead, 0, 1024, OnStreamOK, rs);
                }
                else
                {
                    rs.ResponseStream.BeginRead(rs.BufferRead, 0, 1024, OnStreamOK, rs);
                }
            }
            catch (ObjectDisposedException)
            {
                // User aborted operation
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.RequestCanceled)
                {
                    // User aborted. Dodgy MS spelling :)
                }
                else
                {
                    if (this.Error != null)
                    {
                        UI.WindowHandler.ParentForm.Invoke(
                                new EventCallback(
                                    delegate()
                                    {
                                        APIEvent ae = new APIEvent(ex.Message);
                                        this.Error(this, ae);
                                    }
                                )
                                );
                    }
                }
            }
        }

        private void OnStreamOK(IAsyncResult result)
        {
            try
            {
                RequestState rs = (RequestState)result.AsyncState;

                int Read = -1;
                if (SettingsWrapper.GlobalSettings.UseCompression)
                {
                    Read = rs.CompressedStream.EndRead(result);
                }
                else
                {
                    Read = rs.ResponseStream.EndRead(result);
                }
                if (Read > 0)
                {
                    string Text = System.Text.Encoding.UTF8.GetString(rs.BufferRead, 0, Read);
                    rs.RequestData.Append(Text);

                    if (SettingsWrapper.GlobalSettings.UseCompression)
                    {
                        rs.CompressedStream.BeginRead(rs.BufferRead, 0, 1024, OnStreamOK, rs);
                    }
                    else
                    {
                        rs.ResponseStream.BeginRead(rs.BufferRead, 0, 1024, OnStreamOK, rs);
                    }
                }
                else
                {
                    if (rs.CompressedStream != null)
                    {
                        rs.CompressedStream.Close();
                    }

                    rs.ResponseStream.Close();

                    if (this.DataDownloaded != null)
                    {
                        UI.WindowHandler.ParentForm.Invoke(
                            new EventCallback(
                                delegate()
                                {
                                    APIEvent ae = new APIEvent(rs.RequestData.ToString());
                                    this.DataDownloaded(this, ae);
                                }
                            )
                            );
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // User aborted operation
            }
            catch (OutOfMemoryException)
            {

            }
            catch (InvalidOperationException)
            {
                // User aborted operation
            }
        }

        private string GetTimestamp()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));

            return "" + (int)t.TotalSeconds;
        }

        private string EncodeURL(string URL)
        {
            byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(apiKey);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(URL);
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
