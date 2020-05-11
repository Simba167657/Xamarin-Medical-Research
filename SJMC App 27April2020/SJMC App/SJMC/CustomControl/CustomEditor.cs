using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SJMC.CustomControl
{
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create<CustomEditor, string>(view => view.Placeholder, String.Empty);

        public CustomEditor()
        {
        }

        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }
    }
}
