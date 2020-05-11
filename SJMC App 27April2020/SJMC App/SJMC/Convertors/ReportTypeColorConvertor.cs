using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Convertors
{
  public  class ReportTypeColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is ReportsModel reportsModel)
            {

                var color = reportsModel.ASDEPT == "R" ? Color.FromHex("#29C3CC") : Color.FromHex("#E20909");
                return color;
            }

            return Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
