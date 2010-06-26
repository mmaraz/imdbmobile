using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    public class ShowtimeParser : APIParser
    {
        public List<ImdbCinema> Showtimes;

        public ShowtimeParser()
        {

        }

        public void ParseShowtimes(string Location, DateTime Date)
        {
            UI.WindowHandler.APIWorker = new API();
            UI.WindowHandler.APIWorker.DataDownloaded += new EventHandler(APIWorker_DataDownloaded);
            this.OnDownloadingData();
            UI.WindowHandler.APIWorker.GetShowtimes(Location, Date);
        }

        void APIWorker_DataDownloaded(object sender, EventArgs e)
        {
            APIEvent ae = (APIEvent)e;
            this.OnDownloadComplete(ae.EventData);

            this.OnParsingData();
            string Response = ae.EventData;

            List<ImdbCinema> sList = new List<ImdbCinema>();

            JObject Obj = JObject.Parse(Response);

            if (General.ContainsKey(Obj, "data"))
            {
                JToken data = Obj["data"];
                if (General.ContainsKey(data, "cinemas"))
                {
                    JToken cinemas = data["cinemas"];
                    foreach (JToken cinema in cinemas)
                    {
                        ImdbCinema ic = new ImdbCinema();
                        if (General.ContainsKey(cinema, "distance"))
                        {
                            ic.Distance = (string)cinema["distance"];
                        }

                        if (General.ContainsKey(cinema, "cinema"))
                        {
                            JToken cinemainfo = cinema["cinema"];
                            if (General.ContainsKey(cinemainfo, "address"))
                            {
                                ic.Address = (string)cinemainfo["address"];
                            }
                            if (General.ContainsKey(cinemainfo, "city"))
                            {
                                ic.City = (string)cinemainfo["city"];
                            }
                            if (General.ContainsKey(cinemainfo, "latitude"))
                            {
                                ic.Latitude = (double)cinemainfo["latitude"];
                            }
                            if (General.ContainsKey(cinemainfo, "longitude"))
                            {
                                ic.Longitude = (double)cinemainfo["longitude"];
                            }
                            if (General.ContainsKey(cinemainfo, "name"))
                            {
                                ic.Name = (string)cinemainfo["name"];
                            }
                            if (General.ContainsKey(cinemainfo, "postcode"))
                            {
                                ic.Postcode = (string)cinemainfo["postcode"];
                            }
                            if (General.ContainsKey(cinemainfo, "region"))
                            {
                                ic.Region = (string)cinemainfo["region"];
                            }
                            if (General.ContainsKey(cinemainfo, "state"))
                            {
                                ic.State = (string)cinemainfo["state"];
                            }
                            if (General.ContainsKey(cinemainfo, "phone"))
                            {
                                ic.Telephone = (string)cinemainfo["phone"];
                            }
                        }
                        if (General.ContainsKey(cinema, "movie_schedules"))
                        {
                            JToken schedules = cinema["movie_schedules"];
                            foreach (JToken filmSchedule in schedules)
                            {
                                ImdbCinemaSchedule ics = new ImdbCinemaSchedule();
                                ImdbTitle t = new ImdbTitle();
                                t.ImdbId = (string)filmSchedule["tconst"];
                                t.Title = (string)filmSchedule["vendor_title"];
                                ics.Movie = t;

                                foreach (JToken movietime in filmSchedule["sessions"])
                                {
                                    ics.Times.Add((string)movietime["local"]);
                                }

                                ic.Showtimes.Add(ics);
                            }
                        }
                    }
                }
            }

            this.Showtimes = sList;
            this.OnParsingComplete();
        }
    }
}
