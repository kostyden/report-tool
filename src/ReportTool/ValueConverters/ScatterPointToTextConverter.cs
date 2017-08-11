namespace ReportTool.ValueConverters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ScatterPointToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (ScatterPoint)value;
            return $"({point.X}, {point.Y})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
