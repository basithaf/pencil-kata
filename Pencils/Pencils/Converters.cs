﻿using System;
using System.Windows.Data;

namespace Pencils
{
    /// <summary>
    /// A simple converter for displaying/binding a code-behind int as a string
    /// </summary>
    public class IntToStringConverter : IValueConverter {

        // int -> string;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue.ToString();
            }

            return "";
        }

        // string -> int
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string stringValue && int.TryParse(stringValue, out int parsedInt))
            {
                return parsedInt;
            }

            return 0;
        }
    }
}
