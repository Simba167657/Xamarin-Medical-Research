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

[assembly: ExportRenderer(typeof(CustomButtonMedium), typeof(CustomButtonMediumRenderer))]
namespace SJMC.iOS.Renderer
{
    public class CustomButtonMediumRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                    var button = (UIButton)Control;
                // for example
                    button.Font = UIFont.FromName("Montserrat-Medium", button.Font.PointSize);
                    button.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
                    button.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}