using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace LToDo
{
    public class BoolenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)parameter)
            {
                case "IsEdit":
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                case "Height":
                    return (bool)value ? 200 : 32;
                case "Opacity":
                    return (bool)value ? 0.9 : 0.9;
                case "TextWrapping":
                    return (bool)value ? TextWrapping.Wrap : TextWrapping.NoWrap;
                case "AcceptsReturn":
                    return (bool)value ? true : false;
                case "EnlargeVisibility":
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                case "ButtonVisibility":
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
