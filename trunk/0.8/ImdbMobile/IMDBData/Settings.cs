using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ImdbMobile.IMDBData
{
    [XmlRootAttribute(ElementName = "Settings", IsNullable = false)]
    public class Settings
    {
        public string VideoPlayerPath { get; set; }
        public string VideoPlayerArguments { get; set; }
        public ImdbLanguage Language { get; set; }
        public bool DownloadThumbnails { get; set; }
        public string CachePath { get; set; }
        public string UILanguage { get; set; }
        public bool UseAnimations { get; set; }
        public bool UseBigImages { get; set; }
        public int NumToDisplay { get; set; }
        public bool UseCompression { get; set; }
        public string CurrentSkinName { get; set; }
        public Skins CurrentSkin { get; set; }
        public List<string> RecentSearches { get; set; }

        public Settings()
        {
            
            this.RecentSearches = new List<string>();
        }
    }

    public class SettingsWrapper
    {
        public static Settings GlobalSettings;

        public static void Save(Settings s)
        {

            try
            {
                StreamWriter sw = new StreamWriter(ApplicationPath + "\\Config.xml");
                XmlSerializer xs = new XmlSerializer ( typeof ( Settings ) );
                XmlTextWriter xmlTextWriter = new XmlTextWriter(sw.BaseStream, Encoding.UTF8);
                xs.Serialize ( xmlTextWriter, s );
                sw.Close();

                GlobalSettings = s;
            }
            catch (Exception e)
            {

            }
        }

        public static string ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

        public static void Load()
        {
            try
            {
                if (!System.IO.File.Exists(ApplicationPath + "\\Config.xml"))
                {
                    Settings s = new Settings();
                    s.CachePath = ApplicationPath + "\\Cache";
                    s.DownloadThumbnails = true;
                    ImdbLanguages ilangs = new ImdbLanguages();
                    s.Language = ilangs.SupportedLanguages[0];
                    s.VideoPlayerArguments = "";
                    s.VideoPlayerPath = "\\Windows\\wmplayer.exe";
                    s.UseAnimations = false;
                    s.UseBigImages = false;
                    s.UILanguage = ApplicationPath + "//Translations//English.xml";
                    s.NumToDisplay = 500;
                    s.CurrentSkinName = "Default";

                    Save(s);
                }
                XmlSerializer xs = new XmlSerializer(typeof(Settings));
                StreamReader sr = new StreamReader(ApplicationPath + "\\Config.xml");
                GlobalSettings = (Settings)xs.Deserialize(sr.BaseStream);
                sr.Close();
                GlobalSettings.CurrentSkin = SkinsWrapper.Load(GlobalSettings.CurrentSkinName);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
