namespace ServerWatchTower.Agent.View
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts a status string ("Red", "Orange", "Green") to a SolidColorBrush.
    /// </summary>
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            switch (status)
            {
                case "Critical":
                    return Brushes.Red;
                case "Warning":
                    return Brushes.Orange;
                case "OK":
                    return Brushes.Green;
                default:
                    return Brushes.Gray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}