using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using SJMC.CustomControl;
using SJMC.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace SJMC.iOS.Renderer
{
    public class CustomPickerRenderer : PickerRenderer
    {
        public static void Init() { }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.Layer.BorderWidth = 0;
            Control.Font = UIFont.SystemFontOfSize(13);
            Control.BorderStyle = UITextBorderStyle.None;
            Control.LeftView = new UIView(new CGRect(0, 0, 5, 0));
            Control.LeftViewMode = UITextFieldViewMode.Always;
        }

    }
}