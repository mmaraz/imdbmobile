using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ImdbMobile.IMDBData
{
    class General
    {
        public static bool ContainsKey(JToken Data, string Key)
        {
            JToken jt = Data[Key];
            if (jt == null)
            {
                return false;
            }
            return true;
        }
    }
    public class ImdbSearchResult
    {
        public enum ResultTypeList
        {
            Actor,
            Title
        }

        public ResultTypeList ResultType
        {
            get
            {
                if (this.GetType() == typeof(ImdbActor))
                {
                    return ResultTypeList.Actor;
                }
                else
                {
                    return ResultTypeList.Title;
                }
            }
        }

        public ImdbSearchResult() { }
    }

    public class ImdbCinema
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Telephone { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string Distance { get; set; }
        public List<ImdbCinemaSchedule> Showtimes { get; set; }

        public ImdbCinema() { }
    }

    public class ImdbCinemaSchedule
    {
        public ImdbTitle Movie { get; set; }
        public List<string> Times { get; set; }

        public ImdbCinemaSchedule()
        {
            this.Times = new List<string>();
            this.Movie = new ImdbTitle();
        }
    }

    public class ImdbExternalReview
    {
        public string URL { get; set; }
        public string Label { get; set; }
        public string Author { get; set; }

        public ImdbExternalReview() { }
    }

    public class ImdbUserReview
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }
        public string FullText { get; set; }
        public string UserLocation { get; set; }
        public string Username { get; set; }
        public double UserRating { get; set; }

        public ImdbUserReview() { }
    }

    public class ImdbTitle : ImdbSearchResult
    {
        public enum TitleType
        {
            TVSeries,
            FeatureMovie,
            VideoGame,
            TVEpisode
        }

        public bool HasFullCast { get; set; }

        public string Title { get; set; }
        public string ImdbId { get; set; }
        public TitleType Type { get; set; }
        public ImdbCover Cover { get; set; }
        public string Year { get; set; }
        public List<ImdbActor> Actors { get; set; }

        // Full Details
        public List<ImdbActor> Directors { get; set; }
        public List<ImdbWriter> Writers { get; set; }
        public string Rating { get; set; }
        public int NumberOfVotes { get; set; }
        public List<string> Genres { get; set; }
        public string ReleaseDate { get; set; }
        public string Tagline { get; set; }
        public string Runtime { get; set; }
        public List<ImdbCharacter> Cast { get; set; }
        public string Plot { get; set; }
        public string Certificate { get; set; }

        // Extra Details
        public List<string> Trivia { get; set; }
        public List<ImdbQuoteSection> Quotes { get; set; }
        public List<ImdbGoof> Goofs { get; set; }
        public List<ImdbPhoto> Photos { get; set; }
        public ImdbVideo Trailer { get; set; }
        public List<ImdbSeason> Seasons { get; set; }
        public Dictionary<string, string> ParentalGuide { get; set; }
        public List<ImdbExternalReview> ExternalReviews { get; set; }
        public List<ImdbUserReview> UserReviews { get; set; }

        public ImdbTitle()
        {
            this.Actors = new List<ImdbActor>();
            this.Cover = new ImdbCover();
            this.Cast = new List<ImdbCharacter>();
            this.Genres = new List<string>();
            this.Photos = new List<ImdbPhoto>();
            this.Directors = new List<ImdbActor>();
            this.Writers = new List<ImdbWriter>();
            this.Trivia = new List<string>();
            this.Quotes = new List<ImdbQuoteSection>();
            this.Goofs = new List<ImdbGoof>();

        }
    }

    public class ImdbSeason
    {
        public string ShowTitle { get; set; }
        public string Label { get; set; }
        public List<ImdbEpisode> Episodes { get; set; }

        public ImdbSeason()
        {
            this.Episodes = new List<ImdbEpisode>();
        }
    }

    public class ImdbEpisode
    {
        public string ImdbId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }

        public ImdbEpisode() { }
    }

    public class ImdbEncoding
    {
        public enum VideoType
        {
            ThreeG,
            EDGE,
            HD480p,
            HD720p
        }

        public string VideoURL { get; set; }

        public VideoType Type { get; set; }

        public ImdbEncoding() { }
    }

    public class ImdbVideo
    {
        public List<ImdbEncoding> Encodings { get; set; }
        public List<ImdbCover> Slates { get; set; }

        public ImdbVideo()
        {
            this.Encodings = new List<ImdbEncoding>();
            this.Slates = new List<ImdbCover>();
        }
    }

    public class ImdbPhoto
    {
        public string Caption { get; set; }
        public ImdbCover Image { get; set; }

        public ImdbPhoto()
        {
            this.Image = new ImdbCover();
        }
    }

    public class ImdbGoof
    {
        public string Description { get; set; }
        public GoofType Type { get; set; }

        public enum GoofType
        {
            Continuity, // GOOF-CONT
            RevealingMistakes, // GOOF-FAKE
            CrewOrEquipment, // GOOF-CREW
            IncorrectlyRegarded, // GOOF-FAIR
            PlotHoles, // GOOF-PLOT
            FactualErrors // GOOF-FACT
        }
    }

    public class ImdbQuoteSection
    {
        public List<ImdbQuote> Quotes { get; set; }

        public ImdbQuoteSection()
        {
            this.Quotes = new List<ImdbQuote>();
        }
    }

    public class ImdbQuote
    {
        public ImdbCharacter Character { get; set; }
        public string Quote { get; set; }

        public ImdbQuote()
        {
            this.Character = new ImdbCharacter();
        }
    }

    public class ImdbWriter : ImdbActor
    {
        public string TitleAttribute { get; set; }

        public ImdbWriter() { }
    }

    public class ImdbKnownForGroup
    {
        public string Label { get; set; }
        public List<ImdbKnownFor> KnownForList { get; set; }

        public ImdbKnownForGroup()
        {
            this.KnownForList = new List<ImdbKnownFor>();
        }
    }

    public class ImdbKnownFor : ImdbTitle
    {
        public string TitleAttribute { get; set; }
        public string CharacterName { get; set; }

        public ImdbKnownFor() { }
    }

    public class ImdbCharacter : ImdbActor
    {
        public string CharacterName { get; set; }
        public string TitleAttribute { get; set; }

        public ImdbCharacter() { }
    }

    public class ImdbCover
    {
        public string URL { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImdbCover() { }
    }

    public class ImdbActor : ImdbSearchResult
    {
        public string ImdbId { get; set; }
        public string Name { get; set; }
        public ImdbCover Headshot { get; set; }
        public string KnownFor { get; set; }

        // Full Details
        public List<ImdbCover> Photos { get; set; }
        public string Birthday { get; set; }
        public string RealName { get; set; }
        public string Bio { get; set; }
        public List<ImdbKnownForGroup> KnownForFull { get; set; }

        // Extra Details
        public List<string> Trivia { get; set; }

        public ImdbActor()
        {
            this.Headshot = new ImdbCover();
            this.Photos = new List<ImdbCover>();
            this.KnownForFull = new List<ImdbKnownForGroup>();
            this.Trivia = new List<string>();
        }
    }

    public class ImdbLanguage
    {
        public string Locale { get; set; }
        public string Country { get; set; }

        public ImdbLanguage() { }
    }

    public class ImdbLanguages
    {
        public List<ImdbLanguage> SupportedLanguages
        {
            get
            {
                List<ImdbLanguage> langList = new List<ImdbLanguage>();

                ImdbLanguage en = new ImdbLanguage();
                en.Locale = "en_US";
                en.Country = "English (US)";
                langList.Add(en);

                ImdbLanguage de = new ImdbLanguage();
                de.Locale = "de_DE";
                de.Country = "Germany (DE)";
                langList.Add(de);

                ImdbLanguage it = new ImdbLanguage();
                it.Locale = "it_IT";
                it.Country = "Italy (IT)";
                langList.Add(it);

                ImdbLanguage fr = new ImdbLanguage();
                fr.Locale = "fr_FR";
                fr.Country = "France (FR)";
                langList.Add(fr);

                ImdbLanguage es = new ImdbLanguage();
                es.Locale = "es_ES";
                es.Country = "Spain (ES)";
                langList.Add(es);

                return langList;
            }
        }
    }
}
