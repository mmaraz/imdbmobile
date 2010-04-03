using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ImdbMobile.UI
{
    class Translations
    {
        private static System.Xml.XmlDocument _xml;

        public static void ReadTranslationFile()
        {
            _xml = new System.Xml.XmlDocument();
            string Filename = IMDBData.SettingsWrapper.GlobalSettings.UILanguage;
            if (string.IsNullOrEmpty(Filename) || !System.IO.File.Exists(Filename))
            {
                Filename = IMDBData.SettingsWrapper.ApplicationPath + "\\Translations\\English.xml";
            }
            else
            {
                Filename = IMDBData.SettingsWrapper.ApplicationPath + "\\Translations\\" + IMDBData.SettingsWrapper.GlobalSettings.UILanguage + ".xml";
            }
            
            _xml.Load(Filename);
        }

        public static string GetTranslated(string ID)
        {
            System.Xml.XmlNode node = _xml.SelectSingleNode("/Translation/Phrases/Phrase[@ID='" + ID + "']");
            if (node != null)
            {
                return node.InnerText;
            }
            else
            {
                return "";
            }
        }
    }
}
