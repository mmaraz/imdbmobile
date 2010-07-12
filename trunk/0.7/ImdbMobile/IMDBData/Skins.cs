using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ImdbMobile.IMDBData
{
    public class Skins
    {
        public string SkinName { get; set; }
        public int BgRed { get; set; }
        public int BgGreen { get; set; }
        public int BgBlue { get; set; }

        public Skins() { }
    }

    public class SkinsWrapper
    {
        public static Skins CurrentSkin { get; set; }

        public void Load() { }

        public void Save() { }
    }
}
