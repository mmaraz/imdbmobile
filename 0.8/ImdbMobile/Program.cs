using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ImdbMobile
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            ImdbMobile.IMDBData.SettingsWrapper.Load();
            UI.Translations.ReadTranslationFile();
            UI.WindowHandler.ControlList = new List<ImdbMobile.UI.SlidingList>();
            UI.WindowHandler.APIWorker = new ImdbMobile.IMDBData.API();
            Form1 f1 = new Form1();
            UI.WindowHandler.ParentForm = f1;
            Application.Run(f1);
        }
    }
}