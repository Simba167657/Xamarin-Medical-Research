using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Convertors
{
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient Client = new WebClient();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var byteArray = Client.DownloadData(value.ToString());
            return ImageSource.FromStream(() =>
            {
                return new MemoryStream(byteArray);
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class ImageConverter : IValueConverter
    //{
    //    public object Convert(
    //        object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return new BitmapImage(new Uri(value.ToString()));
    //    }

    //    public object ConvertBack(
    //        object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }
    //}
}