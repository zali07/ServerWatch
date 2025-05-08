namespace ServerWatchTower.Agent.View
{
    using ServerWatchTower.Agent.Model;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NoteTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is AlertType type))
            {
                return null;
            }

            switch (type)
            {
                case AlertType.Warning:
                    return "/Cosys.SilverLib.View;Component/Images/32/warning.png";
                case AlertType.Error:
                    return "/Cosys.SilverLib.View;Component/Images/32/error.png";
                case AlertType.Info:
                    return "/Cosys.SilverLib.View;Component/Images/32/info.png";
                default:
                    return "/Cosys.SilverLib.View;Component/Images/32/question.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
