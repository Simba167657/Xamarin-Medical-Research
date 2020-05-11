using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Convertors
{
  
    public class ReportsDateTimeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var val = System.Convert.ToString(value);
                if (!String.IsNullOrEmpty(val))
                {
                    var dateTime = System.Convert.ToDateTime(val);
                    var result = String.Format("{0:dd.MM.yyyy}", dateTime);
                    return result;
                }
            }
            catch (Exception ex)
            {

            }


            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
