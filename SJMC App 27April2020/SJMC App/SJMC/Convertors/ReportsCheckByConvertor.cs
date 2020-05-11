using SJMC.Helpers;
using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Convertors
{
    public class ReportsCheckByConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = false;


            if (Constants.UserProfile != null)
            {
                if (value is ReportsModel reportsModel)
                {
                    switch (Constants.UserProfile.Role)
                    {
                        case "patient":
                            isChecked= !String.IsNullOrEmpty(reportsModel.ASPCHKBY);
                            break;

                        case "doctor":
                            isChecked = !String.IsNullOrEmpty(reportsModel.ASDCHKBY);
                            break;

                        case "agent":
                            isChecked = !String.IsNullOrEmpty(reportsModel.ASACHKBY);
                            break;

                        case "guarantor":
                            isChecked = !String.IsNullOrEmpty(reportsModel.ASGCHKBY);
                            break;

                    }
                   
                }
            }

            if (isChecked)
            {
                return "check.png";
            }
            return "checkbox.png";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
