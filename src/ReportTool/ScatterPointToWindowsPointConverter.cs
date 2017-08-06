namespace ReportTool
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ScatterPointToWindowsPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var scatterPoint = (ScatterPoint)value;
            return new Point(scatterPoint.X, scatterPoint.Y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
