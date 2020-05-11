using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SJMC.CustomControl;
using SJMC.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomRatingBar), typeof(RatingBarRenderer))]
namespace SJMC.Droid.Renderer
{
    public class RatingBarRenderer : ViewRenderer<CustomRatingBar, RatingBar>
    {
        CustomRatingBar element;
        RatingBar ratingBar;

        public RatingBarRenderer(Context context) : base(context)
        {

        }


        protected override void OnElementChanged(ElementChangedEventArgs<CustomRatingBar> e)
        {
            if (e.NewElement == null)
                return;
            element = Element as CustomRatingBar;
            base.OnElementChanged(e);
            if (e.NewElement == null || Control != null)
                return;
            SetStars();
        }

        public void SetStars()
        {
            ratingBar = new RatingBar(Forms.Context, null, element.IsSmallStyle ? Android.Resource.Attribute.RatingBarStyleSmall
                           : Android.Resource.Attribute.RatingBarStyle)
            {
                StepSize = 1.0f,
                NumStars = element.MaxStars,
                Rating = element.Rating,
            };

            RegisterEvents(element.IsReadonly);

            LayerDrawable drawable = (LayerDrawable)ratingBar.ProgressDrawable;
            SetDefaultColor(drawable);
            SetFillColor(drawable);

            SetNativeControl(ratingBar);
        }

        private void RegisterEvents(bool isReadOnly)
        {
            ratingBar.RatingBarChange -= Control_RatingBarChange;
            if (!isReadOnly)
            {
                ratingBar.RatingBarChange += Control_RatingBarChange;
            }
        }

        private void SetFillColor(LayerDrawable drawable)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Android.Support.V4.Graphics.Drawable.DrawableCompat.SetTint(drawable, element.GetFillColor().ToAndroid());
            }
            else
            {
                drawable.GetDrawable(2).SetColorFilter(element.GetFillColor().ToAndroid(),
                 Android.Graphics.PorterDuff.Mode.SrcAtop);
            }
        }

        private void SetDefaultColor(LayerDrawable drawable)
        {
            drawable.GetDrawable(0).SetColorFilter(Android.Graphics.Color.Gray,
                    Android.Graphics.PorterDuff.Mode.SrcAtop);
        }


        private void Control_RatingBarChange(object sender, RatingBar.RatingBarChangeEventArgs e)
        {
            element.OnRatingChanged(e.Rating);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals(CustomRatingBar.RatingProperty.PropertyName))
            {
                RegisterEvents(false);
                ratingBar.Rating = element.Rating;
                RegisterEvents(element.IsReadonly);
                SetFillColor((LayerDrawable)ratingBar.ProgressDrawable);
            }
        }
    }
}