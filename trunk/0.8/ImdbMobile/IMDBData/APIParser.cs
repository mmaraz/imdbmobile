using System;

namespace ImdbMobile.IMDBData
{
    public class APIEvent : EventArgs
    {
        public string EventData { get; set; }

        public APIEvent(string EventData)
        {
            this.EventData = EventData;
        }
    }

    public class APIParser
    {
        public event EventHandler DownloadingData;
        public event EventHandler DownloadComplete;
        public event EventHandler ParsingData;
        public event EventHandler ParsingComplete;
        public event EventHandler Error;

        public APIParser()
        {
            
        }

        protected void OnError(string Message)
        {
            if (this.Error != null)
            {
                this.Error(this, new APIEvent(Message));
            }
        }

        protected void OnDownloadingData()
        {
            if (DownloadingData != null)
            {
                DownloadingData(this, null);
            }
        }

        protected void OnDownloadComplete(string DownloadedData)
        {
            if (DownloadComplete != null)
            {
                APIEvent ae = new APIEvent(DownloadedData);
                DownloadComplete(this, ae);
            }
        }

        protected void OnParsingData()
        {
            if (ParsingData != null)
            {
                ParsingData(this, null);
            }
        }

        protected void OnParsingComplete()
        {
            if (ParsingComplete != null)
            {
                ParsingComplete(this, null);
            }
        }
    }
}
