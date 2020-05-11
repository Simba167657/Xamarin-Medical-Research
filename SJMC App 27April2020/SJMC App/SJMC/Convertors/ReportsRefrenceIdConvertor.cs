using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Convertors
{
    public class ReportsRefrenceIdConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(value is ReportsModel reportsModel)
            {
                var refrenceId = $"{reportsModel.ASBRCH}-{reportsModel.ASYEAR}-{reportsModel.ASREF}";
                return refrenceId;
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
