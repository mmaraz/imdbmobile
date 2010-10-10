using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ImdbMobile.Controls
{
    public partial class AboutControl : UI.SlidingList
    {
        public AboutControl()
        {
            InitializeComponent();

            UI.TextDisplay Title = new UI.TextDisplay();
            Title.Heading = "IMDb Mobile v0.7.6";
            Title.Parent = this.kListControl1;
            Title.Text = "Developed By: Blade0rz\nUpdated By: TvdA\n\nThis project could not have existed without the help of:";
            Title.CalculateHeight();
            this.kListControl1.Items.Add(Title);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem IMDB = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            IMDB.MainText = "IMDb";
            IMDB.Parent = this.kListControl1;
            IMDB.SecondaryText = "The best website on the planet.";
            IMDB.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(IMDB_OnClick);
            this.kListControl1.Items.Add(IMDB);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem Kodos = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            Kodos.MainText = "Kodos05";
            Kodos.Parent = this.kListControl1;
            Kodos.SecondaryText = "Creator of IMDB-PHP iPhone API";
            Kodos.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(Kodos_OnClick);
            this.kListControl1.Items.Add(Kodos);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem MichyPrima = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            MichyPrima.MainText = "MichyPrima";
            MichyPrima.Parent = this.kListControl1;
            MichyPrima.SecondaryText = "Manila.NET SDK";
            MichyPrima.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(MichyPrima_OnClick);
            this.kListControl1.Items.Add(MichyPrima);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem Newton = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            Newton.MainText = "James Newton-King";
            Newton.Parent = this.kListControl1;
            Newton.SecondaryText = "Json.NET";
            Newton.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(Newton_OnClick);
            this.kListControl1.Items.Add(Newton);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem OpenNet = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            OpenNet.MainText = "OpenNETCF Consulting";
            OpenNet.Parent = this.kListControl1;
            OpenNet.SecondaryText = "OpenNET CF DLL";
            OpenNet.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(OpenNet_OnClick);
            this.kListControl1.Items.Add(OpenNet);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem XDA = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            XDA.MainText = "XDA-Developers";
            XDA.Parent = this.kListControl1;
            XDA.SecondaryText = "The second best website on the planet.";
            XDA.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(XDA_OnClick);
            this.kListControl1.Items.Add(XDA);

            MichyPrima.ManilaDotNetSDK.ManilaPanelItem Omer = new MichyPrima.ManilaDotNetSDK.ManilaPanelItem();
            Omer.MainText = "Omer van Kloeten";
            Omer.Parent = this.kListControl1;
            Omer.SecondaryText = "MeasureStringExtended Function";
            Omer.OnClick += new MichyPrima.ManilaDotNetSDK.ManilaPanelItem.OnClickEventHandler(Omer_OnClick);
            this.kListControl1.Items.Add(Omer);
        }

        void Omer_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://weblogs.asp.net/okloeten/archive/2004/03/30/103384.aspx", "");
        }

        void XDA_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://www.xda-developers.com/", "");
        }

        void OpenNet_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://www.opennetcf.com/", "");
        }

        void Newton_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://james.newtonking.com/pages/json-net.aspx", "");
        }

        void IMDB_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://www.imdb.com", "");   
        }

        void Kodos_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/imdb-php/", "");
        }

        void MichyPrima_OnClick(object Sender)
        {
            System.Diagnostics.Process.Start("http://forum.xda-developers.com/showthread.php?t=566188", "");
        }
    }
}
