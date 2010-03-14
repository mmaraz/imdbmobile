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
    public partial class ViewReview : UI.SlidingList
    {
        public ViewReview(IMDBData.ImdbUserReview iur)
        {
            InitializeComponent();

            UI.ReviewDisplay rd = new ImdbMobile.UI.ReviewDisplay();
            rd.Heading = iur.Summary;
            rd.Parent = this.kListControl1;
            rd.Rating = iur.UserRating;
            rd.Text = iur.FullText + "\n\n" + iur.Username + ",\n" + iur.UserLocation;
            rd.YIndex = 0;
            rd.CalculateHeight(this.kListControl1.Width);
            this.kListControl1.AddItem(rd);
        }
    }
}
