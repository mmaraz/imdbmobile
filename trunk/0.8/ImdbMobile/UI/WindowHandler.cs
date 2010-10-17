using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ImdbMobile.UI
{
    class WindowHandler
    {
        public static List<SlidingList> ControlList;
        public static Form1 ParentForm;
        public static IMDBData.API APIWorker;

        public static void OpenForm(SlidingList sl)
        {
            if (ControlList.Count > 0)
            {
                SlidingList OutControl = ControlList[ControlList.Count - 1];
                OutControl.SlideOutRight();
            }
            sl.ParentForm = ParentForm;
            sl.Initialise();
            ControlList.Add(sl);
        }

        public static void Back()
        {
            // Kill any current web/read requests
            APIWorker.Abort();
            if (ControlList.Count > 1)
            {
                SlidingList sl = ControlList[ControlList.Count - 1];
                ControlList.Remove(sl);
                sl.Close();
                if (ControlList.Count > 0)
                {
                    SlidingList NewFocus = ControlList[ControlList.Count - 1];
                    NewFocus.SlideInLeft();
                }
            }
            else
            {
                ImdbMobile.Controls.MainControl mc = (ImdbMobile.Controls.MainControl)ControlList[0];
                mc.ShowMainMenu();
            }
        }

        public static void Home()
        {
            // Kill any current web/read requests
            APIWorker.Abort();
            if (ControlList.Count > 1)
            {
                for (int i = 1; i < ControlList.Count; i++)
                {
                    SlidingList sl = ControlList[i];
                    ControlList.Remove(sl);
                    sl.Close();
                }
                ControlList[0].SlideInLeft();
               
            }
            else
            {
                ImdbMobile.Controls.MainControl mc = (ImdbMobile.Controls.MainControl)ControlList[0];
                mc.ShowMainMenu();
                try
                {
                    ((Form1)UI.WindowHandler.ParentForm).Titlebar.textBox1.Text = "";
                }
                catch (Exception e)  {   }
            }
        }
    }
}
