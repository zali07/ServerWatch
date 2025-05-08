namespace ServerWatchTower.Agent.View
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class DateTimeSimplifyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Date == dateTime)
                {
                    return dateTime.ToString("dd.MM.yyyy");
                }
                else
                {
                    return dateTime.ToString("dd.MM.yyyy HH:mm");
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
