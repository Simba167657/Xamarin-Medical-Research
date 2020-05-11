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

[assembly: ExportRenderer(typeof(CustomEntryRegular), typeof(CustomEntryRegularRenderer))]
namespace SJMC.Droid.Renderer
{
    public class CustomEntryRegularRenderer : EntryRenderer
    {
        public CustomEntryRegularRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {

                var textView = (TextView)Control;
                // for example
                 Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "Montserrat-Regular.otf");
                 textView.Typeface = font;
                textView.Background = null;

            }
        }
    }
}