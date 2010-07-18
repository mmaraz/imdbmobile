using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ImdbMobile.IMDBData
{
    public class Skins
    {
        public string SkinName { get; set; }
        public int BackgroundR {get;set;}
        public int BackgroundG { get; set; }
        public int BackgroundB { get; set; }
        public System.Drawing.Rectangle ListBounds { get; set; }
        public int TitlebarGradientStartR { get; set; }
        public int TitlebarGradientStartG { get; set; }
        public int TitlebarGradientStartB { get; set; }
        public int TitlebarGradientStopR { get; set; }
        public int TitlebarGradientStopG { get; set; }
        public int TitlebarGradientStopB { get; set; }
        public int ActionHoverFromR { get; set; }
        public int ActionHoverFromG { get; set; }
        public int ActionHoverFromB { get; set; }
        public int ActionHoverToR { get; set; }
        public int ActionHoverToG { get; set; }
        public int ActionHoverToB { get; set; }
        public int ActionTextHoverR { get; set; }
        public int ActionTextHoverG { get; set; }
        public int ActionTextHoverB { get; set; }
        public string FontName { get; set; }
        public float FontSize_Large { get; set; }
        public float FontSize_Small { get; set; }
        public System.Drawing.FontStyle FontStyle_Large { get; set; }
        public System.Drawing.FontStyle FontStyle_Small { get; set; }

        public System.Drawing.Color ActionHoverFrom
        {
            get
            {
                return System.Drawing.Color.FromArgb(ActionHoverFromR, ActionHoverFromG, ActionHoverFromB);
            }
        }

        public System.Drawing.Color ActionHoverTo
        {
            get
            {
                return System.Drawing.Color.FromArgb(ActionHoverToR, ActionHoverToG, ActionHoverToB);
            }
        }

        public System.Drawing.Color ActionTextHover
        {
            get
            {
                return System.Drawing.Color.FromArgb(ActionTextHoverR, ActionTextHoverG, ActionTextHoverB);
            }
        }

        public System.Drawing.Color BackgroundColour
        {
            get
            {
                return System.Drawing.Color.FromArgb(BackgroundR, BackgroundG, BackgroundB);
            }
        }

        public System.Drawing.Color TitlebarGradientStart
        {
            get
            {
                return System.Drawing.Color.FromArgb(TitlebarGradientStartR, TitlebarGradientStartG, TitlebarGradientStartB);
            }
        }

        public System.Drawing.Color TitlebarGradientStop
        {
            get
            {
                return System.Drawing.Color.FromArgb(TitlebarGradientStopR, TitlebarGradientStopG, TitlebarGradientStopB);
            }
        }

        public Skins() { }
    }

    public class SkinsWrapper
    {
        public static Skins CurrentSkin { get; set; }

        public static Skins Load(string SkinName)
        {
            try
            {
                if (!System.IO.File.Exists(SettingsWrapper.ApplicationPath + "\\Skins\\" + SkinName + "\\config.xml"))
                {
                    Skins s = new Skins();
                    s.BackgroundR = System.Drawing.Color.LightGoldenrodYellow.R;
                    s.BackgroundG = System.Drawing.Color.LightGoldenrodYellow.G;
                    s.BackgroundB = System.Drawing.Color.LightGoldenrodYellow.B;
                    s.SkinName = SkinName;
                    System.Drawing.Color start = System.Drawing.Color.FromArgb(52, 60, 67); 
                    s.TitlebarGradientStartR = start.R;
                    s.TitlebarGradientStartG = start.G;
                    s.TitlebarGradientStartB = start.B;
                    System.Drawing.Color stop = System.Drawing.Color.FromArgb(27, 31, 29);
                    s.TitlebarGradientStopR = stop.R;
                    s.TitlebarGradientStopG = stop.G;
                    s.TitlebarGradientStopB = stop.B;
                    s.ActionHoverFromR = start.R;
                    s.ActionHoverFromG = start.G;
                    s.ActionHoverFromB = start.B;
                    s.ActionHoverToR = start.R;
                    s.ActionHoverToG = start.G;
                    s.ActionHoverToB = start.B;
                    s.ActionTextHoverR = System.Drawing.Color.LightGoldenrodYellow.R;
                    s.ActionTextHoverG = System.Drawing.Color.LightGoldenrodYellow.G;
                    s.ActionTextHoverB = System.Drawing.Color.LightGoldenrodYellow.B;
                    s.ListBounds = new System.Drawing.Rectangle();
                    s.FontName = "Tahoma";
                    s.FontSize_Large = 12f;
                    s.FontSize_Small = 10f;
                    s.FontStyle_Small = System.Drawing.FontStyle.Regular;
                    s.FontStyle_Large = System.Drawing.FontStyle.Regular;

                    Save(s);
                    CurrentSkin = s;
                }
                XmlSerializer xs = new XmlSerializer(typeof(Skins));
                StreamReader sr = new StreamReader(SettingsWrapper.ApplicationPath + "\\Skins\\" + SkinName + "\\config.xml");
                CurrentSkin = (Skins)xs.Deserialize(sr.BaseStream);
                sr.Close();

                return CurrentSkin;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void Save(Skins s)
        {
            try
            {
                StreamWriter sw = new StreamWriter(SettingsWrapper.ApplicationPath + "\\Skins\\" + s.SkinName + "\\config.xml");
                XmlSerializer xs = new XmlSerializer(typeof(Skins));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(sw.BaseStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, s);
                sw.Close();

                CurrentSkin = s;
            }
            catch (Exception e)
            {

            }
        }
    }
}
