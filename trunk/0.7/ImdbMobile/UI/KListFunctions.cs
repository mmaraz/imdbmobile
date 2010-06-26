using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ImdbMobile.UI
{
    class KListFunctions
    {
        public static void ShowError(string Message, MichyPrima.ManilaDotNetSDK.KListControl KList)
        {
            KList.Items.Clear();
            UI.ErrorButton lb = new ImdbMobile.UI.ErrorButton();
            lb.Icon = global::ImdbMobile.Properties.Resources.Close;
            lb.Parent = KList;
            lb.Text = UI.Translations.GetTranslated("0079") + ":\n" + Message;
            lb.YIndex = 0;
            lb.CalculateHeight();
            KList.Items.Add(lb);
        }

        public static void ShowLoading(string Text, MichyPrima.ManilaDotNetSDK.KListControl KList)
        {
            KList.Items.Clear();
            KList.Visible = true;
            UI.LoadingButton lb = new ImdbMobile.UI.LoadingButton();
            lb.Icon = global::ImdbMobile.Properties.Resources.Trivia;
            lb.Parent = KList;
            lb.Text = Text;
            lb.YIndex = 0;
            lb.CalculateHeight();
            KList.Items.Add(lb);
        }

        public static int GetIndexOf(MichyPrima.ManilaDotNetSDK.ManilaPanelItem mpi, MichyPrima.ManilaDotNetSDK.KListControl KList)
        {
            for (int i = 0; i < KList.Items.Count; i++)
            {
                if (KList.Items[i] == mpi)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
