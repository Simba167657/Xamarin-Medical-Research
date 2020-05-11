using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Convertors
{
 
    public class ReportsDaysAgoConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            try
            {
                var val = System.Convert.ToString(value);
                if (!String.IsNullOrEmpty(val))
                {
                    var dateTime = System.Convert.ToDateTime(val);

                    var diff = System.Convert.ToInt32((DateTime.Now- dateTime).TotalDays);

                    if (diff == 0)
                    {
                        return "today";
                    }
                    if (diff == 1)
                    {
                        return "yesterday";
                    }

                    if (diff > 1 && diff <365)
                    {
                        return $"{diff} days ago";
                    }

                    if (diff > 365 )
                    {
                        var yearCount = System.Convert.ToInt32(diff/365);
                        if (yearCount == 1)
                        {
                            return "1 year ago";
                        }
                        else
                        {
                            return $"{yearCount} years ago";
                        }
                    }
                    
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
