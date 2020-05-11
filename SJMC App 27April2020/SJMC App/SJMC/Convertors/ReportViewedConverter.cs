using System;
using System.Globalization;
using Xamarin.Forms;

namespace SJMC.Convertors
{
    public class ReportViewedConverter : IValueConverter
    {
        public ReportViewedConverter()
        {
        }

        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value is bool)
        //    {
        //        var name = "Bold";
        //        return name;
        //    }

        //    return String.Empty;
        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueConverted = (bool)value;
            if (valueConverted)
            {
                var name = (Device.RuntimePlatform == Device.Android ? "Montserrat-Regular.otf#Montserrat-Regular" : "Montserrat-Regular");
                return name;
            }
            else
            {
                var name = (Device.RuntimePlatform == Device.Android ? "Montserrat-Bold.otf#Montserrat-Bold" : "Montserrat-Bold");
                return name;
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
