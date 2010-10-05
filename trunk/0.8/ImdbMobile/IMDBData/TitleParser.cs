using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class TitleParser : APIParser
    {
        public ImdbTitle Title;

        public TitleParser()
        {

        }

        public void ParseFullDetails(ImdbTitle OriginalTitle)
        {
            this.Title = OriginalTitle;
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.Error += new EventHandler(APIWorker_Error);
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetFullDetails(OriginalTitle.ImdbId);
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
            this.Title = ParseData(ae.EventData);
            this.OnParsingComplete();
        }

        private ImdbTitle ParseData(string Response)
        {
            ImdbTitle title = this.Title;
            JObject jd = JObject.Parse(Response);
            

            if (General.ContainsKey(jd, "data"))
            {
                JToken Obj = jd["data"];
                title.Certificate = ParseCertificate(Obj);
                title.Directors = ParseDirectors(Obj);
                title.Writers = ParseWriters(Obj);
                title.Rating = ParseRating(Obj);
                title.NumberOfVotes = ParseNumberOfVotes(Obj);
                title.Genres = ParseGenres(Obj);
                title.ReleaseDate = ParseReleaseDate(Obj);
                title.Tagline = ParseTagline(Obj);
                title.Runtime = ParseRuntime(Obj);
                title.Cast = ParseCast(Obj);
                title.Plot = ParsePlot(Obj);
                title.Trailer = ParseTrailers(Obj);
                title.Cover = ParseImage(Obj);
            }

            return title;
        }

        private string ParseCertificate(JToken data)
        {
            if (General.ContainsKey(data, "certificate"))
            {
                JToken cert = data["certificate"];
                if (General.ContainsKey(cert, "certificate"))
                {
                    return (string)cert["certificate"];
                }
            }
            return null;
        }

        private ImdbVideo ParseTrailers(JToken data)
        {
            if (General.ContainsKey(data, "trailer"))
            {
                JToken trailers = data["trailer"];
                ImdbVideo iv = new ImdbVideo();
                if (General.ContainsKey(trailers, "encodings"))
                {
                    JToken encodings = trailers["encodings"];
                    foreach (JToken encoding in encodings)
                    {
                        foreach (JToken singleEncoding in encoding)
                        {
                            ImdbEncoding ie = new ImdbEncoding();
                            ie.VideoURL = (string)singleEncoding["url"];
                            switch ((string)singleEncoding["format"])
                            {
                                case "iPhone 3G": ie.Type = ImdbEncoding.VideoType.ThreeG; break;
                                case "iPhone EDGE": ie.Type = ImdbEncoding.VideoType.EDGE; break;
                                case "HD 480p": ie.Type = ImdbEncoding.VideoType.HD480p; break;
                                case "HD 720p": ie.Type = ImdbEncoding.VideoType.HD720p; break;
                            }
                            iv.Encodings.Add(ie);
                        }
                    }
                }
                if (General.ContainsKey(trailers, "slates"))
                {
                    JToken slates = trailers["slates"];
                    foreach (JToken slate in slates)
                    {
                        ImdbCover ic = new ImdbCover();
                        ic.URL = (string)slate["url"];
                        ic.Width = int.Parse(slate["width"].ToString());
                        ic.Height = int.Parse(slate["height"].ToString());
                        iv.Slates.Add(ic);
                    }
                }

                return iv;
            }
            return null;
        }

        private string ParsePlot(JToken data)
        {
            if (General.ContainsKey(data, "plot"))
            {
                JToken plot = data["plot"];
                if (General.ContainsKey(plot, "outline"))
                {
                    return (string)plot["outline"];
                }
            }
            return null;
        }

        private List<ImdbCharacter> ParseCast(JToken data)
        {
            List<ImdbCharacter> retList = new List<ImdbCharacter>();
            if (General.ContainsKey(data, "cast_summary"))
            {
                JToken cast = data["cast_summary"];
                foreach (JToken actor in cast)
                {
                    ImdbCharacter ic = new ImdbCharacter();
                    if (General.ContainsKey(actor, "char"))
                    {
                        ic.CharacterName = (string)actor["char"];
                    }
                    if (General.ContainsKey(actor, "name"))
                    {
                        JToken name = actor["name"];
                        ic.ImdbId = (string)name["nconst"];
                        ic.Name = (string)name["name"];
                        if (General.ContainsKey(name, "image"))
                        {
                            ic.Headshot.URL = (string)name["image"]["url"];
                            ic.Headshot.Height = (int)name["image"]["height"];
                            ic.Headshot.Width = (int)name["image"]["width"];
                        }
                    }
                    retList.Add(ic);
                }
            }
            return retList;
        }

        private string ParseRuntime(JToken data)
        {
            if (General.ContainsKey(data, "runtime"))
            {
                JToken runtime = data["runtime"];
                if (General.ContainsKey(runtime, "time"))
                {
                    return "" + (int)runtime["time"];
                }
            }
            return null;
        }

        private string ParseTagline(JToken data)
        {
            if (General.ContainsKey(data, "tagline"))
            {
                return (string)data["tagline"];
            }
            return null;
        }

        private ImdbCover ParseImage(JToken data)
        {
            ImdbCover cover = new ImdbCover();
            if (General.ContainsKey(data, "image"))
            {
                cover.URL = (string)data["image"]["url"];
                cover.Width = (int)data["image"]["width"];
                cover.Height = (int)data["image"]["height"];
            }
            return cover;
        }

        private string ParseReleaseDate(JToken data)
        {
            if (General.ContainsKey(data, "release_date"))
            {
                JToken release = data["release_date"];
                if (General.ContainsKey(release, "normal"))
                {
                    return (string)release["normal"];
                }
            }
            return null;
        }

        private List<string> ParseGenres(JToken data)
        {
            List<string> retList = new List<string>();
            if (General.ContainsKey(data, "genres"))
            {
                JToken genres = data["genres"];
                foreach (JToken g in genres)
                {
                    retList.Add((string)g);
                }
            }
            return retList;
        }

        private int ParseNumberOfVotes(JToken data)
        {
            if (General.ContainsKey(data, "num_votes"))
            {
                return (int)data["num_votes"];
            }
            return -1;
        }

        private string ParseRating(JToken data)
        {
            if (General.ContainsKey(data, "rating"))
            {
                JValue ratingValue = (JValue)data["rating"];

                if (ratingValue.Value.GetType() == typeof(Int32))
                {
                    return "" + (Int32)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(Int64))
                {
                    return "" + (Int64)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(string))
                {
                    return "" + (string)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(decimal))
                {
                    return "" + (decimal)ratingValue.Value;
                }
                else if (ratingValue.Value.GetType() == typeof(double))
                {
                    return "" + (double)ratingValue.Value;
                }

                return null;
            }
            return null;
        }

        private List<ImdbWriter> ParseWriters(JToken data)
        {
            List<ImdbWriter> retList = new List<ImdbWriter>();
            if (General.ContainsKey(data, "writers_summary"))
            {
                JToken writers = data["writers_summary"];
                foreach (JToken writer in writers)
                {
                    ImdbWriter iw = new ImdbWriter();
                    if (General.ContainsKey(writer, "name"))
                    {
                        JToken name = writer["name"];
                        iw.Name = (string)name["name"];
                        iw.ImdbId = (string)name["nconst"];
                    }
                    if (General.ContainsKey(writer, "attr"))
                    {
                        iw.TitleAttribute = (string)writer["attr"];
                    }
                    retList.Add(iw);
                }
            }
            return retList;
        }

        private List<ImdbActor> ParseDirectors(JToken data)
        {
            List<ImdbActor> retList = new List<ImdbActor>();
            if (General.ContainsKey(data, "directors_summary"))
            {
                JToken Obj = data["directors_summary"];
                foreach (JToken jsd in Obj)
                {
                    if (General.ContainsKey(jsd, "name"))
                    {
                        JToken dirname = jsd["name"];
                        ImdbActor ia = new ImdbActor();
                        ia.ImdbId = (string)dirname["nconst"];
                        ia.Name = (string)dirname["name"];
                        retList.Add(ia);
                    }
                }
            }
            return retList;
        }
    }
}
