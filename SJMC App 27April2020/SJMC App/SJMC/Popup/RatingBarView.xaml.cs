using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SJMC.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBarView : ContentView
    {
        public double Rating { get; set; }
        public RatingBarView()
        {
            InitializeComponent();
        }


        private void OnRatingChanged(object sender, float e)
        {
            customRatingBar.Rating = e;
            Rating = e;
        }
        //private void rating_ValueChanged(object sender, Syncfusion.SfRating.XForms.ValueEventArgs e)
        //{
        //    Rating = rating.Value;
        //}
    }
}