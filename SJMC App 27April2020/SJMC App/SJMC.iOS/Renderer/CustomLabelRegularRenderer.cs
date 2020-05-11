using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SJMC.CustomControl;
using SJMC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabelRegular), typeof(CustomLabelRegularRenderer))]
namespace SJMC.iOS.Renderer
{
    public class CustomLabelRegularRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var label = (UILabel)Control;
                label.Font = UIFont.FromName("Montserrat-Regular", label.Font.PointSize); 
            }
        }

    }
}