using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SJMC.CustomControl;
using SJMC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomLabelRegular), typeof(CustomLabelRegularRenderer))]
namespace SJMC.Droid.Renderer
{
    public class CustomLabelRegularRenderer : LabelRenderer
    {
        public CustomLabelRegularRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var label = (TextView)Control; // for example
                   Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "Montserrat-Regular.otf");
                   label.Typeface = font;
                 // do whatever you want to the textField here!
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            }


        }
    }
}