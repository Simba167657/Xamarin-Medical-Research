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

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace SJMC.iOS.Renderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        private string Placeholder { get; set; }

        // TODO: This is On Element Change event
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            var element = this.Element as CustomEditor;

            if (Control != null && element != null)
            {
                Placeholder = element.Placeholder;
                Control.TextColor = UIColor.DarkGray;
                Control.Text = Placeholder;
                Control.BackgroundColor = UIColor.FromCGColor(Color.FromHex("#F8F8F8").ToCGColor());

                Control.ShouldBeginEditing += (UITextView textView) => {
                    if (textView.Text == Placeholder)
                    {
                        textView.Text = "";
                        textView.TextColor = UIColor.Black; // Text Color
                    }

                    return true;
                };

                Control.ShouldEndEditing += (UITextView textView) => {
                    if (textView.Text == "")
                    {
                        textView.Text = Placeholder;
                        // textView.TextColor = UIColor.DarkGray; // Placeholder Color
                        textView.TextColor = UIColor.FromRGB(166, 166, 166);
                    }

                    return true;
                };
            }
        }
    }
}