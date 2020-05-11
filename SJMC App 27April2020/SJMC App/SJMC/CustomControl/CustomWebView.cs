using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SJMC.CustomControl
{
   public class CustomWebView:WebView
    {

        public static BindableProperty CustomHtmlProperty =
           BindableProperty.Create(nameof(CustomHtml), typeof(string), typeof(CustomWebView), "", BindingMode.TwoWay);

        public string CustomHtml
        {
            get { return (string)GetValue(CustomHtmlProperty); }
            set { SetValue(CustomHtmlProperty, value); }
        }
    }
}
