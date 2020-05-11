using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SJMC.CustomControl;
using SJMC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace SJMC.Droid.Renderer
{
    public class CustomPickerRenderer : PickerRenderer
    {
        public CustomPickerRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement != null)
            {
                var customPicker = e.NewElement as CustomPicker;

                Control.TextSize *= (customPicker.FontSize * 0.01f);
                Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));

                //Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.TitleColor));
                //  Control.SetHint(Android.Graphics.Color.ParseColor(customPicker.TitleColor));
            }
            if (Control != null)
            {
                var customPicker = e.NewElement as CustomPicker;

                Control.Background = null;
                Element.BackgroundColor = Color.Transparent;
                Control.Gravity = GravityFlags.Start;
                Control.TextSize = 12;

            }

        }
    }
}