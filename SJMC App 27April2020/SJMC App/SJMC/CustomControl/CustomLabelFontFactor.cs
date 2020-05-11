using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SJMC.CustomControl
{
    public class CustomLabelFontFactor : Label
    {

        public static readonly BindableProperty FontSizeFactorProperty =
            BindableProperty.Create(
            "FontSizeFactor", typeof(double), typeof(CustomLabelFontFactor),
            defaultValue: 1.0, propertyChanged: OnFontSizeFactorChanged);

        public double FontSizeFactor
        {
            get { return (double)GetValue(FontSizeFactorProperty); }
            set { SetValue(FontSizeFactorProperty, value); }
        }

        private static void OnFontSizeFactorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomLabelFontFactor)bindable).OnFontSizeChangedImpl();
        }

        public static readonly BindableProperty NamedFontSizeProperty =
            BindableProperty.Create(
            "NamedFontSize", typeof(NamedSize), typeof(CustomLabelFontFactor),
            defaultValue: NamedSize.Small, propertyChanged: OnNamedFontSizeChanged);

        public NamedSize NamedFontSize
        {
            get { return (NamedSize)GetValue(NamedFontSizeProperty); }
            set { SetValue(NamedFontSizeProperty, value); }
        }

        private static void OnNamedFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CustomLabelFontFactor)bindable).OnFontSizeChangedImpl();
        }

        protected virtual void OnFontSizeChangedImpl()
        {
            if (this.FontSizeFactor != 1)
                this.FontSize = (this.FontSizeFactor * Device.GetNamedSize(NamedFontSize, typeof(Label)));
        }
    }
}
